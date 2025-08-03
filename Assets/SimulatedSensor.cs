using Assets.Scripts.SunSensor.Interfaces;
using Assets.Scripts.SunSensor.Sources.UsbSunSensor;
using System;
using UnityEngine;

public class SimulatedSensor : MonoBehaviour
{
    //float timer = 0f;
    ISunSensorRealtimeSource source = new FakedUsbSunSensorSource();
    Quaternion rotation = Quaternion.identity;

    // Start is called before the first frame update
    private void Awake()
    {
        source.DataReceived += (data) =>
        {
            Debug.Log($"Data {data}");
            rotation = Quaternion.Euler(
                data.UnitVector.X,
                data.UnitVector.Y,
                data.UnitVector.Z);
        };
        source.Start();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        timer += Time.deltaTime;
        
        float x = Mathf.Sin(timer) * 30f;
        float y = Mathf.Sin(timer * 0.5f) * 45f;
        float z = Mathf.Sin(timer * 0.8f) * 60f;

        transform.rotation = Quaternion.Euler(x, y, z);
        */

        transform.rotation = rotation;
    }

    private void OnDestroy()
    {
        source.Dispose();
    }
}
