using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PortalAmbientSound : MonoBehaviour
{
    public float frequency = 220f;
    public float volume = 0.18f;
    public float duration = 2f;
    public int sampleRate = 44100;

    private void Awake()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = CreateLoop();
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.spatialBlend = 0f;
        audioSource.volume = volume;
        audioSource.Play();
    }

    private AudioClip CreateLoop()
    {
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        AudioClip clip = AudioClip.Create("PortalAmbient", sampleCount, 1, sampleRate, false);
        float[] samples = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            float phase = (float)i / sampleCount;
            float envelope = Mathf.Sin(phase * Mathf.PI);
            float value = Mathf.Sin(2f * Mathf.PI * frequency * i / sampleRate);
            samples[i] = value * envelope * 0.3f;
        }

        clip.SetData(samples, 0);
        return clip;
    }
}
