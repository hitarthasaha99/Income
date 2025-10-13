using Income.Database.Models.Common;
using SQLite;
using System.ComponentModel.DataAnnotations;
using MaxLengthAttribute = System.ComponentModel.DataAnnotations.MaxLengthAttribute;

namespace Income.Database.Models.SCH0_0
{
    public class Tbl_Sch_0_0_Block_0_1 : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }

        [MaxLength(50)]
        public string? Block_0_1 { get; set; }
        //district
        [MaxLength(50)]
        public string? Block_0_2 { get; set; }
        //sub-district/tehsil/town
        [MaxLength(50)]
        public string? Block_0_3 { get; set; }
        //village name
        [MaxLength(50)]
        public string? Block_0_4 { get; set; }
        //investigator unit no.
        [MaxLength(50)]
        public string? Block_0_5 { get; set; }
        //block no.
        [MaxLength(50)]
        public string? Block_0_6 { get; set; }
        //sample sub unit number
        public int? block_0_7 { get; set; }
        //serial number of sample FSU
        [MaxLength(50)]
        public string? Block_1_1 { get; set; }
        //round number
        public int? Block_1_2 { get; set; }
        //schedule number
        [MaxLength(50)]
        public string? Block_1_3 { get; set; }
        //sample (central-1, state-2)
        public int? Block_1_4 { get; set; }
        //sector (rural-1, urban-2)
        public int? Block_1_5 { get; set; }
        //NSS region
        [MaxLength(50)]
        public string? Block_1_6 { get; set; }
        //district
        [MaxLength(50)]
        public string? Block_1_7 { get; set; }
        //stratum
        [MaxLength(50)]
        public string? Block_1_8 { get; set; }
        //sub-stratum
        [MaxLength(50)]
        public string? Block_1_9 { get; set; }
        //sub-round
        public int? Block_1_10 { get; set; }
        //FOD sub-region
        [MaxLength(50)]
        public string? Block_1_11 { get; set; }
        //frame code
        public int? Block_1_12 { get; set; }

        //population of village or number of household of UFS block
        public int? Block_1_13 { get; set; }
        //FOD sub-region
        //[MaxLength(50)]
        //public string Block_1_12 { get; set; }
        //frame code
        //public int Block_1_13 { get; set; }
        //frame population / households
        //approx.present population
        [Required(ErrorMessage = "Approx. present population is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Population must be greater than zero")]
        public int? Block_1_14 { get; set; }

        //total number of hgs/sbs formed (D)
        public int? Block_1_15 { get; set; }

        ////approx population of SU
        //public int? Block_1_16 { get; set; }
        ////No of sub division of SU to be formed D1
        //public int? Block_1_17 { get; set; }
        //survey code
        [Required(ErrorMessage = "Survey code is required")]
        public int? Block_1_16 { get; set; }
        //reason for substitution of original sample (code) (for codes 4 – 7 in item 17)
        public int? Block_1_17 { get; set; }
        //remarks
        [MaxLength(2000)]
        public string? remarks { get; set; } = null;

    }
}
