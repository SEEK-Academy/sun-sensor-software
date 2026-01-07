using UnityEngine;
using Assets.Scripts.Sources.UsbSunSensor;
using Seek.SunSensor.V1;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Sources;
using Assets.Scripts.Configurations;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LightCone : MonoBehaviour
{
    [Header("References")]
    public Transform lightSource;                 // sfera z Point Light
    public Transform sensor;                      // cube (cel)

    public ISunSensorRealtimeSource sunSensor;
    //public FakedUsbSunSensorSource sunSensor;     // źródło danych

    [Header("Cone Settings")]
    public float baseRadius = 0.2f;               // promień podstawy przy sensorze
    public float maxRadius = 0.5f;                // maksymalny promień przy dużym odchyleniu
    public float lengthScale = 1.0f;              // długość stożka (mnożnik)
    public float deviationFactor = 0.01f;         // jak silnie odchylenie wpływa na szerokość stożka

    private MeshFilter meshFilter;
    private Vector3 currentDirection = Vector3.forward;
    private float currentDeviation = 0f;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = CreateConeMesh();

        if (sunSensor != null)
            sunSensor.DataReceived += OnSensorData;

         sunSensor = SourceFactory.CreateSunSensorRealtimeSource(ConfigHost.AppSettings);
    }

    void OnDestroy()
    {
        if (sunSensor != null)
            sunSensor.DataReceived -= OnSensorData;
    }

    private void OnSensorData(SunSensorData data)
    {
        // tylko aktualizujemy wektor światła z czujnika
        currentDirection = new Vector3(
            (float)data.UnitVector.X,
            (float)data.UnitVector.Y,
            (float)data.UnitVector.Z
        );
    }


    void Update()
    {
        if (lightSource == null || sensor == null)
            return;

        transform.position = lightSource.position;

        // oblicz kierunek światła
        Vector3 direction = currentDirection.normalized;
        if (direction == Vector3.zero)
            direction = (sensor.position - lightSource.position).normalized;

        transform.rotation = Quaternion.LookRotation(-direction, Vector3.up);

        // oblicz kąt między światłem a osią sensora
        Vector3 incident = currentDirection.normalized;
        Vector3 trueDir = (sensor.position - lightSource.position).normalized;

        float angle = Vector3.Angle(incident, trueDir);
        currentDeviation = Mathf.Lerp(0.0f, deviationFactor, angle / 90f);


        // odległość
        float distance = Vector3.Distance(lightSource.position, sensor.position);

        // szerokość stożka
        float radius = Mathf.Lerp(baseRadius, maxRadius, currentDeviation);

        transform.localScale = new Vector3(radius, radius, distance * lengthScale);
    }



// --- prosty stożek w osi Z ---
Mesh CreateConeMesh()
    {
        Mesh mesh = new Mesh();
        int segments = 24;
        Vector3[] vertices = new Vector3[segments + 2];

        vertices[0] = Vector3.zero; // wierzchołek
        for (int i = 0; i <= segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            vertices[i + 1] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 1f);
        }

        int[] triangles = new int[segments * 3];
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }
}
