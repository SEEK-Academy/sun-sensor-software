using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatedSensor : MonoBehaviour
{
    float timer = 0f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        float x = Mathf.Sin(timer) * 30f;
        float y = Mathf.Sin(timer * 0.5f) * 45f;
        float z = Mathf.Sin(timer * 0.8f) * 60f;

        transform.rotation = Quaternion.Euler(x, y, z);
    }
}
