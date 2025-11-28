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
    public class Tbl_Block_FieldOperation : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public int? hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        public string? enumerator_name { get; set; }
        public string? enumerator_code { get; set; }
        public string? supervisor_name { get; set; }
        public string? supervisor_code { get; set; }
        public DateTime? date_of_survey { get; set; }
        public DateTime? date_of_receipt { get; set; }
        public DateTime? date_of_scrutiny { get; set; }
        public DateTime? date_of_despatch { get; set; }
        public int? total_time { get; set; }
        public int? number_of_enumerators { get; set; }
        public bool? item_5_1_remarks { get; set; }
        public bool? item_5_2_remarks { get; set; }
        public Guid? fk_block_3 { get; set; }
        public int? informant_serial { get; set; }
        public string? informant_name { get; set; }
        public string? informant_mobile { get; set; }
        public int? informant_response_code { get; set; }
    }
}
