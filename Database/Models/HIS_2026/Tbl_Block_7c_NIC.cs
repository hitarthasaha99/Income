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
    public class Tbl_Block_7c_NIC : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public int hhd_id { get; set; } = SessionStorage.selected_hhd_id;

        // Foreign key to Tbl_Block_7c
        public Guid Block7CId { get; set; }
        public int? SerialNumber { get; set; }

        // Activity type / description (dropdown or free text)
        public string? ActivityName { get; set; }

        // NIC Code (linked from NICCodeConstants)
        public int? NicCode { get; set; }

        // Gross value of receipts
        public int? GrossValue { get; set; }

    }
}
