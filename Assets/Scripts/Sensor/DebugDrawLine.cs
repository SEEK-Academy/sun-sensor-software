using System.Collections.Generic;
using UnityEngine;
using Seek.SunSensor.V1;
using Assets.Scripts.Interfaces;

public class SunSensorVectorDebugger : MonoBehaviour
{
    public ISunSensorRealtimeSource sunSensor;
    public Transform sensorTransform;
    public Transform sunSphere;
    public float lineLength = 5f;

    private Queue<Vector3> dataQueue = new Queue<Vector3>();
    private Vector3 latestVector;

    void OnEnable()
    {
        if (sunSensor != null)
            sunSensor.DataReceived += OnDataReceivedThreadSafe;
    }

    void OnDisable()
    {
        if (sunSensor != null)
            sunSensor.DataReceived -= OnDataReceivedThreadSafe;
    }

    private void OnDataReceivedThreadSafe(SunSensorData data)
    {
        // tylko zapisujemy dane do kolejki
        lock (dataQueue)
        {
            dataQueue.Enqueue(new Vector3(data.UnitVector.X, data.UnitVector.Y, data.UnitVector.Z).normalized);
        }
    }

    void Update()
    {
        // odczyt danych w głównym wątku
        lock (dataQueue)
        {
            if (dataQueue.Count > 0)
                latestVector = dataQueue.Dequeue();
        }

        if (sensorTransform != null)
        {
            // rysowanie linii
            Debug.DrawLine(sensorTransform.position, sensorTransform.position + latestVector * lineLength, Color.red);

            if (sunSphere != null)
            {
                Debug.DrawLine(sensorTransform.position, sunSphere.position, Color.green);

                // opcjonalnie przesuwamy kulkę
                // sunSphere.position = sensorTransform.position + latestVector * 5f;

            }

            if (sensorTransform != null && sunSphere != null)
            {
                Vector3 sensorToSun = (sunSphere.position - sensorTransform.position).normalized;
                float angle = Vector3.Angle(latestVector, sensorToSun);
                Debug.Log($"Kąt między UnitVector a pozycją słońca: {angle:F2}°");
            }

        }
    }
}
