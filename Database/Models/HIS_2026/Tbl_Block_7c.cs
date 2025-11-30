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
    public class Tbl_Block_7c : Tbl_Base, IHISModel
    {
        public int? hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        public int? item_7_11 { get; set; }
        //7c.9
        public int? item_7_12 { get; set; }
    }
}
