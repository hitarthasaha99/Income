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
        public static int survey_type = new();
        public static string survey_code = "";
        public static Guid surveyId = new();
        public static string full_name = "";
        public static int selected_hhd_id = new();
        public static string role_name = "";
        public static int selected_hhd_seq = new();
        public static string user_name = "";
        public static string location = "";
        public static DateTime survey_start_date = funcs.ConvertDate("1st July 2025");
        public static DateTime survey_end_date = funcs.ConvertDate("30th June 2026");
        public static int selected_hhd_size = new();
        public static DateTime survey_timestamp = new();
        public static int FSU_Sector = 0;
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
        }
    }
}
