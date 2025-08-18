using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Config;
using Assets.Scripts.Sources.UsbSunSensor;
using System;

namespace Assets.Scripts.Sources
{
    internal static class SourceFactory
    {
        internal static ISunVectorRealtimeSource CreateSunVectorRealtimeSource(AppSettings settings)
        {
            Enum.TryParse<AppMode>(settings.Mode, true, out var mode);
            return mode switch
            {
                AppMode.TestCentralSequence => new FakedCentralSequenceSunSensorSource(),
                _ => throw new NotImplementedException($"'{nameof(ISunVectorRealtimeSource)}' is not implemented for AppMode '{mode}'.")
            };
        }
    }
}
