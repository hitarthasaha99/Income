using Income.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Common.Config
{
    public class ApiConfig
    {
        public Dictionary<AppEnvironment, EnvironmentConfig> Environments { get; set; } = new();
    }
}
