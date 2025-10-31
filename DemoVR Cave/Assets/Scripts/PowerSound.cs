using UnityEngine;

public class PowerSound : MonoBehaviour
{
    public static PowerSound instance;

    private AudioSource sfxSource;

    void Awake()
    {
        // Configura o Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sfxSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, float volume)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }
}
