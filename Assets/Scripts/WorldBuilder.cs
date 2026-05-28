using UnityEngine;
using UnityEngine.Rendering;

public class WorldBuilder : MonoBehaviour
{
    public int islandCount = 7;
    public float islandMinDistance = 20f;
    public float islandMaxDistance = 40f;
    public float islandMinHeight = 6f;
    public float islandMaxHeight = 18f;
    public float islandMinScale = 2.5f;
    public float islandMaxScale = 6f;

    public Color groundColor = new Color(0.06f, 0.08f, 0.18f);
    public Color islandColor = new Color(0.24f, 0.58f, 0.92f);
    public Color portalColor = new Color(0.9f, 0.28f, 0.95f);
    public Color fogColor = new Color(0.03f, 0.02f, 0.05f);
    public Vector3 cameraPosition = new Vector3(-30f, 18f, -24f);
    public Vector3 cameraRotation = new Vector3(20f, 35f, 0f);

    private const string environmentRootName = "Isekai Environment";

    public void BuildWorld()
    {
        DestroyExistingEnvironment();

        GameObject root = new GameObject(environmentRootName);
        CreateGround(root.transform);
        CreateFloatingIslands(root.transform);
        CreatePortal(root.transform);
        SetupLighting();
        SetupFog();
        CreateSkybox();
        CreateAmbientAudio(root.transform);
        SetupCamera();
    }

    private void DestroyExistingEnvironment()
    {
        GameObject existing = GameObject.Find(environmentRootName);
        if (existing != null)
        {
#if UNITY_EDITOR
            DestroyImmediate(existing);
#else
            Destroy(existing);
#endif
        }
    }

    private void CreateGround(Transform parent)
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Isekai Ground";
        ground.transform.parent = parent;
        ground.transform.localPosition = Vector3.zero;
        ground.transform.localRotation = Quaternion.identity;
        ground.transform.localScale = new Vector3(12f, 1f, 12f);

