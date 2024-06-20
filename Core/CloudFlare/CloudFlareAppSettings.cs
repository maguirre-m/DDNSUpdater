using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CloudFlare
{
    public class CloudFlareAppSettings
    {
        public const string SectionName = "CloudFlare";

        public string RecordName { get; set; }
    }
}
