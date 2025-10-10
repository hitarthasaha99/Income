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

        //Test
        public static readonly string PostAddress = "https://stagesurvey1.esigma.mospi.gov.in/APITourism/";
        public static readonly string CommonAPIPostAddress = "https://stagesurvey1.esigma.mospi.gov.in/APICommon/api/SURVEY/";
        public static readonly string APITOURISMURL = "https://stagesurvey1.esigma.mospi.gov.in/APITourism/api/SURVEY/v1/";

        //Staging
        //public static readonly string PostAddress = "http://115.124.119.108:83/ApiTourism/";
        //public static readonly string CommonAPIPostAddress = "http://115.124.119.108:83/ApiCommon/api/SURVEY/";
        //public static readonly string APITOURISMURL = "http://115.124.119.108:83/ApiTourism/api/SURVEY/v1/";

        //PROD
        //public static readonly string PostAddress = "https://esigma.mospi.gov.in/apITOURISM/";
        //public static readonly string CommonAPIPostAddress = "https://esigma.mospi.gov.in/apIcOMMON/";
        //public static readonly string APITOURISMURL = "https://esigma.mospi.gov.in/apITOURISM/api/SURVEY/v1/";


        /// <summary>
        /// Endpoints
        /// </summary>
        public const string LOGIN_API = "v1/UtilityMaster/AuthenticateSurveyUserAsync";
        public const string FETCH_FSU_LIST_BY_USER_ID = "v1/TravelSurvey/GetFsuListByUserId?v_userid=";
        public const string FETCH_SAVED_RESPONSES_BY_FSU_ID = "v1/TravelSurvey/GetTravelResponseDetailsByFsuIdAndStatus/";
        public const string SAVE_SUBMITTED_RESPONSE = "v1/TravelSurvey/UpSertTravelResponseAsync";
        public static string CHECK_CAPI_VERSION = $"v1/UtilityMaster/GetCapiApkVersionByUsername?survey_id={SessionStorage.surveyId}";
        public static string LOGOUT_API = "v1/UtilityMaster/LogoutUser?v_userName=";

    }
}
