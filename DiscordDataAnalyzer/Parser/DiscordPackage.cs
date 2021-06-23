using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DiscordDataAnalyzer.Parser.Utils;

namespace DiscordDataAnalyzer.Parser
{
    public class DiscordPackage
    {
        private readonly string _path;

        public DiscordPackage(string path)
        {
            _path = path;

            Channels = new Dictionary<string, DiscordChannel>();
        }

        public Dictionary<string, DiscordChannel> Channels { get; }

        public async Task ParseAsync()
        {
            await ParseMessagesAsync();
            await ParseReportingAsync();
            // await ParseTrustAndSafetyAsync();
        }

        private async Task ParseMessagesAsync()
        {
            // Parse channel index.
            await using (var stream = File.OpenRead(Path.Combine(_path, "messages", "index.json")))
            {
                var messagesIndex = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream);
                if (messagesIndex != null)
                {
                    foreach (var (channelId, channelName) in messagesIndex)
                    {
                        Channels.Add(channelId, DiscordChannel.CreateFromName(channelId, channelName));
                    }
                }
            }
            
            // TODO: Parse channels data.
        }

        private async Task ParseReportingAsync()
        {
            var path = Path.Combine(_path, "activity", "reporting");
            var filter = "events-*.json";
            
            await foreach (var item in PipeUtils.ReadFromFilesAsync<DiscordEventReporting>(path, filter))
            {
                if (item.Channel != null)
                {
                    if (!Channels.TryGetValue(item.Channel, out var channel))
                    {
                        channel = DiscordChannel.CreateFromId(item.Channel);
                    }
                        
                    channel.OnEvent(item);
                }
            }
        }

        private Task ParseTrustAndSafetyAsync()
        {
            return Task.CompletedTask;
        }
    }
}