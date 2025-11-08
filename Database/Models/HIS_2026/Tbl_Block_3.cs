using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Models.HIS_2026
{
    public class Tbl_Block_3
    {
        [PrimaryKey]
        public Guid id { get; set; }
        //s.no.
        public int? serial_no { get; set; }
        //name of member
        public string? item_2 { get; set; }
        //relation to head(code)
        public int? item_3 { get; set; }
        //gender(code)
        public int? gender { get; set; }
        //age(years)
        public int? age { get; set; }
        //marital status(code)
        public int? item_6 { get; set; }
        //highest educational level attained(code)
        public int? item_7 { get; set; }
        //during 30 days  primary(code)
        public int? item_8 { get; set; }
        //during 30 days  secondary(code)
        public int? item_9 { get; set; }
        //during 365 days primary(code)
        public int? item_10 { get; set; }
        //during 365 days  secondary(code)
        public int? item_11 { get; set; }
        //whether a beneficiary of any Central/ State Government social assistance scheme as on date of survey(yes-1/ no-2)
        public int? item_12 { get; set; }
    }
}
