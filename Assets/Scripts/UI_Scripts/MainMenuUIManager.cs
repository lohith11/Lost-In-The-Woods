using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    public GameObject option1;
    public GameObject option2;
    public GameObject option3;
    public GameObject option4;
    public GameObject option5;
    public GameObject option6;

    public VolumeProfile volumeProfile;
    private ColorAdjustments colorAdjustments;

    public TMP_Text sliderValue;
    public Slider brightnessSlider;

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

}
