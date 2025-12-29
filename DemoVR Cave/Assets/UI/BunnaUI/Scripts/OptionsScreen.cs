using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class OptionsScreen : MonoBehaviour
{

    public Toggle fullScreen;
    public AudioMixer audioMixer;

    [Header("URP")]
    public UniversalRenderPipelineAsset urpAsset;




    [Header("Toggles")]
    public Toggle fullscreenToggle;
    public Toggle shadowsToggle;
    public Toggle particlesToggle;

    [Header("Dropdowns")]
    public TMP_Dropdown graphicsDropdown;
    public TMP_Dropdown renderScaleDropdown;
    public TMP_Dropdown uiSizeDropdown;

    [Header("Sliders")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

   


    private void Start()
    {
        
        
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        shadowsToggle.isOn = PlayerPrefs.GetInt("Shadows", 1) == 1;
        particlesToggle.isOn = PlayerPrefs.GetInt("Particles", 1) == 1;

        graphicsDropdown.value = PlayerPrefs.GetInt("Graphics", 0);
        renderScaleDropdown.value = PlayerPrefs.GetInt("RenderScale", 0);
        uiSizeDropdown.value = PlayerPrefs.GetInt("UISize", 0);

        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);


    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

    }



    // Called by Apply button
    public void ApplySettings()
    {
        // Save values
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("Shadows", shadowsToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("Particles", particlesToggle.isOn ? 1 : 0);

        PlayerPrefs.SetInt("Graphics", graphicsDropdown.value);
        PlayerPrefs.SetInt("RenderScale", renderScaleDropdown.value);
        PlayerPrefs.SetInt("UISize", uiSizeDropdown.value);

        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);

        PlayerPrefs.Save();

        ApplyToGame();
    }

    void ApplyToGame()
    {
        Screen.fullScreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", 1f);

        int renderScaleIndex = PlayerPrefs.GetInt("RenderScale", 2);

        // Graphics Quality
        int qualityLevel = PlayerPrefs.GetInt("Graphics", 0);
        QualitySettings.SetQualityLevel(qualityLevel, true);


        //RenderScale
        float renderScale = 1f;
        switch (renderScaleIndex)
        {
            case 0: renderScale = 0.7f; break;
            case 1: renderScale = 0.85f; break;
            case 2: renderScale = 1f; break;
        }

        if (urpAsset != null)
        {
            urpAsset.renderScale = renderScale;
        }

        
       

       
    }




}
