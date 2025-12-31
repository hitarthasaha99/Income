using Income.Common;
using Income.Database.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Income.Services
{
    public class NetworkService
    {
        private readonly ILoggingService _loggingService;
        public NetworkService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }
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
                await _loggingService.LogError($"GET request failed: {ex.Message}");
                return null;
            }
        }

        public static string ZipString(string inputStr)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(inputStr);
            string text = "";
            long num = inputStr.Length;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    gZipStream.Write(bytes, 0, bytes.Length);
                }

                byte[] inArray = memoryStream.ToArray();
                text = Convert.ToBase64String(inArray);
                num = text.Length;
            }

            return text;
        }

        public async Task<HttpResponseMessage> PostAsync(string controller, string action, object parameter)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(CommonConfig.PostAddress);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionStorage.auth_token);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = TimeSpan.FromSeconds(120);

                    string apiURL = $"api/SURVEY/v1/{controller}/{action}";

                    // Serialize and zip
                    string json = JsonConvert.SerializeObject(parameter);
                    var zippedJson = ZipString(json); // assuming this returns byte[]
                    var zippedJsonBytes = await UnzipBytes(zippedJson);

                    // Create gzipped content
                    using (var content = new ByteArrayContent(zippedJsonBytes))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        content.Headers.ContentEncoding.Add("gzip");

                        var response = await client.PostAsync(apiURL, content);
                        if (response != null)
                        {
                            await WriteJsonToFileAsync(json, endpoint: action, code:response.StatusCode.ToString()); // still store uncompressed json locally

                        }

                        if (!response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                            {
                                //await AppShell.Current.DisplayAlert("Alert", "Session expired, please login again.", "OK");
                            }

                            var responseContent = await response.Content.ReadAsStringAsync();
                            await _loggingService.LogError($"Error {action}: {responseContent}");
                        }
                        

                        return response;
                    }
                }
            }
            catch (HttpRequestException hex)
            {
                await _loggingService.LogError($"POST request failed: {hex.Message}");
                //if (hex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                //{
                //    await AppShell.Current.DisplayAlert("Alert", "Session expired, please login again.", "OK");
                //}
                return null;
            }
            catch (Exception ex)
            {
                await _loggingService.LogError($"POST request failed: {ex.Message}");
                return null;
            }
        }

        public async Task<byte[]> UnzipBytes(string inputStr)
        {
            try
            {
                byte[] buffer = Convert.FromBase64String(inputStr);
                byte[] result;
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        result = memoryStream.ToArray();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                await _loggingService.LogError($"Error during UnzipBytes operation - {ex}");
            }

            return new List<byte>().ToArray();
        }

        private async Task RequestPermissionsAsync()
        {
            try
            {
                var readStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
                var writeStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();

                if (readStatus != PermissionStatus.Granted || writeStatus != PermissionStatus.Granted)
                {
                    //await Shell.Current.DisplayAlert("Error", "Cannot export without permission", "OK");
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private async Task WriteJsonToFileAsync(string json, string baseFileName = $"POST", string endpoint = "", string code ="")
        {
            try
            {
                baseFileName = $"{SessionStorage.SelectedFSUId}_{endpoint}_{code}";
                string documentsPath = string.Empty;
#if ANDROID
                await RequestPermissionsAsync();
#endif
#if ANDROID
                documentsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments)?.AbsolutePath ?? string.Empty;
#endif
#if WINDOWS
                documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#endif

                if (string.IsNullOrEmpty(documentsPath))
                {
                    documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                string ExportPath = Path.Combine(documentsPath, "Income_Logs");

                // Create the folder if it doesn't exist
                if (!Directory.Exists(ExportPath))
                {
                    Directory.CreateDirectory(ExportPath);
                }
                // Get the app's local folder
                string appDirectory = ExportPath;
                string timestamp = DateTime.Now.ToString("yyyy-MMM-dd_HH-mm-ss-fff");
                string filePath = Path.Combine(appDirectory, "SubmissionJSON", $"{baseFileName}_{timestamp}.json");

                // Extract directory path
                string directoryPath = Path.GetDirectoryName(filePath);

                // Check if the directory exists, and create it if it doesn't
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (var writer = new StreamWriter(filePath, false))
                {
                    await writer.WriteAsync(json);
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogInfo($"Error writing JSON to file: {ex.Message}");
            }
        }

        public async Task<Login_Response_Model>? LoginService(Tbl_User_Details _User_Details)
        {
            try
            {
                //Only for QA
                if(SessionStorage.env == SessionStorage.Environment.Development)
                {
                    SessionStorage.survey_type = 1116;
                    await LogoutService(_User_Details.username);
                }
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
                await _loggingService.LogError($"Login POST request failed: {ex.Message}");
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
                await _loggingService.LogError($"Logout POST request failed: {ex.Message}");
                return null;
            }
        }
    }
}
