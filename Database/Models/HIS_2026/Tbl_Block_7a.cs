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
    public class Tbl_Block_7a : Tbl_Base
    {
        public int hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        public int? item_2_1 { get; set; }
        public int? item_2_2 { get; set; }
        public int? item_2_3 { get; set; }
        public int? item_2_4 { get; set; }
        public int? item_2_5 { get; set; }
        public int? item_2_6 { get; set; }
        public int? item_2_7 { get; set; }
        public int? item_2_8 { get; set; }
        public int? item_2_9 { get; set; }
        public int? item_2_10 { get; set; }
        public int? item_2_11 { get; set; }
        public int? item_2_12 { get; set; }
        public int? item_2_13 { get; set; }
        public int? item_3 { get; set; }
        public int? item_4 { get; set; }
        public int? item_5 { get; set; }
        //7a.9
        public int? item_6 { get; set; }

    }
}
