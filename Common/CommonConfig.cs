using Income.Database.Models.Common;
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

        public static readonly string APP_VERSION = "1.0.26";

        //Test
        public static readonly string PostAddress = "https://stagesurvey1.esigma.mospi.gov.in/APIIncome/";
        public static readonly string CommonAPIPostAddress = "https://stagesurvey1.esigma.mospi.gov.in/APICommon/api/SURVEY/";
        public static readonly string APIIncomeURL = "https://stagesurvey1.esigma.mospi.gov.in/APIIncome/api/SURVEY/v1/";

        //Staging
        //public static readonly string PostAddress = "http://115.124.119.108:83/ApiIncome/";
        //public static readonly string CommonAPIPostAddress = "http://115.124.119.108:83/ApiCommon/api/SURVEY/";
        //public static readonly string APIIncomeURL = "http://115.124.119.108:83/ApiIncome/api/SURVEY/v1/";

        //PROD
        //public static readonly string PostAddress = "https://esigma.mospi.gov.in/apIIncome/";
        //public static readonly string CommonAPIPostAddress = "https://esigma.mospi.gov.in/apIcOMMON/";
        //public static readonly string APIIncomeURL = "https://esigma.mospi.gov.in/apIIncome/api/SURVEY/v1/";


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
