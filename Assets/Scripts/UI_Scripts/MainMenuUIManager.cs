using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEditor.Rendering;

public class MainMenuUIManager : MonoBehaviour
{
    public TMP_Text sliderValue;
    public Slider brightnessSlider;

    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown framerateDropdown;

    private Resolution[] resolutions;
    private List<string> resolutionOptions = new List<string>();
    private int currentResolutionIndex;
    private float brightnessValue;
    public Volume volume;
    private ColorAdjustments colorAdjustments;

    private void Start()
    {
        // Get the ColorAdjustments component from the Volume
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);

        // Set the initial value of the brightness slider to the current post-exposure value
        brightnessSlider.value = colorAdjustments.postExposure.value;
        // Get available resolutions and populate dropdown options
        resolutions = Screen.resolutions;
        foreach (Resolution resolution in resolutions)
        {
            string option = resolution.width + " x " + resolution.height;
            if (!resolutionOptions.Contains(option))
            {
                resolutionOptions.Add(option);
            }
        }
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);

        // Set current resolution index based on current screen resolution
        currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
                break;
            }
        }
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Set framerate options
        framerateDropdown.ClearOptions();
        framerateDropdown.AddOptions(new List<string> { "60", "75", "120", "144" });
    }

    private void Update()
    {
        // Update the text value of the slider to display the current brightness value
        sliderValue.text = brightnessSlider.value.ToString();

        // Update the brightness value based on the slider's value
        SetBrightness(brightnessSlider.value);
    }

    public void SetBrightness(float brightness)
    {
        // Set the post-exposure value of the ColorAdjustments component
        colorAdjustments.postExposure.value = brightness;
    }


    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFramerateLimit(int limitIndex)
    {
        switch (limitIndex)
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
    }
}