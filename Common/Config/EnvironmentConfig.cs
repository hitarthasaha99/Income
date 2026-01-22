using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Common.Config
{
    public class EnvironmentConfig
    {
        public BaseUrlSection BaseUrls { get; set; } = new();
        public RouteSection Routes { get; set; } = new();
        public string APP_Version { get; set; } = string.Empty;
    }
}
