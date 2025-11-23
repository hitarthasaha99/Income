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
    public class Tbl_Block_7c_Q10 : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public int hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        public int serial_number { get; set; }
        public Guid parent_id { get; set; }
        public int? item_10_1 { get; set; }
        public int? item_10_2 { get; set; }
        public int? item_10_3 { get; set; }
        public int? item_10_4 { get; set; }
        public int? item_10_5 { get; set; }
        public int? item_10_6 { get; set; }
        public int? item_10_7 { get; set; }
        public int? item_10_8 { get; set; }
        public int? item_10_9 { get; set; }
        public int? item_10_10 { get; set; }
        public int? item_10_11 { get; set; }
        public bool isUpdated { get; set; } = false;
    }
}
