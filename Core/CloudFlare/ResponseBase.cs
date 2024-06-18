using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CloudFlare
{
    public class ResponseBase
    {
        public List<Error> Errors { get; set; }
        public List<ResponseMessage> Messages { get; set; }
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
}
