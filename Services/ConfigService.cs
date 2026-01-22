using Income.Common.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Services
{
    public enum AppEnvironment
    {
        QA,
        Staging,
        Production
    }
    public static class ConfigService
    {
        private static ApiConfig? _config;
        private static EnvironmentConfig? _currentEnv;

        // Default environment
        public static AppEnvironment EnvironmentName { get; set; } = AppEnvironment.QA;

        public static EnvironmentConfig Current =>
            _currentEnv ?? throw new InvalidOperationException("Config not loaded");

        public static async Task LoadAsync()
        {
            if (_config != null) return;

            using var stream = await FileSystem.OpenAppPackageFileAsync("api.config.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();

            _config = JsonConvert.DeserializeObject<ApiConfig>(json)
                      ?? throw new Exception("Invalid config file");

            if (!_config.Environments.TryGetValue(EnvironmentName, out _currentEnv))
                throw new Exception($"Environment '{EnvironmentName}' not found in config");
        }
    }

}
