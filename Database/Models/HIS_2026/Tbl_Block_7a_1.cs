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
    public class Tbl_Block_7a_1 : Tbl_Base, IHISModel
    {
        public Guid Block_7a_Id { get; set; }
        public int? hhd_id { get; set; } = SessionStorage.selected_hhd_id;
        //serial number
        public int serial_number { get; set; }
        //crop code
        public int? code {  get; set; }
        public int? unit {  get; set; }
        public int? item_4 {  get; set; }
        public int? item_5 { get; set; }
        public int? item_6 { get; set; }
        public int? item_7 { get; set; }
        public int? item_8 { get; set; }
        public int? item_9 { get; set; }
    }
}
