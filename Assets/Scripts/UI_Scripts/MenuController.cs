using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Volume Settings")]
    [Space(5)]

    [SerializeField] AudioMixer mixer;

    //*SLiders
    [SerializeField] private Slider masterSlider =null;
    [SerializeField] private Slider musicSlider =null;
    [SerializeField] private Slider sfxSlider =null;

    //*Variables names exposed from Audio Mixer
    const string MIXER_MASTER = "MasterVolume";
    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SFX = "SFXVolume";

    //*Slider Values converted to Text
    [SerializeField] private TMP_Text masterTextValue = null;
    [SerializeField] private TMP_Text musicTextValue = null;
    [SerializeField] private TMP_Text sfxTextValue = null;

    //*Default Slider values used in ResetButton Function
    [SerializeField] private float defaultMasterVolume = 1.0f;
    [SerializeField] private float defaultMusicVolume = 1.0f;
    [SerializeField] private float defaultSFXVolume = 1.0f;

    [Header("Confirmation Image")]
    [Space(5)]

    //*Image that pops up to denote that the player has saved their settings
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Level To Load")]
    [Space(5)]

    public string _newGameLevel;

    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume); 
        musicSlider.onValueChanged.AddListener(SetMusicVolume); 
        sfxSlider.onValueChanged.AddListener(SetSFXVolume); 
    }
    //Todo Have to finish this
    #region Volume Settings

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat(MIXER_MASTER, Mathf.Log10(volume) * 20);
        //AudioListener.volume = volume;
        masterTextValue.text = volume.ToString("0.00");
    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(volume) * 20);
        //AudioListener.volume = volume;
        musicTextValue.text = volume.ToString("0.00");
    }

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(volume) * 20);
        //AudioListener.volume = volume;
        sfxTextValue.text = volume.ToString("0.00");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        
        //*Image Confirmation
        StartCoroutine(ConfirmationBox());
    }

    #endregion

    #region Reset Button

    public void ResetButton(string MenuType)
    {
        if(MenuType == "Audio")
        {
            //Master Reset
            AudioListener.volume = defaultMasterVolume;
            masterSlider.value = defaultMasterVolume;
            masterTextValue.text = defaultMasterVolume.ToString("0.0");
            //Music Reset
            AudioListener.volume = defaultMusicVolume;
            musicSlider.value = defaultMusicVolume;
            musicTextValue.text = defaultMusicVolume.ToString("0.0");
            //SFX Reset
            AudioListener.volume = defaultSFXVolume;
            sfxSlider.value = defaultSFXVolume;
            sfxTextValue.text = defaultSFXVolume.ToString("0.0");
            VolumeApply();
        }
    }

    #endregion

    #region Image Confirmation

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

    #endregion

    #region Scene Management Functions

    public void PlayGame()
    {
        SceneManager.LoadScene(_newGameLevel);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}

