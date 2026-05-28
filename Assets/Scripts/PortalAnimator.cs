using UnityEngine;

public class PortalAnimator : MonoBehaviour
{
    public float ringRotationSpeed = 40f;
    public float pulseSpeed = 2f;
    public Color portalColor = new Color(0.9f, 0.28f, 0.95f);

    private Transform ringTransform;
    private Renderer portalRenderer;
    private Material portalMaterial;
    private float initialScale = 1f;

    private void Awake()
    {
        ringTransform = transform.Find("Portal Ring");

        Transform portal = transform.Find("Isekai Portal");
        if (portal != null)
        {
            portalRenderer = portal.GetComponent<Renderer>();
            if (portalRenderer != null)
            {
                portalMaterial = portalRenderer.material;
                portalMaterial.EnableKeyword("_EMISSION");
                portalMaterial.SetColor("_EmissionColor", portalColor * 1.2f);
            }
        }

        initialScale = transform.localScale.x;
    }

    private void Update()
    {
        if (ringTransform != null)
        {
            ringTransform.Rotate(Vector3.up, ringRotationSpeed * Time.deltaTime, Space.Self);
        }

        float pulse = 0.75f + Mathf.Sin(Time.time * pulseSpeed) * 0.25f;
        transform.localScale = Vector3.one * (initialScale * pulse);

        if (portalMaterial != null)
        {
            float intensity = 1.5f + Mathf.Sin(Time.time * pulseSpeed * 1.2f) * 0.6f;
            portalMaterial.SetColor("_EmissionColor", portalColor * intensity);
        }
    }
}
