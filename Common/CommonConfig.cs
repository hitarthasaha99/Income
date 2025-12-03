using Income.Database.Models.Common;
using Microsoft.Extensions.Options;
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
        private readonly AppConfig _appConfig;
        public CommonConfig(IOptions<AppConfig> config)
        {
            _appConfig = config.Value;
        }
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

        public string PostAddress => _appConfig.PostAddress;
        public string CommonAPIPostAddress => _appConfig.CommonAPIPostAddress;
        public string APIIncomeURL => _appConfig.APIIncomeURL;
        public string AppVersion => _appConfig.AppVersion;


        /// <summary>
        /// Endpoints
        /// </summary>
        public const string LOGIN_API = "v1/UtilityMaster/AuthenticateSurveyUserAsync";
        public const string FETCH_FSU_LIST_BY_USER_ID = "IncomeSurvey/GetFsuListByUserId?v_userid=";
        public const string FETCH_SAVED_RESPONSES_BY_FSU_ID = "IncomeSurvey/GetIncomeResponseDetailsByFsuIdAndStatus/";
        public const string SAVE_SUBMITTED_RESPONSE = "UpSertIncomeResponseAsync";
        public static string CHECK_CAPI_VERSION = $"v1/UtilityMaster/GetCapiApkVersionByUsername?survey_id={SessionStorage.surveyId}";
        public static string LOGOUT_API = "v1/UtilityMaster/LogoutUser?v_userName=";
        public static string BaseController = "IncomeSurvey";
        public static string UpdateListingAction = "RemoveIncomeCapiDetails";

    }
}
