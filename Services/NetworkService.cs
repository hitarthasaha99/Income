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
    public class NetworkService
    {
        public async Task<string?> GetAsync(string baseurl, string endpoint)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionStorage.auth_token);
                string network_uri = $"{baseurl}{endpoint}";
                var response = await client.GetStringAsync(network_uri);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public async Task<HttpResponseMessage> PostAsync(string controller, string action, object parameter)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(Constants.PostAddress);
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionDetail.Token);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            client.Timeout = TimeSpan.FromSeconds(300);

        //            string apiURL = $"api/SURVEY/v1/{controller}/{action}";

        //            // Serialize and zip
        //            string json = JsonConvert.SerializeObject(parameter);
        //            var zippedJson = ZipString(json); // assuming this returns byte[]
        //            var zippedJsonBytes = UnzipBytes(zippedJson);

        //            await WriteJsonToFileAsync(json); // still store uncompressed json locally

        //            // Create gzipped content
        //            using (var content = new ByteArrayContent(zippedJsonBytes))
        //            {
        //                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //                content.Headers.ContentEncoding.Add("gzip");

        //                var response = await client.PostAsync(apiURL, content);

        //                if (!response.IsSuccessStatusCode)
        //                {
        //                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //                    {
        //                        await AppShell.Current.DisplayAlert("Alert", "Session expired, please login again.", "OK");
        //                    }

        //                    var responseContent = await response.Content.ReadAsStringAsync();
        //                    CommonService.LogInfo($"Error {action}: {responseContent}");
        //                }

        //                return response;
        //            }
        //        }
        //    }
        //    catch (HttpRequestException hex)
        //    {
        //        CommonService.LogInfo($"Error {hex.Message}");
        //        if (hex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //        {
        //            await AppShell.Current.DisplayAlert("Alert", "Session expired, please login again.", "OK");
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonService.LogInfo($"Error {ex.Message}");
        //        return null;
        //    }
        //}

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
