using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Level To Load")]
    [Space(5)]

    public int _newGameLevel;
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

