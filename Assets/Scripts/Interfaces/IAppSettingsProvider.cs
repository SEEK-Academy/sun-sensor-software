using Assets.Scripts.Models.Config;

namespace Assets.Scripts.Interfaces
{
    public interface IAppSettingsProvider
    {
        AppSettings Load();
    }
}
