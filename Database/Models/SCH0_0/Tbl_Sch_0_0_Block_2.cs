using Income.Database.Models.Common;
using SQLite;

namespace Income.Database.Models.SCH0_0
{
    public class Tbl_Sch_0_0_Block_2 : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public string? enumerator_name { get; set; }
        public DateTime? field_work_start_date { get; set; }
        public DateTime? field_work_end_date { get; set; }
        public int? time_taken { get; set; }
    }
}
