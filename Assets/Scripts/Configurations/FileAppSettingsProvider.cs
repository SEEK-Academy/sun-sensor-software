using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Config;
using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Configurations
{
    internal class FileAppSettingsProvider : IAppSettingsProvider
    {
        public AppSettings Load()
        {
            string cli = GetCliConfigPath();
            if (!string.IsNullOrEmpty(cli) && File.Exists(cli))
                return ReadJsonOrThrowException(cli);

            string exeDir = Directory.GetParent(Application.dataPath)!.FullName;
            string besideExe = Path.Combine(exeDir, "appsettings.json");
            if (File.Exists(besideExe))
                return ReadJsonOrThrowException(besideExe);

            return AppSettings.GetDefault();
        }

        private static string GetCliConfigPath()
        {
            foreach (string a in Environment.GetCommandLineArgs())
            {
                if (a.StartsWith("--config=", StringComparison.OrdinalIgnoreCase))
                    return a["--config=".Length..].Trim('"');
                if (a.StartsWith("-config=", StringComparison.OrdinalIgnoreCase))
                    return a["-config=".Length..].Trim('"');
            }

            return string.Empty;
        }

        private static AppSettings ReadJsonOrThrowException(string path)
        {
            AppSettings settings;
            
            try
            {
                string json = File.ReadAllText(path);
                settings =  JsonUtility.FromJson<AppSettings>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to read configuration '{path}': {e.Message}");
                throw;
            }

            if (settings == null)
            {
                Debug.LogError($"Failed to read configuration '{path}'.");
                throw new InvalidOperationException($"Failed to read configuration '{path}'.");
            }

            return settings;
        }
    }
}
