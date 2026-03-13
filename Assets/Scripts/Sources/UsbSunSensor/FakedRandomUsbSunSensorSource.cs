using Assets.Scripts.Interfaces;
using Seek.SunSensor.V1;
using Sunsensor;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Sources.UsbSunSensor
{
    public class FakedRandomUsbSunSensorSource : 
        MonoBehaviour, 
        ISunVectorRealtimeSource,
        ISunSensorRealtimeSource, 
        ISunSensorFrameRealtimeSource
    {
        private CancellationTokenSource _cts;
        private Task _readTask;
        private uint _frameSeq;

        public event Action<Vector3> VectorReceived;
        public event Action<SunSensorData> DataReceived;

        public event Action<Frame> FrameReceived;

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

                var vector = new Vector3(x, y, z);
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

                VectorReceived?.Invoke(vector);
                DataReceived?.Invoke(data);
                FrameReceived?.Invoke(BuildFrame(x, y, z));

                await Task.Delay(60, _cts.Token);
            }

            sw.Stop();
        }

        private Frame BuildFrame(float x, float y, float z)
        {
            return new Frame
            {
                Telemetry = new TelemetryFrame
                {
                    Sequence = _frameSeq++,
                    Telemetry = new Telemetry
                    {
                        SunVector = new SunVector
                        {
                            X = x,
                            Y = y,
                            Z = z,
                            Confidence = 1f,
                            TimestampMs = (uint)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                        },
                        Status = new Status
                        {
                            Code = StatusCode.Ok,
                            HealthBits = 0,
                            AdcErrorCode = 0,
                            HalStatus = 0
                        }
                    }
                }
            };
        }
    }
}
