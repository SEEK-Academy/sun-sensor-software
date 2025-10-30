using UnityEngine;
using Seek.SunSensor.V1;
using Assets.Scripts.Sources.UsbSunSensor;

public class SunSensorVisualizer : MonoBehaviour
{
    public FakedUsbSunSensorSource source; // przeciągnij z inspektora
    public Transform sphere;               // kulka do wizualizacji
    public float distanceScale = 10f;     // skala odległości

    private Vector3 _latestDirection;
    private bool _hasNewData = false;
    private readonly object _lock = new object();

    private void OnEnable()
    {
        if (source != null)
        {
            source.DataReceived += OnDataReceived;
            source.Start();
            Debug.Log("Subskrybowano FakedUsbSunSensorSource");
        }
        else
        {
            Debug.LogError("Brak przypisanego źródła danych!");
        }
    }

    private void OnDisable()
    {
        if (source != null)
            source.DataReceived -= OnDataReceived;
    }

    private void OnDataReceived(SunSensorData data)
    {
        Vector3 dir = new Vector3((float)data.UnitVector.X, (float)data.UnitVector.Y, (float)data.UnitVector.Z);
        lock (_lock)
        {
            _latestDirection = dir.normalized;
            _hasNewData = true;
        }
    }

    private void Update()
    {
        if (_hasNewData)
        {
            lock (_lock)
            {
                sphere.position = transform.position + _latestDirection * distanceScale;
                _hasNewData = false;
            }
        }
    }
}
