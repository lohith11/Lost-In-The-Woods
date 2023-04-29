using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    public VolumeProfile volumeProfile;
    private ColorAdjustments colorAdjustments;

    public TMP_Text sliderValue;
    public Slider brightnessSlider;

    private int targetFrameRate = 60;
    public TMP_Dropdown dropdown;

    private void Awake()
    {
        targetFrameRate = int.Parse(dropdown.options[dropdown.value].text);
        Application.targetFrameRate = targetFrameRate;
    }

    private void Start()
    {
        volumeProfile.TryGet(out colorAdjustments);
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