        Material groundMaterial = CreateSurfaceMaterial(groundColor, new Color(0.08f, 0.12f, 0.06f));
        Renderer renderer = ground.GetComponent<Renderer>();
        renderer.sharedMaterial = groundMaterial;
    }

    private void CreateFloatingIslands(Transform parent)
    {
        for (int i = 0; i < islandCount; i++)
        {
            float angle = i * Mathf.PI * 2f / islandCount;
            float radius = Random.Range(islandMinDistance, islandMaxDistance);
            float height = Random.Range(islandMinHeight, islandMaxHeight);
            float scale = Random.Range(islandMinScale, islandMaxScale);
            int primitiveIndex = Random.Range(0, 3);

            Vector3 position = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
            position.y = height;

            PrimitiveType type = primitiveIndex == 0 ? PrimitiveType.Cylinder : primitiveIndex == 1 ? PrimitiveType.Sphere : PrimitiveType.Cube;
            GameObject island = GameObject.CreatePrimitive(type);
            island.name = "Floating Island " + (i + 1);
            island.transform.parent = parent;
            island.transform.position = position;
            island.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            if (type == PrimitiveType.Cube)
            {
                island.transform.localScale = new Vector3(scale * 1.2f, scale * 0.25f, scale * 1.2f);
            }
            else if (type == PrimitiveType.Sphere)
            {
                island.transform.localScale = new Vector3(scale * 1.1f, scale * 0.4f, scale * 1.1f);
            }
            else
            {
                island.transform.localScale = new Vector3(scale, scale * 0.35f, scale);
            }

            Material islandMaterial = CreateSurfaceMaterial(islandColor, new Color(0.18f, 0.12f, 0.08f));
            Renderer islandRenderer = island.GetComponent<Renderer>();
            islandRenderer.sharedMaterial = islandMaterial;
            island.AddComponent<FloatingIslandMotion>();
            CreateIslandDetail(island.transform, scale);
        }
    }

    private void CreateIslandDetail(Transform parent, float scale)
    {
        GameObject detail = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        detail.name = "Island Detail";
        detail.transform.parent = parent;
        detail.transform.localScale = new Vector3(0.4f, 1.5f, 0.4f) * (scale * 0.12f);
        detail.transform.localPosition = new Vector3(0f, -0.9f, 0f);
        SetColor(detail, new Color(0.14f, 0.22f, 0.48f));
    }

    private void CreatePortal(Transform parent)
    {
        GameObject portalRoot = new GameObject("Isekai Portal Group");
        portalRoot.transform.parent = parent;
        portalRoot.transform.localPosition = new Vector3(0f, 8f, 18f);
        portalRoot.transform.localRotation = Quaternion.identity;
        portalRoot.transform.localScale = Vector3.one;

        GameObject portal = GameObject.CreatePrimitive(PrimitiveType.Quad);
        portal.name = "Isekai Portal";
        portal.transform.parent = portalRoot.transform;
        portal.transform.localPosition = Vector3.zero;
        portal.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        portal.transform.localScale = new Vector3(10f, 14f, 1f);

        Renderer renderer = portal.GetComponent<Renderer>();
        Material portalMaterial = new Material(Shader.Find("Standard"));
        portalMaterial.color = portalColor;
        portalMaterial.EnableKeyword("_EMISSION");
        portalMaterial.SetColor("_EmissionColor", portalColor * 2f);
        portalMaterial.SetFloat("_Glossiness", 0.9f);
        portalMaterial.SetFloat("_Mode", 3f);
        portalMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        portalMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        portalMaterial.SetInt("_ZWrite", 0);
        portalMaterial.DisableKeyword("_ALPHATEST_ON");
        portalMaterial.EnableKeyword("_ALPHABLEND_ON");
        portalMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        portalMaterial.renderQueue = 3000;
        portalMaterial.color = new Color(portalColor.r, portalColor.g, portalColor.b, 0.85f);
        renderer.sharedMaterial = portalMaterial;

        CreatePortalRing(portalRoot.transform);
        portalRoot.AddComponent<PortalAnimator>().portalColor = portalColor;
        portalRoot.AddComponent<SpiritParticleSpawner>().SpawnSpiritParticles();
        portalRoot.AddComponent<PortalAmbientSound>();
    }

    private void CreatePortalRing(Transform parent)
    {
        GameObject ring = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        ring.name = "Portal Ring";
        ring.transform.parent = parent;
        ring.transform.localPosition = Vector3.zero;
        ring.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        ring.transform.localScale = new Vector3(4.5f, 0.15f, 4.5f);
        SetColor(ring, portalColor * 0.8f);
    }

    private void SetupLighting()
    {
        Light existingDirectional = Object.FindObjectOfType<Light>();
        if (existingDirectional == null || existingDirectional.type != LightType.Directional)
        {
            GameObject lightGameObject = new GameObject("Isekai Light");
            Light directionalLight = lightGameObject.AddComponent<Light>();
            directionalLight.type = LightType.Directional;
            directionalLight.intensity = 1.2f;
            directionalLight.color = new Color(0.91f, 0.82f, 1f);
            lightGameObject.transform.rotation = Quaternion.Euler(50f, 30f, 0f);
        }

        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.14f, 0.12f, 0.22f);
    }

    private void SetupFog()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = 0.008f;
    }

    private void CreateSkybox()
    {
        Shader skyboxShader = Shader.Find("Skybox/Procedural");
        if (skyboxShader == null)
            return;

        Material skyboxMaterial = new Material(skyboxShader);
        skyboxMaterial.SetColor("_SkyTint", new Color(0.18f, 0.08f, 0.25f));
        skyboxMaterial.SetColor("_GroundColor", new Color(0.05f, 0.02f, 0.12f));
        skyboxMaterial.SetColor("_SunTint", new Color(0.95f, 0.7f, 1f));
        skyboxMaterial.SetFloat("_Exposure", 1.1f);
        skyboxMaterial.SetFloat("_AtmosphereThickness", 0.8f);
        skyboxMaterial.SetFloat("_GroundEmission", 0.2f);
        RenderSettings.skybox = skyboxMaterial;
    }

    private void CreateAmbientAudio(Transform parent)
    {
        GameObject audioObject = new GameObject("Isekai Ambient Audio");
        audioObject.transform.parent = parent;
        audioObject.transform.localPosition = Vector3.zero;
        audioObject.AddComponent<AmbientSoundSet>();
    }

    private void SetupCamera()
    {
        Camera camera = Camera.main;
        if (camera == null)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            camera = cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();
            camera.tag = "MainCamera";
        }

        camera.transform.position = cameraPosition;
        camera.transform.rotation = Quaternion.Euler(cameraRotation);
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = fogColor;
        camera.farClipPlane = 250f;
    }

    private Material CreateSurfaceMaterial(Color baseColor, Color detailColor)
    {
        Material material = new Material(Shader.Find("Standard"));
        material.color = baseColor;
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", detailColor * 0.25f);
        material.SetFloat("_Glossiness", 0.35f);
        material.SetTexture("_MainTex", GenerateNoiseTexture(baseColor, detailColor));
        return material;
    }

    private Texture2D GenerateNoiseTexture(Color baseColor, Color detailColor)
    {
        int size = 256;
        Texture2D texture = new Texture2D(size, size, TextureFormat.ARGB32, false);
        texture.wrapMode = TextureWrapMode.Repeat;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float noise = Mathf.PerlinNoise(x * 0.08f, y * 0.08f);
                float lerp = Mathf.Pow(noise, 1.5f);
                Color pixel = Color.Lerp(baseColor, detailColor, lerp);
                float variation = Mathf.PerlinNoise(x * 0.14f, y * 0.14f) * 0.12f;
                pixel += new Color(variation, variation, variation, 0f);
                pixel.a = 1f;
                texture.SetPixel(x, y, pixel);
            }
        }

        texture.Apply();
        return texture;
    }

    private void SetColor(GameObject obj, Color color)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null)
            return;

        Material material = new Material(Shader.Find("Standard"));
        material.color = color;
        material.enableInstancing = true;
        renderer.sharedMaterial = material;
    }
}
