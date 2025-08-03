using Assets.Scripts.SunSensor.Interfaces;
using Seek.SunSensor.V1;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.SunSensor.Sources.UsbSunSensor
{
    internal class FakedUsbSunSensorSource : ISunSensorRealtimeSource
    {
        private CancellationTokenSource _cts;
        private Task _readTask;

        public event Action<SunSensorData> DataReceived;

        public bool IsActive { get; private set; }

        public void Start()
        {
            if (IsActive)
                return;
            
            IsActive = true;
            _cts = new CancellationTokenSource();
            _readTask = Task.Run(EmitLoop, _cts.Token);
        }

        public void Stop() 
        {
            if (!IsActive)
                return;

            IsActive = false;
            _cts?.Cancel();
            _readTask?.Wait();
        }

        public void Dispose() => Stop();

        private async Task EmitLoop()
        {
            while (!_cts.IsCancellationRequested)
            {
                var angle = Time.time;
                var data = new SunSensorData
                {
                    UnitVector = new Vector
                    {
                        X = Mathf.Cos(angle),
                        Y = Mathf.Sin(angle),
                        Z = 0
                    },
                    ErrorCode = ErrorCode.Ok
                };

                DataReceived?.Invoke(data);

                await Task.Delay(60, _cts.Token);
            }
        }
    }
}
