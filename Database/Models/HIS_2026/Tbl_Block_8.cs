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
    public class Tbl_Block_8 : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public int? hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        //sl no
        //Q8.1
        public int? item_1 { get; set; }
        //Q8.2
        public int? item_2 { get; set; }
        //Q8.3
        public int? item_3 { get; set; }
        //Q8.4
        public int? item_4 { get; set; }
        //Q8.5
        public int? item_5 { get; set; }
        //Q8.6
        public int? item_6 { get; set; }
        public bool isUpdated { get; set; } = false;
    }
}
