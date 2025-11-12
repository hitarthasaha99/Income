using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Income.Services
{
    public class LocalizationService
    {
        private readonly ResourceManager _resourceManager;

        public event Action? OnLanguageChanged;

        public LocalizationService()
        {
            try
            {
                _resourceManager = new ResourceManager("Income.Resources.Localization.AppResources", typeof(LocalizationService).Assembly);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public string this[string key] => _resourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;

        public void SetLanguage(string culture)
        {
            try
            {
                CultureInfo newCulture = new CultureInfo(culture);
                CultureInfo.DefaultThreadCurrentCulture = newCulture;
                CultureInfo.DefaultThreadCurrentUICulture = newCulture;
                OnLanguageChanged?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
    
}
