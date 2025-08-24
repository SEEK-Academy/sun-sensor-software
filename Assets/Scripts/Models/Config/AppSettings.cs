using System;

namespace Assets.Scripts.Models.Config
{
    public record AppSettings
    {
        public AppMode Mode;
        public UsbSettings UsbSettings;

        internal static AppSettings GetDefault()
        {
            return new AppSettings
            {
                Mode = AppMode.Prod,
                UsbSettings = UsbSettings.GetDefault()
            };
        }
    }
}
