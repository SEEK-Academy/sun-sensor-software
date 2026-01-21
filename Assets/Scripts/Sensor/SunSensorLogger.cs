using UnityEngine;
using Assets.Scripts.Sources.UsbSunSensor;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Sources;
using Assets.Scripts.Configurations;

public class SunSensorLogger : MonoBehaviour
{
    //public FakedUsbSunSensorSource sunSensor;
    public ISunSensorRealtimeSource sunSensor;

    void Start()
    {
        sunSensor = SourceFactory.CreateSunSensorRealtimeSource(ConfigHost.AppSettings);
    }

    void OnEnable()
    {
        if (sunSensor != null)
            sunSensor.DataReceived += OnDataReceived;

        
    }

    void OnDisable()
    {
        if (sunSensor != null)
            sunSensor.DataReceived -= OnDataReceived;
    }

    private void OnDataReceived(Seek.SunSensor.V1.SunSensorData data)
    {
        Debug.Log($"SunSensorData: x={data.UnitVector.X:F2}, y={data.UnitVector.Y:F2}, z={data.UnitVector.Z:F2}");
    }
}
