using Income.Database.Models.HIS_2026;
using Income.Database.Models.SCH0_0;
using SQLite;
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
        public Tbl_Sch_0_0_FieldOperation? IncomeSch00FieldOp { get; set; }
        public List<Tbl_Warning>? IncomeSch00WarningList { get; set; }
        public List<SCH_HIS_Model>? IncomeSchHISDto {  get; set; }
    }

    public class SCH_HIS_Model
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public int hhd_id { get; set; }
        public int hhd_status { get; set; }
        public Tbl_Block_1? IncomeBlock1 { get; set; }
        public Tbl_Block_FieldOperation? IncomeBlockFieldOp { get; set; }
        public List<Tbl_Block_3>? IncomeBlock3 { get; set; }
        public Tbl_Block_4? IncomeBlock4 { get; set; }
        public List<Tbl_Block_4_Q5>? IncomeBlock4Q5 { get; set; }
        public List<Tbl_Block_5>? IncomeBlock5 { get; set; }
        public List<Tbl_Block_6>? IncomeBlock6 { get; set; }
        public List<Tbl_Block_7a>? IncomeBlock7A { get; set; }
        public List<Tbl_Block_7a_1>? IncomeBlock7AQ1 { get; set; }
        public List<Tbl_Block_7b>? IncomeBlock7B { get; set; }
        public List<Tbl_Block_7c_NIC>? IncomeBlock7CQ1 { get; set; }
        public List<Tbl_Block_7c_Q10>? IncomeBlock7CQ10 { get; set; }
        public List<Tbl_Block_7c>? IncomeBlock7C { get; set; }
        public List<Tbl_Block_7d>? IncomeBlock7D { get; set; }
        public List<Tbl_Block_8>? IncomeBlock8 { get; set; }
        public List<Tbl_Block_8_Q6>? IncomeBlock8Q6 { get; set; }
        public List<Tbl_Block_9a>? IncomeBlock9A { get; set; }
        public List<Tbl_Block_9b>? IncomeBlock9B { get; set; }
        public Tbl_Block_10? IncomeBlock10 { get; set; }
        public Tbl_Block_11a? IncomeBlock11A { get; set; }
        public List<Tbl_Block_11b>? IncomeBlock11B { get; set; }
        public Tbl_Block_A? IncomeBlockA { get; set; }
        public Tbl_Block_B? IncomeBlockB { get; set; }
        public List<Tbl_Warning>? IncomeWarningList { get; set; }
    }
}
