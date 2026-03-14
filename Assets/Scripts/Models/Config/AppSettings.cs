namespace Assets.Scripts.Models.Config
{
    public record AppSettings
    {
        public AppMode Mode;
        public UsbSettings UsbSettings;
        public SceneSettings SceneSettings;

        internal static AppSettings GetDefault()
        {
            return new AppSettings
            {
                Mode = AppMode.Prod,
                UsbSettings = UsbSettings.GetDefault(),
                SceneSettings = SceneSettings.GetDefault()
            };
        }
    }
}
