using Income.Database.Models.Common;
using SQLite;
using System.Text.Json.Serialization;

namespace Income.Database.Models.SCH0_0
{
    public class Tbl_Sch_0_0_Block_7 : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public int SSS { get; set; }
        public bool? isInitialySelected { get; set; }

        //is household or not
        public int? is_household { get; set; }
        //is selected for 
        public bool isSelected { get; set; } = false;
        //serial number
        public int? Block_5A_1 { get; set; }
        //house number
        [MaxLength(50)]
        public string Block_5A_2 { get; set; } = string.Empty;
        //srl.no. hhd id used everywhere to identify house hold, in schedule it shows as household serial number
        //only these considered for listing
        public int? Block_5A_3 { get; set; }
        //name of head of the household
        [MaxLength(50)]
        public string? Block_5A_4 { get; set; } = string.Empty;
        //household (hh) size
        [MaxLength(50)]
        public int? Block_5A_5 { get; set; }
        //highest education level attained among the household members
        public int? Block_5A_6 { get; set; }
        //household type
        public int? Block_5A_7 { get; set; }
        //total amount of land owned (in acres)
        public double? Block_5A_8 { get; set; }
        //usual monthly consumption expenditure of the household
        public int? Block_5A_9 { get; set; }
        // Indicates whether this household is a substitution for another household
        public bool isSubstitute { get; set; } = false;

        // Tracks if this household was a casualty (could not participate)
        public bool isCasualty { get; set; } = false;
        // Tracks the ID of the household this one is substituting, if any
        public int? SubstitutedForID { get; set; }
        // Track the original household ID that started this substitution chain
        public int? OriginalHouseholdID { get; set; }

        // Track how many times the original household has been substituted
        public int SubstitutionCount { get; set; }
        // Status 
        public int? hhdStatus { get; set; }
        // use for to maintain status to check survey of the household are send to sso
        public string status { get; set; } = string.Empty;
        [JsonIgnore]
        public int needDownload { get; set; }
        public int? SSS_household_id { get; set; }
        public decimal a { get; set; }
        public decimal b { get; set; }
    }
}
