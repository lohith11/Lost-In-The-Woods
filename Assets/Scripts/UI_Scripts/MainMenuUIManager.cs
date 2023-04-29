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

    private int targetFrameRate = 60;
    public TMP_Dropdown dropdown;

    public VolumeProfile volumeProfile;
    private ColorAdjustments colorAdjustments;
    public Slider brightnessSlider;

    private void Awake()
    {
        //targetFrameRate = int.Parse(dropdown.options[dropdown.value].text);
        Application.targetFrameRate = targetFrameRate;
    }

    private void Start()
    {
        if (!volumeProfile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            Debug.LogError("ColorAdjustments not found in VolumeProfile!");
            return;
        }

        // Set the initial value of the brightness slider to the current post-exposure value
        brightnessSlider.value = colorAdjustments.postExposure.value;
    }

    public void OnBrightnessChanged(float value)
    {
        // Set the post-exposure value of the ColorAdjustments component
        colorAdjustments.postExposure.value = value;
    }

    private void Update()
    {
        sliderValue.text = " " + brightnessSlider.value;
        SetBrightness(brightnessSlider.value);
    }

    public void SetBrightness(float brightness)
    {
        colorAdjustments.postExposure.value = brightness;
    }

    public void OnDropdownValueChanged()
    {
        switch (dropdown.value)
        {
            case 0:
                targetFrameRate = -1;
                break;
            case 1:
                targetFrameRate = 240;
                break;
            case 2:
                targetFrameRate = 144;
                break;
            case 3:
                targetFrameRate = 120;
                break;
            case 4:
                targetFrameRate = 60;
                break;
        }
        Application.targetFrameRate = targetFrameRate;
    }

}