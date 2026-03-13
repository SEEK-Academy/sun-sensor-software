using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Config;
using Assets.Scripts.Sources.UsbSunSensor;
using System;

namespace Assets.Scripts.Sources
{
    public static class SourceFactory
    {
        [Obsolete]
        public static ISunVectorRealtimeSource CreateSunVectorRealtimeSource(AppSettings settings)
        {
            return settings.Mode switch
            {
                AppMode.TestCentralSequence => new FakedCentralSequenceSunSensorSource(),
                AppMode.TestRandom => new FakedRandomUsbSunSensorSource(),
                _ => throw new NotImplementedException($"`{nameof(ISunVectorRealtimeSource)}` is not implemented for `{settings.Mode}`.")
            };
        }

        [Obsolete]
        public static ISunSensorRealtimeSource CreateSunSensorRealtimeSource(AppSettings settings)
        {
            return settings.Mode switch
            {
                AppMode.Prod => new ProdSunSensorSource(settings.UsbSettings),
                AppMode.TestCentralSequence => new FakedCentralSequenceSunSensorSource(),
                AppMode.TestRandom => new FakedRandomUsbSunSensorSource(),
                _ => throw new NotImplementedException($"`{nameof(ISunSensorRealtimeSource)}` is not implemented for `{settings.Mode}`.")
            };
        }

        public static ISunSensorFrameRealtimeSource CreateSunSensorFrameRealtimeSource(AppSettings settings)
        {
            return settings.Mode switch
            {
                AppMode.Prod => new ProdSunSensorFrameSource(settings.UsbSettings),
                AppMode.TestCentralSequence => new FakedCentralSequenceSunSensorSource(),
                AppMode.TestRandom => new FakedRandomUsbSunSensorSource(),
                _ => throw new NotImplementedException($"`{nameof(ISunSensorFrameRealtimeSource)}` is not implemented for `{settings.Mode}`.")
            };
        }
    }
}
