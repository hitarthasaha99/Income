using Income.Common;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Models.Common
{
    public class Tbl_Warning : Tbl_Base
    {
        public string? warning_message { get; set; }
        public int? warning_type { get; set; }
        public int? warning_status { get; set; }     // 1 => warning created ||  2 => warning resolved ||  3=> warning rejected  ||  4 => warning Accepted by sso || 5 => accepeted by DS
        public int? hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        public string? block { get; set; }
        public string? item_no { get; set; }
        public int? serial_number { get; set; }
        public Guid? parent_comment_id { get; set; }
        public string? remarks { get; set; }
        public DateTime? created_on { get; set; } = DateTime.Now;
        public DateTime? updated_at { get; set; }
        public string? role_code { get; set; }
        public string? user_name { get; set; }
        public string? schedule { get; set; }
        public string? block_guid { get; set; }
    }
}
