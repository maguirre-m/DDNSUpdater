using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.CloudFlare
{
    public class ResponseBase
    {
        [JsonPropertyName("errors")]
        public List<Error> Errors { get; set; }
        [JsonPropertyName("messages")]
        public List<ResponseMessage> Messages { get; set; }
        [JsonPropertyName("success")]
        public bool Success { get; set; }        
    }

    public class Error
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }

    public class ResponseMessage
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }

    public class ResultInfo
    {
        public int Count { get; set; }
        public int Page { get; set; }
        [JsonPropertyName("per_page")]
        public int RecordsPerPage { get; set; }
        [JsonPropertyName("total_count")]
        public int TotalRecords { get; set; }
    }
}
