using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsScreen : MonoBehaviour
{

    public Toggle fullScreen;
    public AudioMixer audioMixer;

    private void Start()
    {
        
        fullScreen.isOn = Screen.fullScreen;

        
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

    }
}
