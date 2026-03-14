using Assets.Scripts.Models.Config;
using System;

namespace Assets.Scripts.Configurations
{
    public static class ConfigHost
    {
        private static AppSettings _appSettings;

        public static AppSettings AppSettings
            => _appSettings ?? throw new Exception($"{nameof(ConfigHost)} is not initialized properly.");

        public static void Initialize(AppSettings appSettings)
        {
            if (_appSettings != null)
                throw new InvalidOperationException($"{nameof(ConfigHost)} is already initialized.");

            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }
    }
}
