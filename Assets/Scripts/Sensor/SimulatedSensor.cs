using Assets.Scripts.Configurations;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Sources;
using UnityEngine;

public class SimulatedSensor : MonoBehaviour
{
    private ISunSensorRealtimeSource _source;
    private Quaternion _rotation = Quaternion.identity;

    private void Awake()
    {
        _source = (ISunSensorRealtimeSource) SourceFactory.CreateSunVectorRealtimeSource(ConfigHost.AppSettings);

        _source.VectorReceived += (data) =>
        {
            Vector3 direction = new Vector3(
                (float)data.UnitVector.X,
                (float)data.UnitVector.Y,
                (float)data.UnitVector.Z
            );

            if (direction != Vector3.zero)
            {
                _rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
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
