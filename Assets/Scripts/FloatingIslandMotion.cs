using UnityEngine;

public class FloatingIslandMotion : MonoBehaviour
{
    public float bobSpeed = 0.8f;
    public float bobHeight = 0.7f;
    public float rotateSpeed = 12f;

    private Vector3 startPosition;
    private float offset;

    private void Awake()
    {
        startPosition = transform.position;
        offset = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        float bob = Mathf.Sin(Time.time * bobSpeed + offset) * bobHeight;
        transform.position = startPosition + Vector3.up * bob;
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.Self);
    }
}
