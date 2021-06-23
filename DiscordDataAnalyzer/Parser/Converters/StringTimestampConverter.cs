using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordDataAnalyzer.Parser.Converters
{
    public class StringTimestampConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            if (str != null && str.StartsWith('"') && str.EndsWith('"'))
            {
                return DateTimeOffset.Parse(str.Substring(1, str.Length - 2), CultureInfo.InvariantCulture);
            }
            
            throw new Exception($"Unsupported date {str}");
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset dateTimeValue, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}