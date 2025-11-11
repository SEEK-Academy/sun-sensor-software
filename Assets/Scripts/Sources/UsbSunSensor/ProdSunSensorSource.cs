using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.Config;
using Google.Protobuf;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using Seek.SunSensor.V1;
using System;
using System.Collections;
using UnityEngine;
using UsbErrorCode = LibUsbDotNet.Main.ErrorCode;

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

            IsActive = true;
            _runner = _host.StartCoroutine(RunProcess());
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

        private IEnumerator RunProcess()
        {
            var deviceFinder = new UsbDeviceFinder(_usbSettings.VendorId, _usbSettings.ProductId);
            var endpointId = (ReadEndpointID)_usbSettings.Endpoint;
            var endpointType = ResolveEndpointType(_usbSettings.TransferType);
            var packetSize = Mathf.Max(1, _usbSettings.PacketSize);
            var retryDelaySeconds = Mathf.Max(0f, _usbSettings.ReadInterval / 1000f);
            var readBuffer = new byte[packetSize];

            var isInitialized = TryInitializeDevice(
                deviceFinder,
                endpointId,
                endpointType,
                packetSize,
                out UsbDevice device,
                out IUsbDevice claimedDevice,
                out UsbEndpointReader endpointReader);

            if (!isInitialized)
                yield break;

            try
            {
                while (IsActive)
                {
                    var errorCode = UsbErrorCode.None;
                    var bytesRead = 0;
                    var readError = false;
                    try
                    {
                        errorCode = endpointReader.Read(
                            readBuffer,
                            _usbSettings.ReadTimeout,
                            out bytesRead);
                    }
                    catch (Exception ex)
                    {
                        readError = true;
                        Debug.LogError($"Sun sensor USB read failed: {ex.Message}");
                    }

                    if (readError)
                    {
                        yield return CreateDelayYield(retryDelaySeconds);
                    }
                    else if (errorCode == UsbErrorCode.None && bytesRead > 0)
                    {
                        TryEmitPacket(readBuffer, bytesRead);
                    }
                    else
                    {
                        Debug.LogWarning($"USB device returned '{errorCode}'.");
                        yield return CreateDelayYield(retryDelaySeconds);
                    }
                }
            }
            finally
            {
                ResetUsbState(ref endpointReader, ref claimedDevice, ref device);
                UsbDevice.Exit();
            }
        }

        private bool TryInitializeDevice(
            UsbDeviceFinder finder,
            ReadEndpointID endpointId,
            EndpointType endpointType,
            int packetSize,
            out UsbDevice device,
            out IUsbDevice claimedDevice,
            out UsbEndpointReader reader)
        {
            device = null;
            claimedDevice = null;
            reader = null;

            try
            {
                device = UsbDevice.OpenUsbDevice(finder);

                if (device == null)
                {
                    Debug.LogWarning($"Sun sensor USB device not found " +
                        $"(VID=0x{_usbSettings.VendorId:X4}, " +
                        $"PID=0x{_usbSettings.ProductId:X4}).");
                    return false;
                }

                claimedDevice = device as IUsbDevice;
                if (claimedDevice != null)
                {
                    if (!claimedDevice.SetConfiguration(1))
                        Debug.LogWarning("Unable to set configuration 1 for USB device.");

                    if (!claimedDevice.ClaimInterface(_usbSettings.Interface))
                    {
                        Debug.LogWarning($"Unable to claim interface {_usbSettings.Interface} for USB device.");
                        ReleaseDevice(ref claimedDevice, ref device);
                        return false;
                    }
                }

                reader = device.OpenEndpointReader(
                    endpointId, packetSize, endpointType);
                reader.Flush();

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Initialization of Sun Sensor USB device failed: {ex.Message}");
                ReleaseDevice(ref claimedDevice, ref device);
                reader?.Dispose();
                return false;
            }
        }

        private void TryEmitPacket(byte[] buffer, int length)
        {
            try
            {
                var input = new CodedInputStream(buffer, 0, length);
                var data = SunSensorData.Parser.ParseFrom(input);
                DataReceived?.Invoke(data);
            }
            catch (InvalidProtocolBufferException ex)
            {
                Debug.LogWarning($"Invalid Sun Sensor payload ({length} bytes): {ex.Message}");
            }
        }

        private void ResetUsbState(ref UsbEndpointReader reader, ref IUsbDevice claimedDevice, ref UsbDevice device)
        {
            if (reader != null)
            {
                try
                {
                    reader.Abort();
                }
                catch { }
                reader.Dispose();
                reader = null;
            }

            ReleaseDevice(ref claimedDevice, ref device);
        }

        private void ReleaseDevice(ref IUsbDevice claimedDevice, ref UsbDevice device)
        {
            if (claimedDevice != null)
            {
                try 
                {
                    claimedDevice.ReleaseInterface(_usbSettings.Interface);
                }
                catch { }
                claimedDevice = null;
            }

            if (device != null)
            {
                try
                {
                    device.Close();
                }
                catch { }
                device = null;
            }
        }

        private static EndpointType ResolveEndpointType(string transferType)
        {
            if (string.IsNullOrWhiteSpace(transferType))
                return EndpointType.Bulk;
            if (Enum.TryParse(transferType, true, out EndpointType endpointType))
                return endpointType;

            return EndpointType.Bulk;
        }

        private static object CreateDelayYield(float seconds)
            => seconds > 0f ? (object)new WaitForSeconds(seconds) : null;

        private sealed class CoroutineHost : MonoBehaviour { }
    }
}
