using System;

#if WINDOWS
using Windows.System.Profile;
using Windows.Storage.Streams;
#endif
#if ANDROID
using Android.Provider;
using Income.Components.Common;
using Microsoft.Maui.ApplicationModel;
using Income;
#endif

namespace Income.Components.Common
{
    public class DeviceInfoService
    {
        public string GetDeviceID()
        {
            string id = string.Empty;

#if ANDROID
            try
            {
                var context = Android.App.Application.Context?.ContentResolver;
                if (context != null)
                {
                    id = Settings.Secure.GetString(context, Settings.Secure.AndroidId);
                }
            }
            catch
            {
                id = string.Empty;
            }
#endif

#if WINDOWS
            try
            {
                var systemId = SystemIdentification.GetSystemIdForPublisher();
                if (systemId?.Id != null)
                {
                    byte[] bytes = new byte[systemId.Id.Length];
                    DataReader.FromBuffer(systemId.Id).ReadBytes(bytes);
                    id = BitConverter.ToString(bytes).Replace("-", "");
                }
            }
            catch
            {
                id = string.Empty;
            }
#endif

            return id;
        }
    }
}
