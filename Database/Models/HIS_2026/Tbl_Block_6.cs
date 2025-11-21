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
    public class Tbl_Block_6 : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public int? hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        //s.no.
        public int? serial_no { get; set; }
        //Q6.1
        public int? item_1 { get; set; }
        //Q6.2
        public int? item_2 { get; set; }
        //Q6.3
        public int? item_3 { get; set; }
        //Q6.4
        public int? item_4 { get; set; }
    }
}
