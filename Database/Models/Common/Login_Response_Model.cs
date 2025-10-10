using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Models.Common
{
    public class Login_Response_Model
    {
        public string? username { get; set; }
        public string? password { get; set; }
        public string? userId { get; set; }
        public string? userType { get; set; }
        public int? surveyType { get; set; }
        public bool? is_valid { get; set; }
        public string? message { get; set; }
        public user_token? userToken { get; set; }
    }

    public class user_token
    {
        public refresh_token? refresh_Token { get; set; }
        public string? token { get; set; }
        public double? expiration { get; set; }
    }

    public class refresh_token
    {
        public string? token { get; set; }
        public double? expiration { get; set; }
    }
}
