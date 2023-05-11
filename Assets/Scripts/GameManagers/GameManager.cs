using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioManager))]
public class GameManager : MonoBehaviour
{
    [Header("Loading Screen")]
    [Space(2)]

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Slider loadingSlider;

    [Space(5)]
    public PlayerData playerData;
    public SaveDataManager saveDataManager;
    public SceneLoader sceneLoader;

    private void Start()
    {
       // loadingScreen.SetActive(false);
        StartGame();
        BlindBrute.endBossBattle += EndBattle;
        PlayerStateMachine.levelEnd += LoadNextLevel;
    }
    private void EndBattle(object sender , EventArgs e )
    {
        sceneLoader.endLevel?.Invoke();
    }

    private void StartGame()
    {
        saveDataManager.StartGame();
    }


    private void LoadNextLevel(object sender , EventArgs e)
    {
        LoadLevelButtons(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void LoadLevelButtons(int levelToLoad)
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    private IEnumerator LoadLevelAsync(int levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
    }
}
