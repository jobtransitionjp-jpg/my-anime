using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public float volume = 0.35f;
    public bool playOnAwake = true;
    public bool loop = true;

    private void Awake()
    {
        AudioClip clip = Resources.Load<AudioClip>("BGM");
        if (clip == null)
        {
            Debug.LogWarning("BGM clip not found in Assets/Resources/BGM.wav");
            return;
        }

        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
        source.playOnAwake = playOnAwake;
        source.spatialBlend = 0f;
        source.Play();
    }
}
