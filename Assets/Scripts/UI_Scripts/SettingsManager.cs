using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [Space(10)]

    // Private variables for storing audio slider values
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    void Start()
    {
        // Load audio slider values from PlayerPrefs and set sliders accordingly
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    void Update()
    {
        // Update text components to show current slider values
        masterSlider.GetComponentInChildren<TMP_Text>().text = masterSlider.value.ToString("F2");
        musicSlider.GetComponentInChildren<TMP_Text>().text = musicSlider.value.ToString("F2");
        sfxSlider.GetComponentInChildren<TMP_Text>().text = sfxSlider.value.ToString("F2");
    }

    // Method for handling changes to master slider
    public void OnMasterVolumeChanged(float value)
    {
        // Set master volume and save to PlayerPrefs
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    // Method for handling changes to music slider
    public void OnMusicVolumeChanged(float value)
    {
        // Set music volume and save to PlayerPrefs
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    // Method for handling changes to SFX slider
    public void OnSFXVolumeChanged(float value)
    {
        // Set SFX volume and save to PlayerPrefs
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    // Method for clearing PlayerPrefs data(A reset button)
    public void ClearPlayerPrefs()
    {
        // Delete all PlayerPrefs data
        PlayerPrefs.DeleteAll();

        // Reset sliders to default values
        masterSlider.value = 1f;
        musicSlider.value = 1f;
        sfxSlider.value = 1f;
    }
}
