using Income.Database.Models.Common;
using Income.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Common
{
    public class CommonConfig
    {
        public SessionStorage sessionStorage { get; set; } = new();
        /// <summary>
        /// Required Fields 
        /// </summary>
        public List<Validation_Model> LOGINREQUIREDFIELDS { get; set; } = new();
        public string LoginRequiredFileds()
        {
            LOGINREQUIREDFIELDS = new();
            LOGINREQUIREDFIELDS.Add(new Validation_Model
            {
                field_name = "username",
                field_type = CommonConstants.VALIDATION_TYPE_DEFAULT,
            });
            LOGINREQUIREDFIELDS.Add(new Validation_Model
            {
                field_name = "password",
                field_type = CommonConstants.VALIDATION_TYPE_DEFAULT,
            });
            return JsonConvert.SerializeObject(LOGINREQUIREDFIELDS);
        }
        public List<Validation_Model> SCH_0_0_BLOCK_1_REQ_FIELDS { get; set; } = new();

        public static string BaseController = "IncomeSurvey";

        public static string PostAddress => ConfigService.Current.BaseUrls.Post;
        public static string CommonAPIPostAddress => ConfigService.Current.BaseUrls.Common;
        public static string APIIncomeURL => ConfigService.Current.BaseUrls.Income;
        public static string APP_VERSION => ConfigService.Current.APP_Version;

        public static string LOGIN_API => ConfigService.Current.Routes.Login;
        public static string FETCH_FSU_LIST_BY_USER_ID => ConfigService.Current.Routes.FetchFsuList;
        public static string FETCH_SAVED_RESPONSES_BY_FSU_ID => ConfigService.Current.Routes.FetchSavedResponses;
        public static string SAVE_SUBMITTED_RESPONSE => ConfigService.Current.Routes.SaveResponse;
        public static string UpdateListingAction => ConfigService.Current.Routes.UpdateListing;
        public static string LOGOUT_API => ConfigService.Current.Routes.Logout;

    }
}
