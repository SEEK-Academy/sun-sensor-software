using UnityEngine;
using Assets.Scripts.Sources.UsbSunSensor;

public class SunSensorLogger : MonoBehaviour
{
    public FakedUsbSunSensorSource sunSensor;

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
        Debug.Log($"SunSensorData: X={data.UnitVector.X:F2}, Y={data.UnitVector.Y:F2}, Z={data.UnitVector.Z:F2}");
    }
}
