using Income.Database.Models.Common;
using SQLite;
using System.ComponentModel.DataAnnotations;
using MaxLengthAttribute = System.ComponentModel.DataAnnotations.MaxLengthAttribute;

namespace Income.Database.Models.SCH0_0
{
    public class Tbl_Sch_0_0_Block_0_1 : Tbl_Base
    {

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
        public int? Block_0_7 { get; set; }
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
        //approx.present population
        public int? Block_1_14 { get; set; }

        //total number of hgs/sbs formed (D)
        public int? Block_1_15 { get; set; }

        //survey code
        public int? Block_1_16 { get; set; }
        //reason for substitution of original sample (code) (for codes 4 – 7 in item 17)
        public int? Block_1_17 { get; set; }
        [MaxLength(2000)]
        public string? remarks_block_1_17 { get; set; } = string.Empty;
        public string? state_code { get; set; } = string.Empty;
        //remarks
        [MaxLength(2000)]
        public string? remarks { get; set; } = null;
        [MaxLength(2000)]
        public string? remarks_block_2_1 { get; set; } = null;
        [MaxLength(2000)]
        public string? remarks_block_2_2 { get; set; } = null;
        [MaxLength(2000)]
        public string? remarks_block_3 { get; set; } = null;
        [MaxLength(2000)]
        public string? remarks_block_4 { get; set; } = null;
        [MaxLength(2000)]
        public string? remarks_block_5 { get; set; } = null;
        [MaxLength(2000)]
        public string? remarks_block_6 { get; set; } = null;
        [MaxLength(2000)]
        public string? remarks_block_7 { get; set; } = null;
        [MaxLength(2000)]
        public string? remarks_block_7_selection { get; set; } = null;
        [MaxLength(2000)]
        public string? remarks_block_11 { get; set; } = null;
        //Block 4 items
        public int? sample_su_number { get; set; }
        public int? approximate_population_su { get; set; }
        public int? number_of_sub_division_of_su_to_be_formed { get; set; }

        //Remarks across different blocks
        public string? EnumeratorRemarks { get; set; } = string.Empty;
        public string? SupervisorRemarks { get; set; } = string.Empty;
    }
}
