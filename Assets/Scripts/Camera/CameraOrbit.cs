using Seek.SunSensor.V1;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Target to orbit around")]
    public Transform target;

    [Header("Distance and speed")]
    public float distance = 30f;
    public float minDistance = 2f;
    public float maxDistance = 20f;
    public float xSpeed = 120f;
    public float ySpeed = 120f;
    public float scrollSpeed = 5f;

    [Header("Vertical limits")]
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    private float x = 0f;
    private float y = 0f;
    private float currentDistance;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float startDistance;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraOrbit: No target assigned!");
            return;
        }

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        currentDistance = distance;

        // zapamiętaj startową pozycję i rotację
        startPosition = transform.position;
        startRotation = transform.rotation;
        startDistance = distance;

        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.freezeRotation = true;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // obrót kamery po prawej myszy
        if (Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            y = Mathf.Clamp(y, yMinLimit, yMaxLimit);
        }

        // scroll: przybliż/oddal
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance = Mathf.Clamp(currentDistance - scroll * scrollSpeed, minDistance, maxDistance);

        // LPM: reset pozycji i rotacji
        if (Input.GetMouseButtonDown(0))
        {
            x = startRotation.eulerAngles.y;
            y = startRotation.eulerAngles.x;
            currentDistance = startDistance;
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0, 0, -currentDistance) + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }
}
