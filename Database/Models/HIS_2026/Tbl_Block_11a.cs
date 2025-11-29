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
    public class Tbl_Block_11a : Tbl_Base, IHISModel
    {
        public int? hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        //Q11.1
        public int? item_1 { get; set; }
        //Q11.2
        public int? item_2 { get; set; }
        //Q11.3
        public int? item_3 { get; set; }
        //Q11.4
        public int? item_4 { get; set; }
        //Q11.5
        public int? item_5 { get; set; }
        //Q11.6
        public int? item_6 { get; set; }
    }
}
