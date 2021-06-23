using System;
using System.Collections.Generic;

namespace DiscordDataAnalyzer.Parser
{
    public class DiscordChannel
    {
        private DiscordChannel(string id, string name)
        {
            Id = id;
            Name = name;
        }
        
        private DiscordChannel(string id, string name, string discriminator)
        {
            Id = id;
            Name = name;
            Discriminator = discriminator;
        }
        
        public string Id { get; }
        public string Name { get; }
        public string Discriminator { get; }

        public DateTimeOffset? MessageFirst { get; private set; }
        public DateTimeOffset? MessageLast { get; private set; }
        public Dictionary<int, int> MessageHours { get; } = new Dictionary<int, int>();
        
        public bool IsPrivate => Discriminator != null;

        public Dictionary<string, int> EventCount { get; } = new Dictionary<string, int>()
        {
            ["send_message"] = 0
        };

        public void OnEvent(DiscordEventReporting item)
        {
            if (EventCount.ContainsKey(item.EventType))
            {
                EventCount[item.EventType] += 1;
            }
            else
            {
                EventCount[item.EventType] = 1;
            }

            switch (item.EventType)
            {
                case "send_message":
                    if (MessageFirst == null || MessageFirst > item.Timestamp)
                    {
                        MessageFirst = item.Timestamp;
                    }
                    
                    if (MessageLast == null || MessageLast < item.Timestamp)
                    {
                        MessageLast = item.Timestamp;
                    }
                    
                    var hourOffset = TimeZoneInfo.Local.GetUtcOffset(item.Timestamp);
                    var hour = (item.Timestamp.Hour + hourOffset.Hours) % 24;
                    
                    if (MessageHours.ContainsKey(hour))
                    {
                        MessageHours[hour] += 1;
                    }
                    else
                    {
                        MessageHours[hour] = 1;
                    }
                    break;
            }
        }

        public static DiscordChannel CreateFromId(string id)
        {
            return new DiscordChannel(id, null);
        }

        public static DiscordChannel CreateFromName(string id, string name)
        {
            if (name != null && name.StartsWith("Direct Message with "))
            {
                name = name.Replace("Direct Message with ", string.Empty);

                var userName = name.Substring(0, name.Length - 5);
                var userDiscriminator = name.Substring(name.Length - 4, 4);
                
                // Direct message.
                return new DiscordChannel(id, userName, userDiscriminator);
            }
            
            // Channel.
            return new DiscordChannel(id, name);
        }

        public override string ToString()
        {
            return IsPrivate 
                ? $"{Name}#{Discriminator}" 
                : $"#{Name}";
        }
    }
}