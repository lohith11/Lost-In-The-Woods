using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//TODO make the game restart from last checkpoint when the player dies
//TODO start chapter mid checkpoint End chapter
[RequireComponent(typeof(SaveSystem)), RequireComponent(typeof(AudioManager))]
public class GameManager : MonoBehaviour
{
    [Header("Loading Screen")]
    [Space(2)]

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Slider loadingSlider;

    [Space(5)]

    public UnityEvent endLevel;
    public PlayerData playerData;
    public SaveDataManager saveDataManager;

    private void Start()
    {
       // loadingScreen.SetActive(false);
        StartGame();
        BlindBrute.endBossBattle += EndBattle;
        PlayerStateMachine.levelEnd += LoadNextLevel;
    }
    void Update()
    {
        
       
    }

    private void EndBattle(object sender , EventArgs e )
    {
        Debug.Log("Battle ended from game manager");
        endLevel?.Invoke();
    }

    #region PlayerData
    private void StartGame()
    {
        saveDataManager.StartGame();
    }
    public void Respawn()
    {
        saveDataManager.LoadGame();
    }

    #endregion

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
