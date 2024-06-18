using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CloudFlare
{
    public class RequestBase
    {
        public string URL { get; set; }
        public string AuthKey { get; set; }
    }
}
