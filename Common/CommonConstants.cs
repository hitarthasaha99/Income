using Income.Database.Models.Common;
using Income.Database.Models.SCH0_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Common
{
    public class CommonConstants
    {
        public const string DatabaseFilename = "Income.db3";
        public const string LocalDirname = "Income";

        public const string DateTimeFormat = "dd MMM, yyyy HH:mm:ss";
        public const string DateTimeFormat2 = "dd_MMM_yyyy";
        public const string DateFormat = "dd/MM/yyyy";

        public const string VALIDATION_TYPE_DEFAULT = "DEFAULT";
        public const string VALIDATION_TYPE_SELECT = "SELECT";
        public const string VALIDATION_TYPE_REG_EXP = "REG_EXP";
        public const string VALIDATION_TYPE_REG_EXP_NOT_REQUIRED = "REG_EXP_NOT_REQUIRED";
        public const string VALIDATION_TYPE_MIN_MAX = "MIN_MAX";
        public const string VALIDATION_TYPE_MIN = "ONLY_MIN";
        public const string VALIDATION_TYPE_MAX = "ONLY_MAX";
        public const string METHOD_TYPE_GET = "GET";
        public const string METHOD_TYPE_POST = "POST";
        public const string LOOKUP_SURVEY_CODE_4 = "4";
        public const string LOOKUP_SURVEY_CODE_7 = "7";
        public const string LOOKUP_YES_NO_CODE = "LOOKUP_YES_NO";
        public const string LOOKUP_RELIGION_TYPE = "LOOKUP_RELIGION";
        public const string LOOKUP_CASTE_TYPE = "LOOKUP_CASTE";
        public const string LOOKUP_SURVEY_CODE_HDD_2 = "2";
        public const string LOOKUP_SURVEY_CODE_HDD_3 = "3";
        public const string LOOKUP_SURVEY_CODE_HDD_9 = "9";
        public const string LOOKUP_SURVEY_CODE_HDD_1 = "1";
        public const string LOOKUP_YES_CODE = "1";
        public const string REG_EXP_POSITIVE_NUMBER_NON_ZERO = "/^(?!0(\\.0+)?$)([1-9]\\d*(\\.\\d+)?|0\\.\\d*[1-9])$/";
        public const string REG_EXP_POSITIVE_NUMBER_ZERO = "/^(0|[1-9]\\d*)(\\.\\d+)?$/";
        public const string REG_EXP_POSITIVE_WHOLE_NUMBER_ZERO = "/^(0|[1-9]\\d*)?$/";
        public const string REG_EXP_POSITIVE_WHOLE_NUMBER_NON_ZERO = "/^([1-9]\\d*)?$/";
        public const long MAX_FILE_SIZE = 5 * 1024 * 1024; //5MB
        public const string REG_EXP_PERSON_NAME = "/^[A-Za-z]{1,}(?:\\s[A-Za-z]{1,})*(?:[-'][A-Za-z]{1,})*\\s*$/";
        public const string REG_EXP_20_CHARS_REMARKS = "/^(?=.*\\S).{20,}$/";
        public const string DASH_VALUE = "--";
        public const string REG_EXP_6_DIGIT_PINCODE = "/^(9|[1-9][0-9]{5})$/";
        public const string SURVEY_TYPE_ID = "1116";
        public const string REG_EXP_AGE = "/^(100|[1-9][0-9]?)$/";
        public const string REG_EXP_INDIAN_MOBILE_NUMBER = "/^(?:\\+91|91|0)?[6-9]\\d{9}|^999$/";

        /// <summary>
        /// ROLE CODES
        /// </summary>
        public const string USER_CODE_JSO = "20200131ROL00011";
        public const string USER_CODE_JSO2 = "20200131ROL00014";
        public const string USER_CODE_SSO = "20200131ROL00010";
        public const string USER_CODE_SSO2 = "20200131ROL00015";
        public const string USER_CODE_DS = "20200131ROL00007";
        public const string USER_CODE_DA_ADMIN = "20200131ROL00008";
        public const string USER_CODE_CPG = "20200131ROL00006";
        public const string USRE_CODE_CPG1 = "20200131ROL00001";
        public const string USER_CODE_CPG2 = "20200131ROL00005";
        public const string USER_CODE_CPG3 = "20200131ROL00012";
        public const string USER_CODE_CPG5 = "20200131ROL00004";

        public const string ROLE_NAME_JSO = "JSO";
        public const string ROLE_NAME_SSO = "SSO";
        public const string ROLE_NAME_DS = "DS";
        public const string ROLE_NAME_DA_ADMIN = "DA ADMIN";
        public const string ROLE_NAME_CPG = "CPG";
        public const int LOOKUP_weekly_status_code_student = 4;
        public const int LOOKUP_weekly_status_code_other = 9;
        public const int LOOKUP_yes_code = 1;
        public const int LOOKUP_weekly_status_code_self_emp = 1;
        public const int LOOKUP_weekly_status_code_salary_reg_emp = 2;
        public const int LOOKUP_weekly_status_code_labour = 3;
        public const string SUCCESS_TEXT = "SUCCESS";
        public const string FAILURE_TEXT = "FAILURE";

        /// <summary>
        /// //////// LOOKUP
        /// </summary>
        public const string LOOKUP_SURVEY_CODE = "LOOKUP_SURVEY_CODE";
        public const string LOOKUP_SUBSTITUTION_REASON = "LOOKUP_SUBSTITUTION_REASON";
        public const string LOOKUP_SUBSTITUTION_REASON_HHD = "LOOKUP_SUBSTITUTION_REASON_HHD";
        public const string LOOKUP_SURVEY_CODE_HHD = "LOOKUP_SURVEY_CODE_HHD";
        public const string LOOKUP_YES_NO = "LOOKUP_YES_NO";
        public const string LOOKUP_GENDER = "LOOKUP_GENDER";
        public const string LOOKUP_MARITAL_STATUS = "LOOKUP_MARITAL_STATUS";
        public const string LOOKUP_STATE_LIST = "LOOKUP_STATE_LIST";
        public const string LOOKUP_EDUCATION_LEVEL = "LOOKUP_EDUCATION_LEVEL";
        public const string LOOKUP_WEEKLY_STATUS = "LOOKUP_WEEKLY_STATUS";
        public const string LOOKUP_CASTE = "LOOKUP_CASTE";
        public const string LOOKUP_RELIGION = "LOOKUP_RELIGION";
        public const string LOOKUP_HOUSEHOLD_TYPE_URBAN = "LOOKUP_HOUSEHOLD_TYPE_URBAN";
        public const string LOOKUP_HOUSEHOLD_TYPE_RURAL = "LOOKUP_HOUSEHOLD_TYPE_RURAL";
        public const string LOOKUP_TRANSPORT_PASS = "LOOKUP_TRANSPORT_PASS";
        public const string LOOKUP_MODE_OF_TRANSPORT = "LOOKUP_MODE_OF_TRANSPORT";
        public const string LOOKUP_TRIP_PURPOSE = "LOOKUP_TRIP_PURPOSE";
        public const string LOOKUP_frequently_member_visited = "LOOKUP_frequently_member_visited";
        public const string LOOKUP_CAPI_SUBMIT_STATUS = "LOOKUP_CAPI_SUBMIT_STATUS";
        public const string LOOKUP_TRAVEL_CLASS = "LOOKUP_TRAVEL_CLASS";
        public const string LOOKUP_REFERENCE_PERIOD = "LOOKUP_REFERENCE_PERIOD";
        public const int LOOKUP_CAPI_SUBMIT_STATUS_REF_BACK = 12;
        public const string HHD_STATUS_CASUALTY = "CASUALTY";
    }

    enum CommonEnum
    {
        fsu_list_page,
        sch_0_0_block_1,
        sch_0_0_block_2,
        sch_0_0_block_3,
        sch_0_0_block_3_1,
        sch_0_0_block_2_1,
        sch_0_0_block_2_2,
        sch_0_0_block_4,
        sch_0_0_block_4_3,
        sch_0_0_block_5,
        sch_0_0_block_6,
        sch_0_0_block_7,
        sch_0_0_block_7_selection,
        sch_0_0_block_8,
        sch_0_0_block_9,
        sch_0_0_block_11,
        sch_0_0_block_11b,
        ssu_list_page,
        his_block_1,
        his_block_3,
        his_block_4,
        his_block_5,
        his_block_6
    }

    public class CommonList
    {
        public List<Tbl_Lookup> LOOKUP_CONST_YES_NO = new List<Tbl_Lookup>
        {
            new Tbl_Lookup { id = 1, lookup_type = CommonConstants.LOOKUP_YES_NO, title = "Yes - 01"},
            new Tbl_Lookup { id = 2, lookup_type = CommonConstants.LOOKUP_YES_NO, title = "No - 02"},
        };

        public List<Tbl_Lookup> LOOKUP_CONST_GENDER = new List<Tbl_Lookup>
        {
            new Tbl_Lookup { id = 1, lookup_type = CommonConstants.LOOKUP_GENDER, title = "Male"},
            new Tbl_Lookup { id = 2, lookup_type = CommonConstants.LOOKUP_GENDER, title = "Female"},
            new Tbl_Lookup { id = 3, lookup_type = CommonConstants.LOOKUP_GENDER, title = "Transgender"},
        };

        public List<Tbl_Lookup> LOOKUP_CONST_MARITAL_STATUS = new List<Tbl_Lookup>
        {
            new Tbl_Lookup { id = 1, lookup_type = CommonConstants.LOOKUP_MARITAL_STATUS, title = "Never Married"},
            new Tbl_Lookup { id = 2, lookup_type = CommonConstants.LOOKUP_MARITAL_STATUS, title = "Currently Married"},
            new Tbl_Lookup { id = 3, lookup_type = CommonConstants.LOOKUP_MARITAL_STATUS, title = "Widowed"},
            new Tbl_Lookup { id = 4, lookup_type = CommonConstants.LOOKUP_MARITAL_STATUS, title = "Divorced/Separated"},
        };

        public List<Tbl_Lookup> LOOKUP_CONST_WEEKLY_STATUS = new List<Tbl_Lookup>
        {
            new Tbl_Lookup { id = 1, lookup_type = CommonConstants.LOOKUP_WEEKLY_STATUS, title = "Self-employed - 01"},
            new Tbl_Lookup { id = 2, lookup_type = CommonConstants.LOOKUP_WEEKLY_STATUS, title = "Regular wage/salaried employee - 02"},
            new Tbl_Lookup { id = 3, lookup_type = CommonConstants.LOOKUP_WEEKLY_STATUS, title = "casual labour - 03"},
            new Tbl_Lookup { id = 4, lookup_type = CommonConstants.LOOKUP_WEEKLY_STATUS, title = "Student - 04"},
            new Tbl_Lookup { id = 9, lookup_type = CommonConstants.LOOKUP_WEEKLY_STATUS, title = "Others - 09"},
        };

        public List<Tbl_Lookup> LOOKUP_CONST_RELIGION = new List<Tbl_Lookup>
        {
            new Tbl_Lookup { id = 1, lookup_type = CommonConstants.LOOKUP_RELIGION, title = "01 - Hinduism"},
            new Tbl_Lookup { id = 2, lookup_type = CommonConstants.LOOKUP_RELIGION, title = "02 - Islam"},
            new Tbl_Lookup { id = 3, lookup_type = CommonConstants.LOOKUP_RELIGION, title = "03 - Christianity"},
            new Tbl_Lookup { id = 4, lookup_type = CommonConstants.LOOKUP_RELIGION, title = "04 - Sikhism"},
             new Tbl_Lookup { id = 5, lookup_type = CommonConstants.LOOKUP_RELIGION, title = "05 - Jainism"},
            new Tbl_Lookup { id = 6, lookup_type = CommonConstants.LOOKUP_RELIGION, title = "06 - Buddhism"},
            new Tbl_Lookup { id = 7, lookup_type = CommonConstants.LOOKUP_RELIGION, title = "07 - Zoroastrianism"},
            new Tbl_Lookup { id = 9, lookup_type = CommonConstants.LOOKUP_RELIGION, title = "09 - Others"},
            new Tbl_Lookup { id = 10, lookup_type = CommonConstants.LOOKUP_RELIGION, title = "10 - Not Reported"},

        };
        public List<Tbl_Lookup> LOOKUP_CONST_HOUSEHOLDTYPE_URBAN = new List<Tbl_Lookup>
        {
            new Tbl_Lookup { id = 1, lookup_type = CommonConstants.LOOKUP_HOUSEHOLD_TYPE_URBAN, title = "self-employed - 01"},
            new Tbl_Lookup { id = 2, lookup_type = CommonConstants.LOOKUP_HOUSEHOLD_TYPE_URBAN, title = "regular wage/salary earning - 02"},
            new Tbl_Lookup { id = 3, lookup_type = CommonConstants.LOOKUP_HOUSEHOLD_TYPE_URBAN, title = "casual labour - 03"},
            new Tbl_Lookup { id = 9, lookup_type = CommonConstants.LOOKUP_HOUSEHOLD_TYPE_URBAN, title = "Others - 09"},

        };
        public List<Tbl_Lookup> LOOKUP_CONST_HOUSEHOLDTYPE_RURAL = new List<Tbl_Lookup>
        {
            new Tbl_Lookup { id = 1, lookup_type = CommonConstants.LOOKUP_HOUSEHOLD_TYPE_RURAL, title = "self-employed in agriculture - 01"},
            new Tbl_Lookup { id = 2, lookup_type = CommonConstants.LOOKUP_HOUSEHOLD_TYPE_RURAL, title = "self-employed in non-agriculture - 02"},
            new Tbl_Lookup { id = 3, lookup_type = CommonConstants.LOOKUP_HOUSEHOLD_TYPE_RURAL, title = "regular wage/salary earning - 03"},
            new Tbl_Lookup { id = 4, lookup_type = CommonConstants.LOOKUP_HOUSEHOLD_TYPE_RURAL, title = "casual labour in agriculture - 04"},
            new Tbl_Lookup { id = 5, lookup_type = CommonConstants.LOOKUP_HOUSEHOLD_TYPE_RURAL, title = "casual labour in non-agriculture - 05"},
            new Tbl_Lookup { id = 9, lookup_type = CommonConstants.LOOKUP_HOUSEHOLD_TYPE_RURAL, title = "Others - 09"},
        };
        public List<Tbl_Lookup> LOOKUP_CONST_CASTE = new List<Tbl_Lookup>
        {
            new Tbl_Lookup { id = 1, lookup_type = CommonConstants.LOOKUP_CASTE, title = "01 - Scheduled Tribe (ST)"},
            new Tbl_Lookup { id = 2, lookup_type = CommonConstants.LOOKUP_CASTE, title = "02 - Scheduled Caste (SC)"},
            new Tbl_Lookup { id = 3, lookup_type = CommonConstants.LOOKUP_CASTE, title = "03 - Other Backward Class (OBC)"},
            new Tbl_Lookup { id = 9, lookup_type = CommonConstants.LOOKUP_CASTE, title = "09 - Others"},
            new Tbl_Lookup { id = 10, lookup_type = CommonConstants.LOOKUP_CASTE, title = "10 - Not Reported"},

        };

        public List<Tbl_Lookup> LOOKUP_CONST_SURVEY_CODE = new List<Tbl_Lookup>
        {
            new Tbl_Lookup { id = 1, lookup_type = CommonConstants.LOOKUP_SURVEY_CODE, title = "01- selected FSU surveyed: inhabited"},
            new Tbl_Lookup { id = 2, lookup_type = CommonConstants.LOOKUP_SURVEY_CODE, title = "02- selected FSU surveyed: uninhabited"},
            new Tbl_Lookup { id = 3, lookup_type = CommonConstants.LOOKUP_SURVEY_CODE, title = "03- selected FSU surveyed: zero case"},
            new Tbl_Lookup { id = 4, lookup_type = CommonConstants.LOOKUP_SURVEY_CODE, title = "04- originally selected FSU not surveyed but substitute FSU surveyed: inhabited"},
            new Tbl_Lookup { id = 5, lookup_type = CommonConstants.LOOKUP_SURVEY_CODE, title = "05- originally selected FSU not surveyed but substitute FSU surveyed: uninhabited"},
            new Tbl_Lookup { id = 6, lookup_type = CommonConstants.LOOKUP_SURVEY_CODE, title = "06- originally selected FSU not surveyed but substitute FSU surveyed: Zero case"},
            new Tbl_Lookup { id = 7, lookup_type = CommonConstants.LOOKUP_SURVEY_CODE, title = "07- selected FSU casualty and no substitute surveyed"},
        };

        public List<Tbl_Lookup> LOOKUP_CONST_SUBSTITUTION_REASON = new List<Tbl_Lookup>
        {
            new Tbl_Lookup { id = 1, lookup_type = CommonConstants.LOOKUP_SUBSTITUTION_REASON, title = "01 - sample FSU: not identifiable/traceable"},
            new Tbl_Lookup { id = 2, lookup_type = CommonConstants.LOOKUP_SUBSTITUTION_REASON, title = "02 - sample FSU: not accessible"},
            new Tbl_Lookup { id = 3, lookup_type = CommonConstants.LOOKUP_SUBSTITUTION_REASON, title = "03 - sample FSU: restricted area (not permitted to survey)"},
            new Tbl_Lookup { id = 9, lookup_type = CommonConstants.LOOKUP_SUBSTITUTION_REASON, title = "09 - sample FSU: others (specify)"},
        };

        

        
        public bool IsSubstitutionAllowed { get; set; } = false;
        public List<Tbl_Lookup> LOOKUP_CONST_HHD_SURVEY_CODE
        {
            get
            {
                return new List<Tbl_Lookup>
                {
                    !IsSubstitutionAllowed ? new Tbl_Lookup { id = 1, lookup_type = CommonConstants.LOOKUP_SURVEY_CODE_HHD, title = "Original - 01"} : null,
                    IsSubstitutionAllowed ? new Tbl_Lookup { id = 2, lookup_type = CommonConstants.LOOKUP_SURVEY_CODE_HHD, title = "Substitute - 02"} : null,
                    new Tbl_Lookup { id = 3, lookup_type = CommonConstants.LOOKUP_SURVEY_CODE_HHD, title = "Casualty - 03"},
                    new Tbl_Lookup { id = 9, lookup_type = CommonConstants.LOOKUP_SURVEY_CODE_HHD, title = "get a substitution - 09"},
                }.Where(x => x != null).ToList();
            }
        }

        public List<Tbl_Lookup> LOOKUP_CONST_HHD_SUBSTITUTION_REASON = new List<Tbl_Lookup>
        {
            new Tbl_Lookup { id = 1, lookup_type = CommonConstants.LOOKUP_SUBSTITUTION_REASON_HHD, title = "Informant busy - 01"},
            new Tbl_Lookup { id = 2, lookup_type = CommonConstants.LOOKUP_SUBSTITUTION_REASON_HHD, title = "Members away from home - 02"},
            new Tbl_Lookup { id = 3, lookup_type = CommonConstants.LOOKUP_SUBSTITUTION_REASON_HHD, title = "Informant non- cooperative - 03"},
            new Tbl_Lookup { id = 9, lookup_type = CommonConstants.LOOKUP_SUBSTITUTION_REASON_HHD, title = "Others - 09"},
        };

        public List<Tbl_Lookup> LOOKUP_CONST_STATUS_LIST = new()
        {
            new Tbl_Lookup { id = 10, lookup_type = CommonConstants.LOOKUP_CAPI_SUBMIT_STATUS, title = "Ongoing"},
            new Tbl_Lookup { id = 11, lookup_type = CommonConstants.LOOKUP_CAPI_SUBMIT_STATUS, title = "ASSIGN TO ENUMERATOR"},
            new Tbl_Lookup { id = 12, lookup_type = CommonConstants.LOOKUP_CAPI_SUBMIT_STATUS, title = "RETURN BACK TO ENUMERATOR"},
            new Tbl_Lookup { id = 13, lookup_type = CommonConstants.LOOKUP_CAPI_SUBMIT_STATUS, title = "PARTIAL SUBMITTED BY ENUMERATOR"},
            new Tbl_Lookup { id = 21, lookup_type = CommonConstants.LOOKUP_CAPI_SUBMIT_STATUS, title = "SUBMITTED BY ENUMERATOR"},
            new Tbl_Lookup { id = 22, lookup_type = CommonConstants.LOOKUP_CAPI_SUBMIT_STATUS, title = "RETURN TO SUPERVISOR BY DS"},
            new Tbl_Lookup { id = 23, lookup_type = CommonConstants.LOOKUP_CAPI_SUBMIT_STATUS, title = "PARTIAL RETURN BACK TO ENUMERATOR"},
            new Tbl_Lookup { id = 31, lookup_type = CommonConstants.LOOKUP_CAPI_SUBMIT_STATUS, title = "Submitted by Supervisor To Ds"},
            new Tbl_Lookup { id = 32, lookup_type = CommonConstants.LOOKUP_CAPI_SUBMIT_STATUS, title = "Accepted By DS /AT CPG (Process)"},
            new Tbl_Lookup { id = 33, lookup_type = CommonConstants.LOOKUP_CAPI_SUBMIT_STATUS, title = "Final Submit"},
            new Tbl_Lookup { id = 60, lookup_type = CommonConstants.LOOKUP_CAPI_SUBMIT_STATUS, title = "Substitute"},
            new Tbl_Lookup { id = 70, lookup_type = CommonConstants.LOOKUP_CAPI_SUBMIT_STATUS, title = "Casualty"},
            new Tbl_Lookup { id = 80, lookup_type = CommonConstants.LOOKUP_CAPI_SUBMIT_STATUS, title = "Submitted to Supervisor(Sch0.0)"},
            new Tbl_Lookup { id = 100, lookup_type = CommonConstants.LOOKUP_CAPI_SUBMIT_STATUS, title = "Substituted"},
        };

        public List<Tbl_Lookup> LOOKUP_CONST_FREQUENTLY_VISIT_CODE = new List<Tbl_Lookup>
        {
            new Tbl_Lookup { id = 11, lookup_type = CommonConstants.LOOKUP_frequently_member_visited, title = "very frequently – 01"},
            new Tbl_Lookup { id = 12, lookup_type = CommonConstants.LOOKUP_frequently_member_visited, title = " frequently – 02"},
            new Tbl_Lookup { id = 21, lookup_type = CommonConstants.LOOKUP_frequently_member_visited, title = "infrequently – 03"},
            new Tbl_Lookup { id = 31, lookup_type = CommonConstants.LOOKUP_frequently_member_visited, title = "rarely – 04"},
        };

        public List<Tbl_Lookup> LOOKUP_CONST_MONTHS = new()
        {
            new Tbl_Lookup { id = 1, lookup_type = "", title = "January"},
            new Tbl_Lookup { id = 2, lookup_type = "", title = "February"},
            new Tbl_Lookup { id = 3, lookup_type = "", title = "March"},
            new Tbl_Lookup { id = 4, lookup_type = "", title = "April"},
            new Tbl_Lookup { id = 5, lookup_type = "", title = "May"},
            new Tbl_Lookup { id = 6, lookup_type = "", title = "June"},
            new Tbl_Lookup { id = 7, lookup_type = "", title = "July"},
            new Tbl_Lookup { id = 8, lookup_type = "", title = "August"},
            new Tbl_Lookup { id = 9, lookup_type = "", title = "September"},
            new Tbl_Lookup { id = 10, lookup_type = "", title = "October"},
            new Tbl_Lookup { id = 11, lookup_type = "", title = "November"},
            new Tbl_Lookup { id = 12, lookup_type = "", title = "December"},
        };

        public List<Tbl_Lookup> LOOKUP_RESPONCE_CODE_LIST = new List<Tbl_Lookup>
        {
            new Tbl_Lookup { id = 1, lookup_type = CommonConstants.LOOKUP_TRAVEL_CLASS, title = "Informant co-operative and capable - 01" },
            new Tbl_Lookup { id = 2, lookup_type = CommonConstants.LOOKUP_TRAVEL_CLASS, title = "Informant co-operative but not capable - 02" },
            new Tbl_Lookup { id = 3, lookup_type = CommonConstants.LOOKUP_TRAVEL_CLASS, title = "Informant busy - 03" },
            new Tbl_Lookup { id = 4, lookup_type = CommonConstants.LOOKUP_TRAVEL_CLASS, title = "Informant reluctant - 04" },
            new Tbl_Lookup { id = 9, lookup_type = CommonConstants.LOOKUP_TRAVEL_CLASS, title = "Others - 09" }
        };

        public List<Tbl_Lookup> LOOKUP_REFRENCE_PERIOD_LIST = new List<Tbl_Lookup>
        {
            new Tbl_Lookup { id = 1, lookup_type = CommonConstants.LOOKUP_REFERENCE_PERIOD, title = "First half of last 365 days from the date of survey - 01" },
            new Tbl_Lookup { id = 2, lookup_type = CommonConstants.LOOKUP_REFERENCE_PERIOD, title = "Second half of last 365 days from the date of survey - 02" },
            new Tbl_Lookup { id = 9, lookup_type = CommonConstants.LOOKUP_REFERENCE_PERIOD, title = "Last 365 days from the date of survey - 09" }
        };
        public static readonly Dictionary<string, string> UserCodeRoleMap = new Dictionary<string, string>
        {
        { "20200131ROL00011", "JSO" },
        { "20200131ROL00014", "JSO" },
        { "20200131ROL00010", "SSO" },
        { "20200131ROL00015", "SSO" },
        { "20200131ROL00007", "DS" },
        { "20200131ROL00008", "DA ADMIN" },
        { "20200131ROL00006", "CPG" },
        { "20200131ROL00001", "CPG" },
        { "20200131ROL00005", "CPG" },
        { "20200131ROL00012", "CPG" },
        { "20200131ROL00004", "CPG" }
        };

        public string GetRoleNameByUserCode(string userCode)
        {
            if (UserCodeRoleMap.TryGetValue(userCode, out string roleName))
            {
                return roleName;
            }

            return "Unknown Role";
        }

        public static int GetFilteredCount(List<Tbl_Sch_0_0_Block_7> list, int sss)
        {
            return list.Count(entry => entry.SSS == sss);
        }

        public static int GetFilteredCountOnInitialySelected(List<Tbl_Sch_0_0_Block_7> list, int sss, bool isInitialySelected = true)
        {
            return list.Count(entry => entry.SSS == sss && entry.isInitialySelected == isInitialySelected && entry.SubstitutionCount == 0 && entry.status != "CASUALTY");
        }
        public static int GetFilteredCountOnSelected(List<Tbl_Sch_0_0_Block_7> list, int sss, bool isInitialySelected = true)
        {
            return list.Count(entry => entry.SSS == sss && entry.isInitialySelected == isInitialySelected);
        }

        public static int GetFilteredSubstitution(List<Tbl_Sch_0_0_Block_7> list, int sss)
        {
            var counter = 0;
            var initialSelectedList = list.Where(entry => entry.SSS == sss && entry.isInitialySelected == true && entry.SubstitutionCount != 0).ToList();

            foreach (var item in initialSelectedList)
            {
                if (item.SubstitutionCount == 1)
                {
                    var filteredData = list.SingleOrDefault(entry => entry.isSelected == true && entry.OriginalHouseholdID == item.Block_7_3 && entry.SubstitutedForID == item.Block_7_3);
                    if (filteredData != null && filteredData.status == "SUBSTITUTE")
                    {
                        counter++;
                    }
                }
                else if (item.SubstitutionCount == 2)
                {
                    var filteredData = list.SingleOrDefault(entry => entry.isSelected == true && entry.OriginalHouseholdID == item.Block_7_3 && entry.SubstitutedForID != item.Block_7_3);
                    if (filteredData != null && filteredData.status == "SUBSTITUTE")
                    {
                        counter++;
                    }
                }
            }
            return counter;
        }

        public static int GetFilteredSubstitutionTravel(List<Tbl_Sch_0_0_Block_7> list)
        {
            var counter = 0;
            var initialSelectedList = list.Where(entry => entry.isInitialySelected == true && entry.SubstitutionCount != 0).ToList();

            foreach (var item in initialSelectedList)
            {
                if (item.SubstitutionCount == 1)
                {
                    var filteredData = list.SingleOrDefault(entry => entry.isSelected == true && entry.OriginalHouseholdID == item.Block_7_3 && entry.SubstitutedForID == item.Block_7_3);
                    if (filteredData != null && filteredData.status == "SUBSTITUTE")
                    {
                        counter++;
                    }
                }
                else if (item.SubstitutionCount == 2)
                {
                    var filteredData = list.SingleOrDefault(entry => entry.isSelected == true && entry.OriginalHouseholdID == item.Block_7_3 && entry.SubstitutedForID != item.Block_7_3);
                    if (filteredData != null && filteredData.status == "SUBSTITUTE")
                    {
                        counter++;
                    }
                }
            }
            return counter;
        }
    }
}
