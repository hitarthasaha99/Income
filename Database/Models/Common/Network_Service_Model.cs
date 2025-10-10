using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Models.Common
{
    public class Network_Service_Model
    {
        public string? network_uri { get; set; }
        public string? method_type { get; set; }
        public string? posted_data { get; set; }
    }

    public class apk_version_model
    {
        public int? apk_major_version { get; set; }
        public int? apk_minor_version { get; set; }
    }
}
