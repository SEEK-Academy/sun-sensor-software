using Assets.Scripts.Configurations;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Sources;
using UnityEngine;

public class SimulatedSensor : MonoBehaviour
{
    private ISunVectorRealtimeSource _source;
    private Quaternion _rotation = Quaternion.identity;

    private void Awake()
    {
        _source = SourceFactory.CreateSunVectorRealtimeSource(ConfigHost.AppSettings);

        _source.VectorReceived += (data) =>
        {
            Debug.Log($"Data {data}");
            _rotation = Quaternion.LookRotation(data, Vector3.up);
        };
        _source.Start();
    }

    private void Update()
    {
        if (_source.IsActive)
        {
            transform.rotation = _rotation;
        }
    }

    private void OnDestroy()
    {
        _source.Dispose();
    }
}
