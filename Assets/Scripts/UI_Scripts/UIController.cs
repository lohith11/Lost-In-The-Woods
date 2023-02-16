//using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Dropdown graphicsQualityDropdown;
    public Dropdown resolutionDropdown;
    public Slider fovSlider;
    public Slider brightnessSlider;
    public AudioMixer audioMixer;
    public Dropdown keyboardControlsDropdown;
    public Dropdown controllerControlsDropdown;
    public Dropdown languageDropdown;

    private Resolution[] resolutions;

    private void Start()
    {
        // Populate graphics quality dropdown with options
        graphicsQualityDropdown.ClearOptions();
        graphicsQualityDropdown.AddOptions(QualitySettings.names.ToList());
        graphicsQualityDropdown.value = QualitySettings.GetQualityLevel();

        // Populate resolution dropdown with options
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Set FOV slider value
        fovSlider.value = PlayerPrefs.GetFloat("FOV", 60f);

        // Set brightness slider value
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1f);

        // Set audio mixer values
        float masterVolume;
        float musicVolume;
        float sfxVolume;
        audioMixer.GetFloat("MasterVolume", out masterVolume);
        audioMixer.GetFloat("MusicVolume", out musicVolume);
        audioMixer.GetFloat("SFXVolume", out sfxVolume);
        masterVolume = Mathf.Pow(10f, masterVolume / 20f);
        musicVolume = Mathf.Pow(10f, musicVolume / 20f);
        sfxVolume = Mathf.Pow(10f, sfxVolume / 20f);
        GameObject.Find("MasterVolumeSlider").GetComponent<Slider>().value = masterVolume;
        GameObject.Find("MusicVolumeSlider").GetComponent<Slider>().value = musicVolume;
        GameObject.Find("SFXVolumeSlider").GetComponent<Slider>().value = sfxVolume;

        // Populate keyboard controls dropdown with options
        keyboardControlsDropdown.ClearOptions();
        List<string> keyboardOptions = new List<string>();
        keyboardOptions.Add("WASD");
        keyboardOptions.Add("Arrow Keys");
        keyboardControlsDropdown.AddOptions(keyboardOptions);

        // Populate controller controls dropdown with options
        controllerControlsDropdown.ClearOptions();
        List<string> controllerOptions = new List<string>();
        controllerOptions.Add("Xbox 360");
        controllerOptions.Add("PlayStation 4");
        controllerControlsDropdown.AddOptions(controllerOptions);

        // Populate language dropdown with options
        languageDropdown.ClearOptions();
        List<string> languageOptions = new List<string>();
        languageOptions.Add("English");
        languageOptions.Add("French");
        languageOptions.Add("German");
        languageDropdown.AddOptions(languageOptions);
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFOV(float fov)
    {
        fovSlider.value = fov;
        PlayerPrefs.SetFloat("FOV", fov);
    }

    public void SetBrightness(float brightness)
    {
        brightnessSlider.value = brightness;
        PlayerPrefs.SetFloat("Brightness", brightness);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetKeyboardControls(int controlsIndex)
    {
        // Set keyboard controls based on the selected option
        switch (controlsIndex)
        {
            case 0: // WASD
                // TODO: Set controls to WASD
                break;
            case 1: // Arrow Keys
                // TODO: Set controls to arrow keys
                break;
        }
    }

    public void SetControllerControls(int controlsIndex)
    {
        // Set controller controls based on the selected option
        switch (controlsIndex)
        {
            case 0: // Xbox 360
                // TODO: Set controls to Xbox 360 controller
                break;
            case 1: // PlayStation 4
                // TODO: Set controls to PlayStation 4 controller
                break;
        }
    }

    public void SetLanguage(int languageIndex)
    {
        // Set language based on the selected option
        switch (languageIndex)
        {
            case 0: // English
                // TODO: Set language to English
                break;
            case 1: // French
                // TODO: Set language to French
                break;
            case 2: // German
                // TODO: Set language to German
                break;
        }
    }

    public void SaveGame()
    {
        // TODO: Implement save game functionality
    }

    public void ResetGame()
    {
        // TODO: Implement reset game functionality
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
