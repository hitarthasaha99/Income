using Income.Database.Models.Common;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Income.Database.Models.SCH0_0
{
    public class Tbl_Sch_0_0_Block_3 : Tbl_Base
    {
        public string? file_name { get; set; }
        public bool? is_sub_unit { get; set; } = false;
        public string? block_name { get; set; }
        public string? file_type { get; set; }
        [NotMapped]
        [JsonIgnore]
        public string? file_image_source { get; set; }
        public string? Name { get; set; }
        public Byte[]? file_byte { get; set; }
        public byte[]? Image_File { get; set; }
    }
}
