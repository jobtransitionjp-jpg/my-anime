using UnityEngine;

public class SpiritParticleSpawner : MonoBehaviour
{
    public int particleCount = 120;
    public float spawnRadius = 8f;
    public Color spiritColor = new Color(0.9f, 0.6f, 1f, 0.9f);

    public void SpawnSpiritParticles()
    {
        GameObject spiritSystem = new GameObject("Spirit Particles");
        spiritSystem.transform.parent = transform;
        spiritSystem.transform.localPosition = Vector3.zero;

        ParticleSystem ps = spiritSystem.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.maxParticles = particleCount;
        main.startLifetime = new ParticleSystem.MinMaxCurve(2.5f, 4.5f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.8f, 1.8f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.15f, 0.35f);
        main.startColor = new ParticleSystem.MinMaxGradient(spiritColor);
        main.loop = true;
        main.playOnAwake = true;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = ps.emission;
        emission.enabled = true;
        emission.rateOverTime = 25f;

        var shape = ps.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = spawnRadius;

        var vel = ps.velocityOverLifetime;
        vel.enabled = true;
        vel.space = ParticleSystemSimulationSpace.Local;
        vel.x = new ParticleSystem.MinMaxCurve(-0.1f, 0.1f);
        vel.y = new ParticleSystem.MinMaxCurve(0.15f, 0.5f);
        vel.z = new ParticleSystem.MinMaxCurve(-0.1f, 0.1f);

        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(spiritColor, 0f), new GradientColorKey(Color.white, 0.5f), new GradientColorKey(spiritColor * 0.6f, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0.1f, 0f), new GradientAlphaKey(0.9f, 0.2f), new GradientAlphaKey(0f, 1f) }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        Material particleMaterial = new Material(Shader.Find("Particles/Standard Unlit"));
        if (particleMaterial == null)
        {
            particleMaterial = new Material(Shader.Find("Standard"));
        }
        particleMaterial.SetColor("_Color", spiritColor);
        renderer.material = particleMaterial;
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.sortMode = ParticleSystemSortMode.Distance;
    }
}
