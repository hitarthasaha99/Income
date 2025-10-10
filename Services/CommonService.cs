using Income.Common;
using Income.Database.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Income.Services
{
    public class CommonService
    {
        public async Task<string?> GetAsync(string endpoint)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionStorage.auth_token);
                string network_uri = $"{CommonConfig.PostAddress}{endpoint}";
                var response = await client.GetStringAsync(network_uri);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Login_Response_Model>? LoginService(Tbl_User_Details _User_Details)
        {
            try
            {
                using (var _httpClient = new HttpClient())
                {
                    _httpClient.BaseAddress = new Uri(CommonConfig.CommonAPIPostAddress);
                    var request = new HttpRequestMessage(HttpMethod.Post, CommonConfig.LOGIN_API);
                    request.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(_User_Details), Encoding.UTF8, "application/json");
                    var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                    var response_content = response.Content.ReadAsStringAsync().Result;
                    if (response_content == null) return null;
                    else
                    {
                        var data = JsonConvert.DeserializeObject<Login_Response_Model?>(response_content);
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return null;
            }
        }
        public async Task<HttpResponseMessage> LogoutService(string UserName)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(CommonConfig.CommonAPIPostAddress);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionStorage.auth_token);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = TimeSpan.FromSeconds(100);
                    string apiURL = CommonConfig.LOGOUT_API + UserName + "&v_surveyType=" + SessionStorage.survey_type;
                    return await client.PostAsync(apiURL, new StringContent(string.Empty, Encoding.UTF8, "application/json"));
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
