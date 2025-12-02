using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Common
{
    public class AppConfig
    {
        public string AppVersion { get; set; }
        public string PostAddress { get; set; }
        public string CommonAPIPostAddress { get; set; }
        public string APIIncomeURL { get; set; }
    }

    public class EnvironmentUrls
    {
        public string PostAddress { get; set; }
        public string CommonAPIPostAddress { get; set; }
        public string APIIncomeURL { get; set; }
    }

    public class EnvironmentConfig
    {
        public EnvironmentUrls Development { get; set; }
        public EnvironmentUrls Staging { get; set; }
        public EnvironmentUrls Production { get; set; }
    }


    public enum AppEnvironment
    {
        Development,
        Staging,
        Production
    }


}
