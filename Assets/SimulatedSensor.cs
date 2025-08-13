using Assets.Scripts.Interfaces;
using Assets.Scripts.Sources.UsbSunSensor;
using UnityEngine;

public class SimulatedSensor : MonoBehaviour
{
    readonly ISunVectorRealtimeSource source = new FakedCentralSequenceSunSensorSource();
    Quaternion rotation = Quaternion.identity;

    private void Awake()
    {
        source.DataReceived += (data) =>
        {
            Debug.Log($"Data {data}");
            rotation = Quaternion.LookRotation(data, Vector3.up);
        };
        source.Start();
    }

    private void Update()
    {
        if (source.IsActive)
        {
            transform.rotation = rotation;
        }
    }

    private void OnDestroy()
    {
        source.Dispose();
    }
}
