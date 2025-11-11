using Assets.Scripts.Interfaces;
using Seek.SunSensor.V1;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Sources.UsbSunSensor
{
    public class FakedUsbSunSensorSource : MonoBehaviour, ISunSensorRealtimeSource
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

            try
            {
                _readTask?.Wait();
            }
            catch (AggregateException ex)
            {
                ex.Handle(inner => inner is TaskCanceledException);
            }
        }

        public void Dispose() => Stop();

        private async Task EmitLoop()
        {
            var sw = Stopwatch.StartNew();

            while (!_cts.IsCancellationRequested)
            {
                float t = (float)sw.Elapsed.TotalSeconds;
                var x = Mathf.Sin(t) * 30f;
                var y = Mathf.Sin(t * 0.5f) * 45f;
                var z = Mathf.Sin(t * 0.8f) * 60f;

                var data = new SunSensorData
                {
                    UnitVector = new Vector
                    {
                        X = x,
                        Y = y,
                        Z = z
                    },
                    ErrorCode = ErrorCode.Ok
                };

                DataReceived?.Invoke(data);

                await Task.Delay(60, _cts.Token);
            }

            sw.Stop();
        }
    }
}
