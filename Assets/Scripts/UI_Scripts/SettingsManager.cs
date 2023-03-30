using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Display Settings")]
    [Space(10)]

    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown framerateDropdown;
    [SerializeField] private TMP_Dropdown displayModeDropdown;

    private Resolution[] resolutions;
    private List<string> resolutionOptions = new List<string>();
    private int currentResolutionIndex;

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

        //Get available resolutions and populate dropdown options
        resolutions = Screen.resolutions;
        foreach (Resolution resolution in resolutions) 
        {
            if (resolution.width >= 1280 && resolution.height >= 720)
            {
                string option = resolution.width + " x " + resolution.height;
                if (!resolutionOptions.Contains(option))
                {
                    resolutionOptions.Add(option);
                }
            }
        }
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);

        //Set current resolution index based on current screen resolution
        currentResolutionIndex = PlayerPrefs.GetInt("resolutionIndex",0);
        if(currentResolutionIndex < 0 || currentResolutionIndex >= resolutions.Length)
        {
            currentResolutionIndex = 0;
        }
        else
        {
            Resolution resolution = resolutions[currentResolutionIndex];
            if(resolution.width != Screen.currentResolution.width || resolution.height != Screen.currentResolution.height)
            {
                currentResolutionIndex = 0;
            }
        }
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        //Set framerate options
        framerateDropdown.ClearOptions();
        framerateDropdown.AddOptions(new List<string> { "60", "75", "120", "144" });

        //Set current framerate limit index based on PlayerPrefs 
        int currentLimitIndex = PlayerPrefs.GetInt("limitIndex", 0);
        if(currentLimitIndex < 0 || currentLimitIndex >= framerateDropdown.options.Count)
        {
            currentLimitIndex = 0;
        }
        framerateDropdown.value = currentLimitIndex;
        framerateDropdown.RefreshShownValue();

        //Set display mode options
        displayModeDropdown.ClearOptions();
        displayModeDropdown.AddOptions(new List<string> { "FULLSCREEN", "BORDERLESS", "WINDOW"});
        displayModeDropdown.value = PlayerPrefs.GetInt("modeIndex", Screen.fullScreen ? (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen ? 0 : 1) : 2);
        displayModeDropdown.RefreshShownValue();

        ApplySettings();
    }

    void Update()
    {
        // Update text components to show current slider values
        masterSlider.GetComponentInChildren<TMP_Text>().text = masterSlider.value.ToString("F2");
        musicSlider.GetComponentInChildren<TMP_Text>().text = musicSlider.value.ToString("F2");
        sfxSlider.GetComponentInChildren<TMP_Text>().text = sfxSlider.value.ToString("F2");
    }

    public void SetResolution(int resolutionIndex)
    {
        currentResolutionIndex = resolutionIndex;
        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
        ApplySettings();
    }

    public void SetFrameRateLimit(int limitIndex)
    {
        PlayerPrefs.SetInt("limitIndex", limitIndex);
        ApplySettings();
    }

    public void SetDisplayMode(int modeIndex)
    {
        PlayerPrefs.SetInt("modeIndex", modeIndex);
        ApplySettings();
        
    }
    void ApplySettings()
    {
        //Apply resolution values
        Resolution resolution = resolutions[currentResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);

        //Apply framerate values
        switch(framerateDropdown.value)
        {
            case 0:
                Application.targetFrameRate = 60;
                break;
            case 1:
                Application.targetFrameRate = 75;
                break;
            case 2:
                Application.targetFrameRate = 120;
                break;
            case 3:
                Application.targetFrameRate = 144;
                break;
            default:
                Application.targetFrameRate = -1;
                break;
        }

        //Apply display mode values
        switch (displayModeDropdown.value)
        {
            case 0:
                Screen.fullScreen = true;
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreen = true;
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreen = false;
                break;
            default:
                Screen.fullScreen = true;
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
        }
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
