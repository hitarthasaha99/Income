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
    public class Tbl_Block_7b : Tbl_Base, IHISModel
    {
        public int? hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        public int? item_6_1 { get; set; }
        public int? item_6_2 { get; set; }
        public int? item_6_3 { get; set; }
        public int? item_6_4 { get; set; }
        public int? item_6_5 { get; set; }
        public int? item_6_6 { get; set; }
        public int? item_6_7 { get; set; }
        public int? item_6_8 { get; set; }
        public int? item_6_9 { get; set; }
        public int? item_6_10 { get; set; }
        public int? item_6_11 { get; set; }
        public int? item_6_12 { get; set; }
        public int? item_7_1 { get; set; }
        public int? item_7_2 { get; set; }
        public int? item_7_3 { get; set; }
        public int? item_7_4 { get; set; }
        public int? item_7_5 { get; set; }
        public int? item_7_6 { get; set; }
        public int? item_7_7 { get; set; }
        public int? item_7_8 { get; set; }
        public int? item_7_9 { get; set; }
        public int? item_7_10 { get; set; }
        public int? item_7_11 { get; set; }
        public int? item_7_12 { get; set; }
        public int? item_7_13 { get; set; }
        public int? item_8 { get; set; }
        public int? item_9 { get; set; }
    }
}
