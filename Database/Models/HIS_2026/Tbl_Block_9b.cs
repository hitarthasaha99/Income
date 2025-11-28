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
    public class Tbl_Block_9b : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public int hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        public int? item_5_1_3 { get; set; }
        public int? item_5_1_4 { get; set; }
        public int? item_5_1_5 { get; set; }
        public int? item_5_2_3 { get; set; }
        public int? item_5_2_4 { get; set; }
        public int? item_5_2_5 { get; set; }
        public int? item_5_3_3 { get; set; }
        public int? item_5_3_4 { get; set; }
        public int? item_5_3_5 { get; set; }
        public int? item_6 { get; set; }
        public int? item_7 { get; set; }
    }
}
