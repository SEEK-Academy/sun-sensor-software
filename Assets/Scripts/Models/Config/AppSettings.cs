using System;

namespace Assets.Scripts.Models.Config
{
    [Serializable]
    public record AppSettings
    {
        public string Mode;
        public UsbSettings UsbSettings;

        internal static AppSettings GetDefault()
        {
            return new AppSettings
            {
                Mode = "Prod",
                UsbSettings = UsbSettings.GetDefault()
            };
        }
    }
}
