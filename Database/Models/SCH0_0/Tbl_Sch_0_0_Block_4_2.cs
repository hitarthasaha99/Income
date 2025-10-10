using Income.Database.Models.Common;
using SQLite;

namespace Income.Database.Models.SCH0_0
{
    public class Tbl_Sch_0_0_Block_4_2 : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public bool is_selected { get; set; } = false;
        public int? serial_number { get; set; }
        public string? serial_no_of_hamlets_in_su { get; set; }
        public decimal? population_percentage { get; set; }
        public int SerialNumber { get; set; } 
        public double Percentage { get; set; }
        public string? HamletName { get; set; }
        public int? HamletSerialNumbers { get; set; }
        public string SampleHgSbNumber { get; set; } = string.Empty;
        public bool IsChecked { get; set; } = false;
    }
}
