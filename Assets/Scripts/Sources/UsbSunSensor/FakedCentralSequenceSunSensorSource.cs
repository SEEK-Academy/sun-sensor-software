using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Sources.UsbSunSensor
{
    internal class FakedCentralSequenceSunSensorSource : ISunVectorRealtimeSource
    {
        private readonly float _angularSpeedDegPerSec = 30f;
        private readonly float _sampleRateHz = 100f;
        private readonly float _pauseAtVertexSec = 1.5f;
        private readonly float _radius = 1f;
        private readonly Vector3[] _sequence = new[]
        {
            Vector3.forward,
            Vector3.right,
            Vector3.back,
            Vector3.left,
            Vector3.up,
            Vector3.down
        };

        private Coroutine _runner;
        private CoroutineHost _host;

        public event Action<Vector3> DataReceived;

        public bool IsActive { get; private set; }

        public FakedCentralSequenceSunSensorSource()
        { }

        public FakedCentralSequenceSunSensorSource(
            float angularSpeedDegPerSec,
            float sampleRateHz,
            float pauseAtVertexSec,
            float radius)
        {
            _angularSpeedDegPerSec = Mathf.Max(0.001f, angularSpeedDegPerSec);
            _sampleRateHz = Mathf.Clamp(sampleRateHz, 1f, 240f);
            _pauseAtVertexSec = Mathf.Max(0f, pauseAtVertexSec);
            _radius = Mathf.Max(0.0001f, radius);
        }

        public void Start()
        {
            if (IsActive)
                return;

            if (_host == null)
            {
                var go = new GameObject("[SunSensor Coroutine Host]")
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
                UnityEngine.Object.DontDestroyOnLoad(go);
                _host = go.AddComponent<CoroutineHost>();
            }

            _runner = _host.StartCoroutine(RunOrbit());
            IsActive = true;
        }

        public void Stop()
        {
            if (!IsActive)
                return;

            if (_host && _runner != null)
            {
                _host.StopCoroutine(_runner);
                _runner = null;
            }

            IsActive = false;
        }

        public void Dispose()
        {
            Stop();

            if (_host)
            {
                try
                {
                    UnityEngine.Object.Destroy(_host.gameObject);
                }
                catch { }

                _host = null;
            }
        }

        private IEnumerator RunOrbit()
        {
            var tick = 1f / _sampleRateHz;

            var current = _sequence[0] * _radius;
            DataReceived?.Invoke(current);

            var i = 0;
            while (true)
            {
                Vector3 from = _sequence[i % _sequence.Length];
                Vector3 to = _sequence[(i + 1) % _sequence.Length];

                // Kąt między wektorami i czas przejścia przy zadanej prędkości kątowej
                float angle = Vector3.Angle(from, to);
                float duration = Mathf.Max(0.0001f, angle / _angularSpeedDegPerSec);

                // Sferyczna interpolacja (SLERP) z równomierną prędkością kątową
                float t = 0f;
                while (t < 1f)
                {
                    // Czasu dyskretny o stałym kroku, żeby wyniki były bardziej powtarzalne
                    t = Mathf.Min(1f, t + tick / duration);
                    Vector3 dir = Vector3.Slerp(from, to, t).normalized;
                    current = dir * _radius;

                    DataReceived?.Invoke(current);

                    yield return new WaitForSeconds(tick);
                }

                // Opcjonalna pauza na centralnej sekwencji
                if (_pauseAtVertexSec > 0f)
                {
                    float elapsed = 0f;
                    Vector3 hold = to.normalized * _radius;
                    while (elapsed < _pauseAtVertexSec)
                    {
                        DataReceived?.Invoke(hold);
                        elapsed += tick;
                        yield return new WaitForSeconds(tick);
                    }
                }

                i++;
            }
        }

        private sealed class CoroutineHost : MonoBehaviour { }
    }
}
