using System;

namespace DiscordDataAnalyzer.Parser
{
    public class DiscordDevice
    {
        public string Browser { get; set; }
        public string Device { get; set; }
        public string Ip { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public string Key()
        {
            return Browser + Device + Ip;
        }
    }
}