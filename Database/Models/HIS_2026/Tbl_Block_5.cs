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
    public class Tbl_Block_5 :Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public int? hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        //sl no
        public int? item_1 { get; set; }
        //Q5.1
        public int? item_2 { get; set; }
        //Q5.2
        public int? item_3 { get; set; }
        //Q5.3(i)
        public int? item_4 { get; set; }
        //Q5.3(ii)
        public int? item_5 { get; set; }
        //Q5.4(i)
        public int? item_6 { get; set; }
        ////Q5.4(ii)
        public int? item_7 { get; set; }
        //Q5.5
        public int? item_8 { get; set; }
        //Q5.6
        public int? item_9 { get; set; }
        //Q5.7
        public int? item_10 { get; set; }
        //Q5.8
        public int? item_11 { get; set; }
        //Q5.9
        public int? item_12 { get; set; }
       
    }
}
