using Assets.Scripts.Models.Config;
using System;

namespace Assets.Scripts.Configurations
{
    public static class ConfigHost
    {
        public static AppSettings AppSettings { get; private set; }

        public static void Initialize(AppSettings appSettings)
        {
            if (AppSettings != null)
                throw new InvalidOperationException("ConfigHost is already initialized.");

            AppSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }
    }
}
