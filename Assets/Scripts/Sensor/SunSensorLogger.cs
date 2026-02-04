using UnityEngine;
using TMPro; // Correct place for namespaces
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
            Debug.Log($"[Logger] Próba uruchomienia sensora. IsActive: {sunSensor.IsActive}");
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
        _latestDataText = $"x={data.UnitVector.X:F2}, y={data.UnitVector.Y:F2}, z={data.UnitVector.Z:F2}";
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