using System.Text.Json.Serialization;

namespace Core.CloudFlare.DNSRecords
{
    public class UpdateDNSRecordRequest : RequestBase
    {
        [JsonPropertyName("data")]
        public Data RequestData {  get; set; }

        public class Data
        {
            [JsonPropertyName("content")]
            public string Content { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("proxied")]
            public bool? Proxied { get; set; }
            [JsonPropertyName("type")]
            public string Type { get; set; }
            [JsonPropertyName("comment")]
            public string? Comment { get; set; }
            [JsonPropertyName("id")]
            public string Id { get; set; }
            [JsonPropertyName("tags")]
            public string[]? Tags { get; set; }
            [JsonPropertyName("ttl")]
            public int? TTL { get; set; }
            [JsonPropertyName("zone_id")]
            public string ZoneId { get; set; }
        }
    }
}
