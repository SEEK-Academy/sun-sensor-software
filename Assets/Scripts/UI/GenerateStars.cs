using UnityEngine;

public class Starfield : MonoBehaviour
{
    public int starCount = 200;
    public float minDistance = 15f;
    public float maxDistance = 25f;
    public float starSize = 0.2f;

    void Start()
    {
        for (int i = 0; i < starCount; i++)
        {
            GameObject star = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            star.transform.SetParent(transform);

            // losowa odległość w zakresie
            float distance = Random.Range(minDistance, maxDistance);
            Vector3 randomDir = Random.onUnitSphere;
            star.transform.localPosition = randomDir * distance;

            star.transform.localScale = Vector3.one * starSize;

            // materiał Unlit
            var renderer = star.GetComponent<Renderer>();
            renderer.material = new Material(Shader.Find("Unlit/Color"));
            renderer.material.color = Color.white;

            Destroy(star.GetComponent<Collider>());
        }
    }
}
