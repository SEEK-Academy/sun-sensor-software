using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Config;
using Assets.Scripts.Sources.UsbSunSensor;
using System;

namespace Assets.Scripts.Sources
{
    public static class SourceFactory
    {
        public static ISunVectorRealtimeSource CreateSunVectorRealtimeSource(AppSettings settings)
        {
            return settings.Mode switch
            {
                AppMode.TestCentralSequence => new FakedCentralSequenceSunSensorSource(),
                _ => throw new NotImplementedException($"`{nameof(ISunVectorRealtimeSource)}` is not implemented for `{settings.Mode}`.")
            };
        }
    }
}
