using Income.Common;
using SQLite;

namespace Income.Database.Models.Common
{
    public class Tbl_Comments : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public string? comment { get; set; }
        public int? comment_type { get; set; }
        public int? comment_status { get; set; } // 1 => comment created ||  2 => comment resolved ||  3=> warning rejected  ||  4 => warning Accepted
        public Guid? parent_comment_id { get; set; }
        public int? hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        public string? block { get; set; }
        public Guid? commented_by { get; set; }
        public DateTime? created_on { get; set; } = DateTime.Now;
        public DateTime? updated_at { get; set; }
        public string? user_name { get; set; }
        public string? role_code { get; set; }
        public string? item_no { get; set; }
        public int? serial_number { get; set; }
        public string? commenter_full_name { get; set; }
        public bool Is_accepted { get; set; } = false; // 1 accept and 2 reject
        public bool Is_rejected { get; set; } = false;
        public int? trip_serial_number { get; set; }
    }
}
