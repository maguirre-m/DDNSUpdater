using System.Text.Json.Serialization;
using static Core.CloudFlare.DNSRecords.UpdateDNSRecordResponse;

namespace Core.CloudFlare.DNSRecords
{
    public class ListDNSRecordsResponse : ResponseBase
    {
        [JsonPropertyName("result")]
        public List<UpdateDNSRecordResult> Result { get; set; }
        [JsonPropertyName("result_info")]
        public ResultInfo ResultInfo { get; set; }
    }
}
