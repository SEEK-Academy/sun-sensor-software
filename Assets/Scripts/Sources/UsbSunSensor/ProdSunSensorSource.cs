using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Config;
using Seek.SunSensor.V1;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Sources.UsbSunSensor
{
    internal class ProdSunSensorSource : ISunSensorRealtimeSource
    {
        private readonly UsbSettings _usbSettings;

        private Coroutine _runner;
        private CoroutineHost _host;

        public event Action<SunSensorData> DataReceived;

        public bool IsActive { get; private set; }

        public ProdSunSensorSource(UsbSettings usbSettings)
        {
            _usbSettings = usbSettings;
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
            yield break;
        }

        private sealed class CoroutineHost : MonoBehaviour { }
    }
}
