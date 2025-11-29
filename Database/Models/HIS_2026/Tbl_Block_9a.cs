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
    public class Tbl_Block_9a : Tbl_Base
    {
        public int hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        public int? item_1_1 { get; set; }
        public int? item_1_2 { get; set; }
        public int? item_1_3 { get; set; }
        public int? item_1_4 { get; set; }
        public int? item_1_5 { get; set; }
        public int? item_1_6 { get; set; }
        public int? item_2 { get; set; }
        public int? item_3 { get; set; }
        public int? item_4 { get; set; }
    }
}
