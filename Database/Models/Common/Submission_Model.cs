using Income.Database.Models.SCH0_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Models.Common
{
    public class Submission_Model
    {
        public Tbl_Sch_0_0_Block_0_1? IncomeSch00block0 { get; set; }
        public List<Tbl_Sch_0_0_Block_2_1>? IncomeSch00block2_1 { get; set; }
        public List<Tbl_Sch_0_0_Block_2_2>? IncomeSch00block2_2 { get; set; }
        public List<Tbl_Sch_0_0_Block_3>? IncomeSch00block3 { get; set; }
        public Tbl_Sch_0_0_Block_4? IncomeSch00block_4 { get; set; }
        public List<Tbl_Sch_0_0_Block_5>? IncomeSch00block_5 { get; set; }
        public List<Tbl_Sch_0_0_Block_7>? IncomeSch00block_7 { get; set; }
        public Tbl_Sch_0_0_FieldOperation? IncomeSch00block_11 { get; set; }
        public List<Tbl_Warning>? IncomeSch00WarningList { get; set; }
        public List<SCH_HIS_Model>? IncomeSchHISDto {  get; set; }
    }

    public class SCH_HIS_Model
    {

    }
}
