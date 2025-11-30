using Income.Database.Models.Common;
using SQLite;

namespace Income.Database.Models.SCH0_0
{
    public class Tbl_Sch_0_0_Block_2_1 : Tbl_Base
    {
        public int? serial_no { get; set; }
        public double? percentage { get; set; } = 0;
        public string hamlet_name { get; set; } = string.Empty;
        public bool is_selected { get; set; } = false;
    }
}
