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
    public class Tbl_Block_7d : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public int hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        public int? serial_number { get; set; }
        //sl no of activity
        public int? item_1 { get; set; }
        //description of activity
        public string? item_2 { get; set; }
        //mode of operation (1/2)
        public string? item_3 { get; set; }
        //% shareholding
        public decimal? item_4 { get; set; }
    }
}
