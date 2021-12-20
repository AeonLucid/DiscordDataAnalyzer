using System;
using System.Text.Json.Serialization;
using DiscordDataAnalyzer.Parser.Converters;

namespace DiscordDataAnalyzer.Parser
{
    public class DiscordEventReporting
    {
        [JsonPropertyName("event_type")]
        public string EventType { get; set; }

        [JsonPropertyName("event_id")]
        public string EventId { get; set; }

        [JsonPropertyName("event_source")]
        public string EventSource { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("freight_hostname")]
        public string FreightHostname { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("day")]
        public string Day { get; set; }

        [JsonPropertyName("chosen_locale")]
        public string ChosenLocale { get; set; }

        [JsonPropertyName("detected_locale")]
        public string DetectedLocale { get; set; }

        [JsonPropertyName("user_is_authenticated")]
        public bool UserIsAuthenticated { get; set; }

        [JsonPropertyName("browser")]
        public string Browser { get; set; }

        [JsonPropertyName("device")]
        public string Device { get; set; }

        [JsonPropertyName("cfduid")]
        public string Cfduid { get; set; }

        [JsonPropertyName("device_vendor_id")]
        public string DeviceVendorId { get; set; }

        [JsonPropertyName("os")]
        public string Os { get; set; }

        [JsonPropertyName("os_version")]
        public string OsVersion { get; set; }

        [JsonPropertyName("client_build_number")]
        public string ClientBuildNumber { get; set; }

        [JsonPropertyName("release_channel")]
        public string ReleaseChannel { get; set; }

        [JsonPropertyName("client_version")]
        public string ClientVersion { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        [JsonPropertyName("region_code")]
        public string RegionCode { get; set; }

        [JsonPropertyName("time_zone")]
        public string TimeZone { get; set; }

        [JsonPropertyName("isp")]
        public string Isp { get; set; }

        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }

        [JsonPropertyName("channel")]
        public string Channel { get; set; }

        [JsonPropertyName("channel_type")]
        public string ChannelType { get; set; }

        [JsonPropertyName("is_friend")]
        public bool IsFriend { get; set; }

        [JsonPropertyName("private")]
        public bool Private { get; set; }

        [JsonPropertyName("num_attachments")]
        public string NumAttachments { get; set; }

        [JsonPropertyName("max_attachment_size")]
        public string MaxAttachmentSize { get; set; }

        [JsonPropertyName("length")]
        public string Length { get; set; }

        [JsonPropertyName("word_count")]
        public string WordCount { get; set; }

        [JsonPropertyName("mention_everyone")]
        public bool MentionEveryone { get; set; }

        [JsonPropertyName("emoji_unicode")]
        public string EmojiUnicode { get; set; }

        [JsonPropertyName("emoji_custom")]
        public string EmojiCustom { get; set; }

        [JsonPropertyName("emoji_custom_external")]
        public string EmojiCustomExternal { get; set; }

        [JsonPropertyName("emoji_managed")]
        public string EmojiManaged { get; set; }

        [JsonPropertyName("emoji_managed_external")]
        public string EmojiManagedExternal { get; set; }

        [JsonPropertyName("emoji_animated")]
        public string EmojiAnimated { get; set; }

        [JsonPropertyName("emoji_only")]
        public bool EmojiOnly { get; set; }

        [JsonPropertyName("num_embeds")]
        public string NumEmbeds { get; set; }

        [JsonPropertyName("attachment_ids")]
        public object[] AttachmentIds { get; set; }

        [JsonPropertyName("has_spoiler")]
        public bool HasSpoiler { get; set; }

        [JsonPropertyName("probably_has_markdown")]
        public bool ProbablyHasMarkdown { get; set; }

        [JsonPropertyName("user_is_bot")]
        public bool UserIsBot { get; set; }

        // [JsonPropertyName("sticker_ids")]
        // public JsonElement[] StickerIds { get; set; }

        [JsonPropertyName("message_type")]
        public string MessageType { get; set; }

        [JsonPropertyName("system_locale")]
        public string SystemLocale { get; set; }

        // [JsonPropertyName("components")]
        // public JsonElement[] Components { get; set; }

        [JsonPropertyName("client_send_timestamp")]
        public string ClientSendTimestamp { get; set; }

        [JsonPropertyName("client_track_timestamp")]
        public string ClientTrackTimestamp { get; set; }

        [JsonPropertyName("timestamp")]
        [JsonConverter(typeof(StringTimestampConverter))]
        public DateTimeOffset Timestamp { get; set; }
    }
}