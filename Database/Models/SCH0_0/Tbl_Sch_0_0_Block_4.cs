using Income.Database.Models.Common;
using SQLite;

namespace Income.Database.Models.SCH0_0
{
    public class Tbl_Sch_0_0_Block_4 : Tbl_Base
    {
        public int? sample_su_number { get; set; }
        public double? approximate_population_su { get; set; }
        public int? number_of_sub_division_of_su_to_be_formed { get; set; }
    }
}
