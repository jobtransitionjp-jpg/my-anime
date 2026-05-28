using UnityEngine;

public class ParticleEffectSpawner : MonoBehaviour
{
    public int particleCount = 200;
    public float spawnRadius = 50f;
    public Color particleColor = new Color(0.5f, 0.2f, 0.9f);

    public void SpawnParticles()
    {
        GameObject particleSystem = new GameObject("Isekai Particles");
        particleSystem.transform.parent = transform;
        particleSystem.transform.localPosition = Vector3.zero;

        ParticleSystem ps = particleSystem.AddComponent<ParticleSystem>();
        ParticleSystemRenderer renderer = particleSystem.GetComponent<ParticleSystemRenderer>();

        var main = ps.main;
        main.maxParticles = particleCount;
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.5f, 2f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.3f);
        main.startLifetime = new ParticleSystem.MinMaxCurve(3f, 6f);
        main.startColor = new ParticleSystem.MinMaxGradient(particleColor);
        main.loop = true;
        main.playOnAwake = true;

        var emission = ps.emission;
        emission.enabled = true;
        emission.rateOverTime = 25f;

        var shape = ps.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = spawnRadius;

        var velocity = ps.velocityOverLifetime;
        velocity.enabled = true;
        velocity.x = new ParticleSystem.MinMaxCurve(-0.5f, 0.5f);
        velocity.y = new ParticleSystem.MinMaxCurve(0.1f, 0.5f);
        velocity.z = new ParticleSystem.MinMaxCurve(-0.5f, 0.5f);

        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.SetColor("_Color", particleColor);
    }
}
