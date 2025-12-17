using Seek.SunSensor.V1;
using UnityEngine;
using Assets.Scripts.Models.Config;
using Assets.Scripts.Configurations;

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
    public float keyboardOrbitSpeed = 60f; // szybkość obrotu klawiaturą
    public float keyboardZoomSpeed = 10f;  // szybkość zoomu Z/X

    [Header("Vertical limits")]
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    private float x = 0f;
    private float y = 0f;
    private float currentDistance;

    private Quaternion startRotation;
    private float startDistance;

    private bool isPresetView = false;

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

        LoadConfigFromHost();

        startRotation = transform.rotation;
        startDistance = distance;

        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.freezeRotation = true;
    }

    private void LoadConfigFromHost()
    {
        if (ConfigHost.AppSettings == null)
        { 
            try
            {
                var provider = new FileAppSettingsProvider();
                ConfigHost.Initialize(provider.Load());
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
                return;
            }
        }

        var settings = ConfigHost.AppSettings; 

        if (settings != null && settings.UsbSettings != null)
        {
            currentDistance = settings.UsbSettings.CameraStartDistance;
            ApplyCameraPosition(settings.UsbSettings.CameraStartOrientation);

            Debug.Log($"[CameraOrbit] Załadowano config: Pos={settings.UsbSettings.CameraStartOrientation}, Dist={currentDistance}");
        }
    }

    private void ApplyCameraPosition(int index)
    {
        Vector3 direction = Vector3.forward;
        switch (index)
        {
            case 1: direction = Vector3.forward; break; // Przód RED
            case 2: direction = Vector3.back; break;    // Tył BLUE
            case 3: direction = Vector3.left; break;    // Lewo GREEN
            case 4: direction = Vector3.right; break;   // Prawo YELLOW
            case 5: direction = Vector3.up; break;      // Góra WHITE
            case 6: direction = Vector3.down; break;    // Dół GRAY
            default: direction = Vector3.forward; break; // Domyślnie przód

        }

        SetPredefinedView(direction);
    }

    void LateUpdate()
    {
        if (target == null) return;

        // scroll: przybliż/oddal
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
            currentDistance = Mathf.Clamp(currentDistance - scroll * scrollSpeed, minDistance, maxDistance);

        // zoom klawiaturą Z/X
        if (Input.GetKey(KeyCode.Z))
            currentDistance = Mathf.Clamp(currentDistance - keyboardZoomSpeed * Time.deltaTime, minDistance, maxDistance);
        if (Input.GetKey(KeyCode.X))
            currentDistance = Mathf.Clamp(currentDistance + keyboardZoomSpeed * Time.deltaTime, minDistance, maxDistance);

        // reset spacja
        if (Input.GetKeyDown(KeyCode.Space))
        {
            x = startRotation.eulerAngles.y;
            y = startRotation.eulerAngles.x;
            currentDistance = startDistance;
            isPresetView = false;
        }

        // Predefiniowane widoki
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SetPredefinedView(Vector3.forward); isPresetView = true; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SetPredefinedView(Vector3.back); isPresetView = true; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SetPredefinedView(Vector3.left); isPresetView = true; }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SetPredefinedView(Vector3.right); isPresetView = true; }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { SetPredefinedView(Vector3.up); isPresetView = true; }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { SetPredefinedView(Vector3.down); isPresetView = true; }

        // jeśli użytkownik używa myszy lub strzałek → wyłącz tryb presetowy
        if (Input.GetMouseButton(1) ||
            Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow))
        {
            isPresetView = false;
        }

        // 🔹 Obrót kamery tylko jeśli NIE jesteśmy w trybie presetowym
        if (!isPresetView)
        {
            if (Input.GetMouseButton(1))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                y = Mathf.Clamp(y, yMinLimit, yMaxLimit);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
                x -= keyboardOrbitSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.RightArrow))
                x += keyboardOrbitSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.UpArrow))
                y += keyboardOrbitSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.DownArrow))
                y -= keyboardOrbitSpeed * Time.deltaTime;

            y = Mathf.Clamp(y, yMinLimit, yMaxLimit);
        }

        // 🔹 Zawsze aktualizuj pozycję na podstawie x/y i distance
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0, 0, -currentDistance) + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    private void SetPredefinedView(Vector3 direction)
    {
        if (direction == Vector3.up)
        {
            // Widok z góry (90, 0, 0)
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
        else if (direction == Vector3.down)
        {
            // Widok od dołu (-90, 0, 0)
            transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        }
        else
        {
            // Pozostałe kierunki - tak jak wcześniej
            Vector3 worldDir = target.TransformDirection(direction.normalized);
            Vector3 newPos = target.position + worldDir * currentDistance;
            transform.position = newPos;
            transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
            Vector3 euler = transform.rotation.eulerAngles;
            x = euler.y;
            y = euler.x;
            return;
        }

        // Pozycja kamery wzdłuż osi Y (dla góry i dołu)
        float offset = (direction == Vector3.up) ? currentDistance : -currentDistance;
        Vector3 newPosY = target.position + Vector3.up * offset;
        transform.position = newPosY;

        // Zaktualizuj kąty
        Vector3 finalEuler = transform.rotation.eulerAngles;
        x = finalEuler.y;
        y = finalEuler.x;
    }



}
