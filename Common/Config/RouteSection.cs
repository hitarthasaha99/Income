using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Common.Config
{
    public class RouteSection
    {
        public string Login { get; set; } = string.Empty;
        public string FetchFsuList { get; set; } = string.Empty;
        public string FetchSavedResponses { get; set; } = string.Empty;
        public string SaveResponse { get; set; } = string.Empty;
        public string Logout { get; set; } = string.Empty;
        public string UpdateListing { get; set; } = string.Empty;
    }
}
