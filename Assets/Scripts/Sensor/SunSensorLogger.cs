using UnityEngine;
using TMPro; 
using Assets.Scripts.Sources.UsbSunSensor;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Sources;
using Assets.Scripts.Configurations;
using Seek.SunSensor.V1;

public class SunSensorLogger : MonoBehaviour
{
    public TMP_Text statusLabel;
    public ISunSensorRealtimeSource sunSensor;
    private string _latestDataText;

    void Start()
    {
        sunSensor = SourceFactory.CreateSunSensorRealtimeSource(ConfigHost.AppSettings);

        if (sunSensor != null)
        {
            sunSensor.DataReceived += OnDataReceived;
            sunSensor.Start();
        }
    }

    void Update()
    {
        if (statusLabel != null)
        {
            statusLabel.text = _latestDataText;
        }
    }

    void OnDataReceived(SunSensorData data)
    {
        _latestDataText = $"x={data.UnitVector.X:F2}, " +
            $"y={data.UnitVector.Y:F2}, " +
            $"z={data.UnitVector.Z:F2}\n" +
            $"Szacowana dokładność={Mathf.Abs(Mathf.Round(data.StdDeviation * 100))}%\nr={Mathf.Lerp(0.2f, 0.5f, 1 - data.StdDeviation)}";
        Debug.Log(_latestDataText);
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
}