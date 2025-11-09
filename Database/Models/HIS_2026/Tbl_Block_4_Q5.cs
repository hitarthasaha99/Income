using Income.Database.Models.Common;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Models.HIS_2026
{
    public class Tbl_Block_4_Q5 : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public int hhd_id { get; set; }

        // Foreign key to Tbl_Block_4
        public Guid Block4Id { get; set; }
        public int? SerialNumber { get; set; }

        // Activity type / description (dropdown or free text)
        public string? ActivityName { get; set; }

        // NIC Code (linked from NICCodeConstants)
        public int? NicCode { get; set; }

        // Business seasonal (1 = Yes, 2 = No)
        public int? BusinessSeasonal { get; set; }

        // No of months
        public int? NumberOfMonths { get; set; }
    }
}
