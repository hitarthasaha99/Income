using Income.Database.Models.Common;
using SQLite;

namespace Income.Database.Models.SCH0_0
{
    public class Tbl_Sch_0_0_Block_4_3 : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public int? serial_number { get; set; }
        public decimal? population_percentage { get; set; }
        public bool? is_selected { get; set; }
        public bool? IsChecked { get; set; }
        public int? SerialNumber { get; set; }
        public double? Percentage { get; set; }
    }
}
