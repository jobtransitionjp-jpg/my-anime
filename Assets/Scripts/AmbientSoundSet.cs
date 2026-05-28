using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientSoundSet : MonoBehaviour
{
    public float volume = 0.16f;
    public float duration = 6f;
    public int sampleRate = 44100;

    private void Awake()
    {
        CreateAudioLayer(90f, 0.08f, 0f);
        CreateAudioLayer(125f, 0.06f, 0f);
        CreateAudioLayer(180f, 0.05f, 0f);
    }

    private void CreateAudioLayer(float frequency, float amplitude, float pan)
    {
        GameObject layerObject = new GameObject("Ambient Sound Layer");
        layerObject.transform.parent = transform;

        AudioSource source = layerObject.AddComponent<AudioSource>();
        source.clip = CreateLoop(frequency, amplitude);
        source.volume = volume;
        source.loop = true;
        source.playOnAwake = true;
        source.spatialBlend = 0f;
        source.panStereo = pan;
        source.Play();
    }

    private AudioClip CreateLoop(float frequency, float amplitude)
    {
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        AudioClip clip = AudioClip.Create("AmbientLayer_" + frequency, sampleCount, 1, sampleRate, false);
        float[] samples = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleRate;
            float envelope = Mathf.Sin(Mathf.PI * t / duration);
            float wave = Mathf.Sin(2f * Mathf.PI * frequency * t);
            float mod = Mathf.Sin(2f * Mathf.PI * 0.25f * t) * 0.25f;
            samples[i] = (wave + mod) * amplitude * envelope;
        }
        clip.SetData(samples, 0);
        return clip;
    }
}
