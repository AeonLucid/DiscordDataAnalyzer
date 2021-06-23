using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Text.Json;

namespace DiscordDataAnalyzer.Parser.Utils
{
    // https://stackoverflow.com/a/65604962
    public class PipeUtils
    {
        private static readonly byte[] NewLineChars = {(byte) '\r', (byte) '\n'};
        private static readonly byte[] WhiteSpaceChars = {(byte) '\r', (byte) '\n', (byte) ' ', (byte) '\t'};

        public static async IAsyncEnumerable<T> ReadFromFilesAsync<T>(string path, string filter)
        {
            foreach (var eventsFile in Directory.EnumerateFiles(path, filter))
            {
                await using (var stream = File.OpenRead(eventsFile))
                {
                    var reader = PipeReader.Create(stream);

                    await foreach (var item in ReadItemsAsync<T>(reader))
                    {
                        yield return item;
                    }
                }
            }
        }

        private static async IAsyncEnumerable<TValue> ReadItemsAsync<TValue>(PipeReader pipeReader, JsonSerializerOptions jsonOptions = null)
        {
            while (true)
            {
                var result = await pipeReader.ReadAsync();
                var buffer = result.Buffer;
                var isCompleted = result.IsCompleted;
                var bufferPosition = buffer.Start;
                while (true)
                {
                    var (value, advanceSequence) = TryReadNextItem<TValue>(buffer, ref bufferPosition, isCompleted, jsonOptions);
                    if (value != null)
                    {
                        yield return value;
                    }

                    if (advanceSequence)
                    {
                        pipeReader.AdvanceTo(bufferPosition, buffer.End); //advance our position in the pipe
                        break;
                    }
                }

                if (isCompleted)
                {
                    yield break;
                }
            }
        }

        private static (TValue, bool) TryReadNextItem<TValue>(ReadOnlySequence<byte> sequence, ref SequencePosition sequencePosition, bool isCompleted, JsonSerializerOptions jsonOptions)
        {
            var reader = new SequenceReader<byte>(sequence.Slice(sequencePosition));
            while (!reader.End) // loop until we've come to the end or read an item
            {
                if (reader.TryReadToAny(out ReadOnlySpan<byte> itemBytes, NewLineChars, advancePastDelimiter: true))
                {
                    sequencePosition = reader.Position;
                    if (itemBytes.TrimStart(WhiteSpaceChars).IsEmpty)
                    {
                        continue;
                    }

                    return (JsonSerializer.Deserialize<TValue>(itemBytes, jsonOptions), false);
                }
                else if (isCompleted)
                {
                    // read last item
                    var remainingReader = sequence.Slice(reader.Position);
                    using var memoryOwner = MemoryPool<byte>.Shared.Rent((int) reader.Remaining);
                    remainingReader.CopyTo(memoryOwner.Memory.Span);
                    reader.Advance(remainingReader.Length); // advance reader to the end
                    sequencePosition = reader.Position;
                    if (!itemBytes.TrimStart(WhiteSpaceChars).IsEmpty)
                    {
                        return (JsonSerializer.Deserialize<TValue>(memoryOwner.Memory.Span, jsonOptions), true);
                    }
                    else
                    {
                        return (default, true);
                    }
                }
                else
                {
                    // no more items in sequence
                    break;
                }
            }

            // PipeReader needs to read more
            return (default, true);
        }
    }
}