using Assets.Scripts.Configurations;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts
{
    public static class CompositionRoot
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            IAppSettingsProvider provider = new FileAppSettingsProvider();
            var appSettings = provider.Load();
            ConfigHost.Initialize(appSettings);
        }
    }
}
