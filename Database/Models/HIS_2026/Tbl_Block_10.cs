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
    public class Tbl_Block_10 : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public int? hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        //Q10.1 
        public int? item_1 { get; set; }
        //Q10.2
        public int? item_2 { get; set; }
        //Q10.3 
        public int? item_3 { get; set; }
        //Q10.4 
        public int? item_4 { get; set; }
        //Q10.5 
        public int? item_5 { get; set; }
        //Q10.6 
        public int? item_6 { get; set; }
        //Q10.7 
        public int? item_7 { get; set; }
        //Q10.8 
        public int? item_8 { get; set; }
        //Q10.9 
        public int? item_9 { get; set; }
        //Q10.10 
        public int? item_10 { get; set; }
        //Q10.11 
        public int? item_11 { get; set; }
        //Q10.12 
        public int? item_12 { get; set; }
        //Q10.13
        public int? item_13 { get; set; }
        //Q10.14 
        public int? item_14 { get; set; }
        //Q10.15 
        public int? item_15 { get; set; }
        //Q10.16 
        public int? item_16 { get; set; }
        //Q10.17 
        public int? item_17 { get; set; }
        //Q10.18 
        public int? item_18 { get; set; }
        //Q10.19 
        public int? item_19 { get; set; }
        //Q10.20 
        public int? item_20 { get; set; }
        //Q10.21 
        public int? item_21 { get; set; }
        //Q10.22 
        public int? item_22 { get; set; }
        //Q10.23 
        public int? item_23 { get; set; }
    }
}
