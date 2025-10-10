using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Models.Common
{
    public class Validation_Model
    {
        public string field_name { get; set; } = string.Empty;
        public string field_type { get; set; } = string.Empty;
        public string reg_exp { get; set; } = string.Empty;
        public int max_val { get; set; } = 0;
        public int min_val { get; set; } = 0;
        public bool is_stringify_val { get; set; } = false;
    }
}
