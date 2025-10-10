using Income.Common;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Models.Common
{
    public class Tbl_User_Details
    {
        [PrimaryKey]
        public Guid id { get; set; }
        public string username { get; set; } = "";
        public string password { get; set; } = "";
        public int tenant_id { get; set; } = new();
        public string user_role { get; set; } = "";
        public string survey_code { get; set; } = "";
        public Guid surveyId { get; set; } = new();
        public string full_name { get; set; } = "";
        public int surveyType { get; set; } = int.TryParse(CommonConstants.SURVEY_TYPE_ID, out int result) ? result : 0;
        public string? user_token { get; set; }
        public bool is_Deleted { get; set; } = false;
        public string deviceInfo { get; set; } = string.Empty;
        public Guid user_id { get; set; }


    }
}
