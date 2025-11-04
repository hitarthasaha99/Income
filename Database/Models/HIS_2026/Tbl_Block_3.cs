using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Models.HIS_2026
{
    public class Tbl_Block_3
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public int? serial_no { get; set; }
        public string? name_of_member { get; set; }
        public int? relation_to_head { get; set; }
        public int? gender { get; set; }
        public int? age { get; set; }
        public int? marital_status { get; set; }
        public int? highest_educational_level_attained { get; set; }
        public int? primary { get; set; }
        public int? secondary { get; set; }
        public int?
    }
}
