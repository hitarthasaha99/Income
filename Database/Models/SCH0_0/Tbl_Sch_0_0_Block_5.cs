using Income.Database.Models.Common;
using SQLite;
using System.Text.Json.Serialization;

namespace Income.Database.Models.SCH0_0
{
    public class Tbl_Sch_0_0_Block_5 : Tbl_Base
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public int SSS { get; set; }
        public int? HG_SB { get; set; }
        public bool? isInitialySelected { get; set; }

        //is household or not
        public bool is_household { get; set; } = true;
        //is selected for 
        public bool isSelected { get; set; } = false;
        //Travel selection
        public bool? is_selected_travel { get; set; } = false;
        //Travel
        public bool is_initially_selected_travel { get; set; } = false;
        //Travel
        public bool is_substituted_travel { get; set; } = false;

        // Tracks if this household was a casualty (could not participate)
        public bool is_casualty_travel { get; set; } = false;
        // Tracks the ID of the household this one is substituting, if any
        public int? substituted_for_id_travel { get; set; }
        // Track the original household ID that started this substitution chain
        public int? original_household_id_travel { get; set; }

        // Track how many times the original household has been substituted
        public int substitution_count_travel { get; set; }
        // use for to maintain status to check survey of the household are send to sso
        public string status_travel { get; set; }
        //household row number 1st row shows all household and not household

        public int Block_5A_1 { get; set; }
        //house number
        [MaxLength(50)]
        public string Block_5A_2 { get; set; }

        //srl.no. hhd id used everywhere to identify house hold, in schedule it shows as household serial number
        //only these considered for listing
        public int Block_5A_3 { get; set; }
        //name of head of the household
        [MaxLength(50)]
        public string Block_5A_4 { get; set; }
        //household (hh) size
        [MaxLength(50)]
        public int Block_5A_5 { get; set; }
        //Usual monthly consumption expenditure of the household (Rs)
        public int Block_5A_6 { get; set; }
        //UMPCE (Block_5A_6 / Block_5A_5)
        public double Block_5A_7 { get; set; } = 0.0;
        //UMPCE code (UMPCE > A → 1, UMPCE ≤ A → 2)
        public int Block_5A_8 { get; set; }
        //Any household member completed overnight trip during last 365 days for medical/ holidaying/ shopping purpose? (yes-1, no-2)
        public int Block_5A_9 { get; set; }
        //For 2 in col. 9, any household member completed any other overnight trip during last 30 days? (yes-1, no-2)
        public int Block_5A_10 { get; set; }
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
        //population of all household size total
        public int population { get; set; }
        // Status 
        public int hhdStatus { get; set; }

        public string Timestamp { get; set; }
        // use for to maintain status to check survey of the household are send to sso
        public string status { get; set; }
        [JsonIgnore]
        public int needDownload { get; set; }
        public int? SSS_household_id { get; set; }
        public decimal a { get; set; }
    }
}
