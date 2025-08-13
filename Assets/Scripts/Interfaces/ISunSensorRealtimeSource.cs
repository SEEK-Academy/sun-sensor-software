﻿using Seek.SunSensor.V1;
using System;

namespace Assets.Scripts.Interfaces
{
    internal interface ISunSensorRealtimeSource : IDisposable
    {
        event Action<SunSensorData> DataReceived;
        bool IsActive { get; }
        void Start();
        void Stop();
    }
}
