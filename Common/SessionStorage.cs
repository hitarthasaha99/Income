using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Common
{
    public class SessionStorage
    {
        public static CommonFunction funcs { get; set; } = new();

        public static int SelectedFSUId = 0;
        public static Guid __user_id = new();
        public static string auth_token = string.Empty;
        public static int tenant_id = new();
        public static string user_role = "";
        public static int survey_type = 1116;
        public static string survey_code = "";
        public static Guid surveyId = new();
        public static string full_name = "";
        public static int selected_hhd_id = new();
        public static string role_name = "";
        public static int selected_hhd_seq = new();
        public static string user_name = "";
        public static string location = "";
        public static DateTime survey_start_date = funcs.ConvertDate("1st February 2026");
        public static DateTime survey_end_date = funcs.ConvertDate("31st January 2027");
        public static int selected_hhd_size = new();
        public static DateTime survey_timestamp = new();
        public static int FSU_Sector = 0;
        public static bool selection_done = false;
        public static bool hamlet_selection_done = false;
        public static bool subdivision_selection_done = false;
        public static bool FSU_Submitted = false;
        public static bool HHD_Submitted = false;
        public static int sss = 0;
        public static string hhd_head = string.Empty;
        public static bool ShowGenerateButton = false;
        public static Environment env;
        public static void ClearSession()
        {
            SelectedFSUId = 0;
            __user_id = new();
            auth_token = string.Empty;
            tenant_id = new();
            user_role = "";
            survey_type = new();
            survey_code = "";
            surveyId = new();
            full_name = "";
            selected_hhd_id = new();
            role_name = "";
            selected_hhd_seq = new();
            user_name = "";
            location = "";
            selected_hhd_size = new();
            survey_timestamp = new();
            FSU_Sector = 0;
            selection_done = false;
            hamlet_selection_done = false;
            FSU_Submitted = false;
            sss = 0;
            hhd_head = string.Empty;
            HHD_Submitted = false;
            ShowGenerateButton = false;
        }

        public static void ClearFSUFlags()
        {
            SelectedFSUId = 0;
            FSU_Sector = 0;
            selected_hhd_id = 0;
            selected_hhd_seq = 0;
            selection_done = false;
            hamlet_selection_done = false;
            subdivision_selection_done = false;
            FSU_Submitted = false;
            sss = 0;
            hhd_head = string.Empty;
            HHD_Submitted = false;
        }

        public static UserRole GetUserRole()
        {
            if (user_role == CommonConstants.USER_CODE_JSO || user_role == CommonConstants.USER_CODE_JSO2)
            {
                return UserRole.JSO;
            }
            else if (user_role == CommonConstants.USER_CODE_SSO || user_role == CommonConstants.USER_CODE_SSO2)
            {
                return UserRole.SSO;
            }
            else
            {
                return UserRole.DS;
            }
        }
        public enum UserRole
        {
            JSO = 1,
            SSO = 2,
            DS = 3
        }

        public enum Environment
        {
            Development = 1,
            Staging = 2,
            Production = 3
        }
    }
}
