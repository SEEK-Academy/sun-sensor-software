using System.Collections.Generic;
using UnityEngine;
using Seek.SunSensor.V1;
using Assets.Scripts.Sources.UsbSunSensor;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Configurations;
using Assets.Scripts.Sources;

public class SunSensorDebugger : MonoBehaviour
{
    [Header("References")]
    public ISunSensorRealtimeSource sunSensor;
    //public FakedUsbSunSensorSource sunSensor;

    public Transform sensorTransform;
    public Transform sunSphere;
    public LightCone lightCone;     // <<- Twój stożek

    [Header("Debug")]
    //public float lineLength = 5f;             // długość linii debugowych
    public bool logEveryFrame = false;        // spam logów on/off

    private Queue<Vector3> dataQueue = new Queue<Vector3>();
    private Vector3 latestVector;

    void Start()
    {
        sunSensor = SourceFactory.CreateSunSensorRealtimeSource(ConfigHost.AppSettings);
    }

    void OnEnable()
    {
        if (sunSensor != null)
            sunSensor.DataReceived += OnDataReceivedThreadSafe;

        //sunSensor = SourceFactory.CreateSunSensorRealtimeSource(ConfigHost.AppSettings);
    }

    void OnDisable()
    {
        if (sunSensor != null)
            sunSensor.DataReceived -= OnDataReceivedThreadSafe;
    }

    private bool newFrame = false;

    private void OnDataReceivedThreadSafe(SunSensorData data)
    {
        lock (dataQueue)
        {
            dataQueue.Enqueue(new Vector3(data.UnitVector.X, data.UnitVector.Y, data.UnitVector.Z));
            newFrame = true;
        }
    }

    void Update()
    {
        bool gotNewFrame = false;

        lock (dataQueue)
        {
            if (dataQueue.Count > 0)
            {
                latestVector = dataQueue.Dequeue().normalized;
                gotNewFrame = true;
            }
        }

        if (!gotNewFrame)
            return;
        Debug.Log("Update działa");

        // odbiór danych z kolejki (bezpiecznie)
        lock (dataQueue)
        {
            if (dataQueue.Count > 0)
                latestVector = dataQueue.Dequeue();
        }

        if (sensorTransform == null)
            return;

        LogDebugInfo();
    }

    void LogDebugInfo()
    {
        if (sunSphere == null || sensorTransform == null || latestVector == Vector3.zero)
            return;

        Vector3 sensorToSun = (sunSphere.position - sensorTransform.position).normalized;
        float angle = Vector3.Angle(latestVector, sensorToSun);

        // dane stożka (jeśli podpięty)
        float coneRadius = lightCone != null ? lightCone.transform.localScale.x : -1f;
        float coneLength = lightCone != null ? lightCone.transform.localScale.z : -1f;

        if (logEveryFrame)
        {
            Debug.Log(
                $"UnitVector = {latestVector} RealDirection = {sensorToSun} Angle Error = {angle:F2}° Cone Radius (base) = {coneRadius:F3} Cone Length = {coneLength:F3}"
            );
        }
    }
}
