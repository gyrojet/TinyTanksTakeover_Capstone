using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider engineSlider;

    const string MUSICVOLUME = "musicVolume";
    const string SFXVOLUME = "sfxVolume";
    const string ENGINEVOLUME = "engineVolume";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CheckForExistingSettings();

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        engineSlider.onValueChanged.AddListener(SetEngineVolume);
    }

    
    void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(MUSICVOLUME, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(MUSICVOLUME, value);

        PlayerPrefs.Save();
    }

    void SetSFXVolume(float value)
    {
        audioMixer.SetFloat(SFXVOLUME, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(SFXVOLUME, value);

        PlayerPrefs.Save();
    }

    void SetEngineVolume(float value)
    {
        audioMixer.SetFloat(ENGINEVOLUME, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(ENGINEVOLUME, value);

        PlayerPrefs.Save();
    }

    private void CheckForExistingSettings()
    {
        if (PlayerPrefs.HasKey(MUSICVOLUME))
        {
            musicSlider.value = PlayerPrefs.GetFloat(MUSICVOLUME);
        }
        else
        {
            PlayerPrefs.SetFloat(MUSICVOLUME, 0.5f);
            musicSlider.value = 0.5f;
        }

        if (PlayerPrefs.HasKey(SFXVOLUME))
        {
            sfxSlider.value = PlayerPrefs.GetFloat(SFXVOLUME);
        }
        else
        {
            PlayerPrefs.SetFloat(SFXVOLUME, 1f);
            sfxSlider.value = 1f;
        }

        if (PlayerPrefs.HasKey(ENGINEVOLUME))
        {
            engineSlider.value = PlayerPrefs.GetFloat(ENGINEVOLUME);
        }
        else
        {
            PlayerPrefs.SetFloat(ENGINEVOLUME, 0.35f);
            engineSlider.value = 0.35f;
        }

        PlayerPrefs.Save();

        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
        SetEngineVolume(engineSlider.value);
    }
}
