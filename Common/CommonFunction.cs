using Income.Database.Models.SCH0_0;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Common
{
    public class CommonFunction
    {
        public int? CalculateNoOfSUToBeFormed(bool is_special, int? population)
        {
            if (is_special)
            {
                // Logic for special case
                if (population < 750)
                    return 1;

                return ((population - 750) / 900) + 2;
            }
            else
            {
                // Logic for normal case
                if (population < 1500)
                    return 1;

                return ((population - 1500) / 900) + 2;
            }
        }

        public List<Tbl_Sch_0_0_Block_5> SSWOR(List<Tbl_Sch_0_0_Block_5> items, int numberOfSelections = 20)
        {
            // Check if the number of selections is valid
            if (numberOfSelections > items.Count)
            {
                throw new ArgumentException("Number of selections cannot be greater than the number of items.");
            }
            // Shuffle the list
            Shuffle(items);
            // Select the first 'numberOfSelections' items from the shuffled list
            return items.GetRange(0, numberOfSelections);
        }

        static void Shuffle(List<Tbl_Sch_0_0_Block_5> list)
        {
            Random random = new Random();
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                // Pick a random index from 0 to i
                int j = random.Next(0, i + 1);
                // Swap list[i] with the element at random index
                Tbl_Sch_0_0_Block_5 temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        public string? DecodeJWT(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    var claims = new
                    {
                        UserName = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value,
                        UserRole = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserRole")?.Value,
                        UserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value,
                        TenantId = jwtToken.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value,
                        SurveyType = jwtToken.Claims.FirstOrDefault(c => c.Type == "SurveyType")?.Value,
                        SurveyCode = jwtToken.Claims.FirstOrDefault(c => c.Type == "SurveyCode")?.Value,
                        SurveyId = jwtToken.Claims.FirstOrDefault(c => c.Type == "SurveyId")?.Value,
                        FullName = jwtToken.Claims.FirstOrDefault(c => c.Type == "Name")?.Value,
                    };
                    SessionStorage.__user_id = Guid.Parse(claims.UserId);
                    SessionStorage.full_name = claims.FullName;
                    SessionStorage.auth_token = token;
                    SessionStorage.tenant_id = int.Parse(claims.TenantId);
                    SessionStorage.user_role = claims.UserRole;
                    SessionStorage.survey_type = int.Parse(claims.SurveyType);
                    SessionStorage.survey_code = claims.SurveyCode;
                    SessionStorage.surveyId = Guid.Parse(claims.SurveyId);
                    SessionStorage.role_name = GetUserRoleName(claims.UserRole);
                    SessionStorage.user_name = claims.UserName;
                    JsonConvert.SerializeObject(claims);
                }
                return null;
            }
            catch (Exception ex)
            {
                // Handle exception
                return null;
            }
        }

        public string GetUserRoleName(string role)
        {
            switch (role)
            {
                case CommonConstants.USER_CODE_JSO:
                case CommonConstants.USER_CODE_JSO2:
                    return CommonConstants.ROLE_NAME_JSO;
                case CommonConstants.USER_CODE_SSO:
                case CommonConstants.USER_CODE_SSO2:
                    return CommonConstants.ROLE_NAME_SSO;
                case CommonConstants.USER_CODE_DS:
                    return CommonConstants.ROLE_NAME_DS;
                case CommonConstants.USER_CODE_DA_ADMIN:
                    return CommonConstants.ROLE_NAME_DA_ADMIN;
                default:
                    return CommonConstants.ROLE_NAME_CPG;
            }
        }

        public string GetLocalFolderPath()
        {
            var backingFile = FileSystem.AppDataDirectory;
            string mainDirPath = System.IO.Path.Combine(backingFile, CommonConstants.LocalDirname);

            if (!Directory.Exists(mainDirPath))
            {
                System.IO.Directory.CreateDirectory(mainDirPath);
            }
            return mainDirPath;
        }

        public string ReturnStatus(int? status_id = 0)
        {
            try
            {
                CommonList commonList = new();
                var data = commonList.LOOKUP_CONST_STATUS_LIST.FirstOrDefault(x => x.id == status_id);
                if (data != null)
                {
                    return data.title;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public bool checkHasInternet()
        {
            try
            {
                return Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<string> fetchGeoLocation()
        {
            try
            {
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                });

                if (location != null)
                {
                    double latitude = location.Latitude;
                    double longitude = location.Longitude;
                    return $"{latitude.ToString("0.000000")} , {longitude.ToString("0.000000")}";
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                return string.Empty;
            }
            catch (PermissionException pEx)
            {
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public DateTime ConvertDate(string date)
        {
            try
            {
                DateTime dateVal = DateTime.Now;
                // Remove ordinal suffixes (st, nd, rd, th)
                string cleaned = System.Text.RegularExpressions.Regex.Replace(date, @"\b(\d+)(st|nd|rd|th)\b", "$1");
                // Parse to DateTime
                dateVal = DateTime.ParseExact(cleaned, "d MMMM yyyy", System.Globalization.CultureInfo.InvariantCulture);
                return dateVal;
            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }
        }

        public bool IsUpdateAvailable(apk_version_model version_Model)
        {
            try
            {
                var verison = VersionTracking.CurrentVersion;
                var splitted_verison = verison.Split(".");
                int major_verion = int.TryParse(splitted_verison[0].ToString(), out int major) ? major : 0;
                int minor_verion = int.TryParse(splitted_verison[2].ToString(), out int minor) ? minor : 0;
                if (version_Model.apk_major_version > major_verion || version_Model.apk_minor_version > minor_verion)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void CloseApp()
        {
            try
            {
                Application.Current.Quit();
            }
            catch (Exception ex)
            {
                // Handle exception if needed
                return;
            }
        }

        public async Task WriteJsonToFileAsync(string json, string baseFileName = "TravelJSON")
        {
            try
            {
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
                // Get the app's local folder
                string timestamp = DateTime.Now.ToString("yyyy-MMM-dd_HH-mm-ss-fff");
                string filePath = Path.Combine(documentsPath, "TRAVEL_JSON", $"{baseFileName}_{timestamp}.json");

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
                Debug.WriteLine($"Error writing JSON to file: {ex.Message}");
            }
        }


        public async Task<string> Export()
        {
            try
            {
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

                string ExportPath = Path.Combine(documentsPath, "TRAVEL_EXPORT");

                // Create the folder if it doesn't exist
                if (!Directory.Exists(ExportPath))
                {
                    Directory.CreateDirectory(ExportPath);
                }


                string[] fileNames = { "TRAVEL.db3" };
                string sourceFile = FileSystem.AppDataDirectory;
                string sourcePath = string.Empty;
                string targetLogPath = string.Empty;
                string message = "";

                foreach (var fileName in fileNames)
                {

                    sourcePath = Path.Combine(sourceFile, fileName);
                    string timestamp = DateTime.Now.ToString("dd_MMM_yyyy_HH-mm-ss");
                    string fileNameWithDate = Path.GetFileNameWithoutExtension(fileName) + "_" + timestamp + Path.GetExtension(fileName);
                    targetLogPath = Path.Combine(ExportPath, fileNameWithDate);
                    if (!File.Exists(sourcePath))
                    {
                        //await Shell.Current.DisplayAlert("Alert", "File does not exist", "OK");
                        return CommonConstants.FAILURE_TEXT;
                    }
                    if (File.Exists(targetLogPath))
                    {
                        File.Delete(targetLogPath);
                    }
                    // Open the source file
                    File.Copy(sourcePath, targetLogPath);

                    message = $"{CommonConstants.SUCCESS_TEXT}: DB Exported At {targetLogPath}/{fileNameWithDate}";
                }
                return message;
            }
            catch (Exception ex)
            {
                return $"DB Export Failed : {ex.Message} : {CommonConstants.FAILURE_TEXT}";
                //await Shell.Current.DisplayAlert("Fail", "File has been export failed", "OK");
            }
        }

        public async Task<string> DownloadPrintedFile(string file_data, string file_name)
        {
            try
            {
                byte[] fileBytes = Convert.FromBase64String(file_data);
                string documentsPath = string.Empty;

#if ANDROID
                // Use app's private external documents folder — NO permission required
                var context = Android.App.Application.Context;
                var folder = context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
                documentsPath = Path.Combine(folder.AbsolutePath, "TRAVEL_PRINT");
#endif

#if WINDOWS
                    documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TRAVEL_PRINT");
#endif

                // Ensure directory exists
                if (!Directory.Exists(documentsPath))
                {
                    Directory.CreateDirectory(documentsPath);
                }

                // Save file
                string filePath = Path.Combine(documentsPath, file_name);
                await File.WriteAllBytesAsync(filePath, fileBytes);

                await Launcher.Default.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath),
                    Title = "Open downloaded file"
                });


                return CommonConstants.SUCCESS_TEXT;
            }
            catch (Exception ex)
            {
                return $"Facing error while download file : {ex}";
            }
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

        public string ReturnStatusColor(string status)
        {
            try
            {
                if (status.ToString().ToLower().Contains("submitted") || status.ToString().ToLower().Contains("accepted"))
                    return "success";
                else if (status.ToString().ToLower().Contains("return"))
                    return "danger";
                else if (status.ToString().ToLower() == "ongoing")
                    return "primary";
                else
                    return "secondary";
            }
            catch (Exception ex)
            {
                return "secondary";
            }
        }
    }
}
