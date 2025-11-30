using Income.Database.Models.Common;
using SQLite;

namespace Income.Database.Models.SCH0_0
{
    public class Tbl_Sch_0_0_Block_5 : Tbl_Base
    {
        public int? serial_number { get; set; }
        public bool? is_selected { get; set; }
        public bool? IsChecked { get; set; }
        public double? Percentage { get; set; }
    }
}
