using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float lookSpeed = 2f;

    private float yaw = 0f;
    private float pitch = 0f;

    void Update()
    {
        // Mouse look (hold right mouse button)
        if (Input.GetMouseButton(1)) // right mouse
        {
            yaw += lookSpeed * Input.GetAxis("Mouse X");
            pitch -= lookSpeed * Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }

        // WASD + QE movement
        Vector3 move = new Vector3();
        if (Input.GetKey(KeyCode.W)) move += transform.forward;
        if (Input.GetKey(KeyCode.S)) move -= transform.forward;
        if (Input.GetKey(KeyCode.A)) move -= transform.right;
        if (Input.GetKey(KeyCode.D)) move += transform.right;
        if (Input.GetKey(KeyCode.Q)) move -= transform.up;
        if (Input.GetKey(KeyCode.E)) move += transform.up;

        transform.position += move * movementSpeed * Time.deltaTime;
    }
}


