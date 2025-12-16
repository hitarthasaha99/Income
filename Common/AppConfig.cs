using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Common
{
    public class AppConfig
    {
        public string AppVersion { get; set; } = string.Empty;
        public string PostAddress { get; set; } = string.Empty;
        public string CommonAPIPostAddress { get; set; } = string.Empty;
        public string APIIncomeURL { get; set; } = string.Empty;
    }
}
