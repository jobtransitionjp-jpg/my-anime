using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 30f;
    public float zoomSpeed = 5f;
    public float minZoom = 10f;
    public float maxZoom = 100f;

    private float currentZoom = 30f;
    private float currentYaw = 0f;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Rotate camera with arrow keys or A/D
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            currentYaw -= rotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            currentYaw += rotationSpeed * Time.deltaTime;

        // Zoom with scroll wheel or +/- keys
        if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.KeypadPlus) || Input.mouseScrollDelta.y > 0)
            currentZoom = Mathf.Clamp(currentZoom - zoomSpeed, minZoom, maxZoom);
        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus) || Input.mouseScrollDelta.y < 0)
            currentZoom = Mathf.Clamp(currentZoom + zoomSpeed, minZoom, maxZoom);

        // Update camera position and rotation
        Vector3 offset = new Vector3(Mathf.Sin(currentYaw * Mathf.Deg2Rad), 0.6f, Mathf.Cos(currentYaw * Mathf.Deg2Rad)) * currentZoom;
        transform.position = offset;
        transform.LookAt(Vector3.up * 8f);
    }
}
