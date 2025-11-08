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
            _resourceManager = new ResourceManager("Income.Resources.Localization.AppResources", typeof(LocalizationService).Assembly);
        }

        public string this[string key] => _resourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;

        public void SetLanguage(string culture)
        {
            CultureInfo newCulture = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentUICulture = newCulture;
            OnLanguageChanged?.Invoke();
        }
    }
    public class LanguageState
    {
        public event Action? OnChange;
        public void NotifyLanguageChanged() => OnChange?.Invoke();
    }
}
