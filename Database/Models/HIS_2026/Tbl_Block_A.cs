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
    public class Tbl_Block_A : Tbl_Base, IHISModel
    {
        public int? hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        public double? item_1 { get; set; }
        public double? item_2 { get; set; }
        public double? item_3 { get; set; }
        public double? item_4 { get; set; }
        public double? item_5 { get; set; }
        public double? item_6 { get; set; }
        public double? item_7 { get; set; }
        public double? item_8 { get; set; }
    }
}
