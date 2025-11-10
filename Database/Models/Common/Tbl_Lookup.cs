using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Models.Common
{
    public class Tbl_Lookup
    {
        public int id { get; set; }
        public string? lookup_type { get; set; }
        public string? title { get; set; }
    }
}
