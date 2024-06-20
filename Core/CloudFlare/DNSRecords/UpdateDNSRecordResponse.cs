using System.Text.Json.Serialization;

namespace Core.CloudFlare.DNSRecords
{
    public class UpdateDNSRecordResponse : ResponseBase
    {
        public UpdateDNSRecordResult Result { get; set; }

        public class UpdateDNSRecordResult
        {
            public string Content { get; set; }
            public string Name { get; set; }
            public bool? Proxied { get; set; }
            public string Type { get; set; }
            public string? Comment { get; set; }
            [JsonPropertyName("created_on")]
            public DateTime CreatedOn { get; set; }
            public string Id { get; set; }
            public bool Locked { get; set; }
            public Metadata Meta { get; set; }
            [JsonPropertyName("modified_on")]
            public DateTime ModifiedOn { get; set; }
            public bool Proxiable { get; set; }
            public List<string>? Tags { get; set; }
            public int? TTL {  get; set; }
            [JsonPropertyName("zone_id")]
            public string? ZoneId { get; set; }
            [JsonPropertyName("zone_name")]
            public string ZoneName { get; set; }
        }

        public class Metadata
        {
            [JsonPropertyName("auto_added")]
            public bool AutoAdded { get; set; }
            public string Source { get; set; }
        }
    }
}
