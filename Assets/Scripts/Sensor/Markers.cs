using Seek.SunSensor.V1;
using UnityEngine;
using Assets.Scripts.Models.Config;
using Assets.Scripts.Configurations;

public class Markers : MonoBehaviour
{
    void Start()
    {
        if (ConfigHost.AppSettings != null && ConfigHost.AppSettings.Mode == AppMode.Prod)
            return;
        

        CreateMarker(Vector3.forward, Color.red, "FRONT");
        CreateMarker(Vector3.back, Color.blue, "BACK");
        CreateMarker(Vector3.left, Color.green, "LEFT");
        CreateMarker(Vector3.right, Color.yellow, "RIGHT");
        CreateMarker(Vector3.up, Color.white, "TOP");
        CreateMarker(Vector3.down, Color.gray, "BOTTOM");
    }

    void CreateMarker(Vector3 dir, Color color, string label)
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        marker.transform.SetParent(transform);
        marker.transform.localPosition = dir * 0.6f;
        marker.transform.localScale = Vector3.one * 0.1f;

        var renderer = marker.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = color;

        marker.name = label;
    }
}
