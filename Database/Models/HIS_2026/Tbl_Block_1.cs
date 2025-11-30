using Income.Common;
using Income.Database.Models.Common;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Models.HIS_2026
{
    public class Tbl_Block_1 : Tbl_Base, IHISModel
    {
        public int? hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        public int? sector { get; set; }
        public string? sector_name { get; set; }
        public string? state_name { get; set; }
        public string? sub_district_name { get; set; }
        public string? district_name { get; set; }
        public string? village_name { get; set; }
        public int? sample_type { get; set; }
        public string? sample_type_name { get; set; }
        public int? state_id { get; set; }
        public int? district_id { get; set; }
        public int? town_id { get; set; }
        public string? town_name { get; set; }
        public int? village_id { get; set; }
        public string? investigator_unit_no { get; set; }
        public string? sample_su_number { get; set; }
        public string? round_number { get; set; }
        public string? schedule_name { get; set; } = "HIS26";
        public string? schedule_number { get; set; }
        public string? serial_number_sample_fsu { get; set; }
        public int? sample_sub_division_number { get; set; }
        public int? sss_number { get; set; }
        public int? sample_hhd_number { get; set; }
        public int? survey_code { get; set; }
        public int? substitution_reason { get; set; }
        public string? substitution_reason_remark { get; set; }
        public string? block_1_remark { get; set; } = string.Empty;
        public string? block_3_remark { get; set; } = string.Empty;
        public string? block_4_remark { get; set; } = string.Empty;
        public string? block_5_remark { get; set; } = string.Empty;
        public string? block_6_remark { get; set; } = string.Empty;
        public string? block_7a_remark { get; set; } = string.Empty;
        public string? block_7b_remark { get; set; } = string.Empty;
        public string? block_7c_remark { get; set; } = string.Empty;
        public string? block_7d_remark { get; set; } = string.Empty;

        public string? block_8a_remark { get; set; } = string.Empty;
        public string? block_8b_remark { get; set; } = string.Empty;

        public string? block_9a_remark { get; set; } = string.Empty;
        public string? block_9b_remark { get; set; } = string.Empty;
        public string? block_10_remark { get; set; } = string.Empty;
        public string? block_11a_remark { get; set; } = string.Empty;
        public string? block_11b_remark { get; set; } = string.Empty;
        public string? block_12_remark { get; set; } = string.Empty;
        public string? block_13_remark { get; set; } = string.Empty;
    }
}
