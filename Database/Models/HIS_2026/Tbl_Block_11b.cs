using DocumentFormat.OpenXml.Bibliography;
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
    public class Tbl_Block_11b : Tbl_Base, IHISModel
    {
        public Guid fk_block_3 { get; set; }
        public int? hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        //S. no. of the member 
        public int? item_1 { get;set; }
        //Name of the member 
        public string? item_2 { get;set; }
        //Whether paid any direct tax during last financial year? (yes-1, no-2) 
        public int? item_3 { get;set; }
        //Amount of direct tax paid (net of refunds) in last financial year (in Rs.)
        public int? item_4 { get;set; }
        public bool? is_updated { get; set; }
    }
}
