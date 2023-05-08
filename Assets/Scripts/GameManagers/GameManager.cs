using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

//TODO make the game restart from last checkpoint when the player dies
//TODO start chapter mid checkpoint End chapter
[RequireComponent(typeof(SaveSystem)), RequireComponent(typeof(AudioManager))]
public class GameManager : MonoBehaviour
{

    [Header("Default Values")]
    [Space(2)]

    public int defaultChapter = 1;
    public int defaultRockCount = 0;
    public int defaultHealth = 100;
    public int defaultMaxHealth = 100;

    [Space(5)]

    [Header("Loading Screen")]
    [Space(2)]

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Slider loadingSlider;

    [Space(5)]

    public UnityEvent endLevel;
    public PlayerData playerData;
    private SaveSystem saveSystem;

    private void Awake()
    {
        saveSystem = GetComponent<SaveSystem>();
    }

    private void Start()
    {
       // loadingScreen.SetActive(false);
        StartGame();
        BlindBrute.endBossBattle += EndBattle;
        PlayerStateMachine.levelEnd += LoadNextLevel;
        PlayerStateMachine.saveGame += SaveData;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadGame();
        }
       
    }

    private void EndBattle(object sender , EventArgs e )
    {
        Debug.Log("Battle ended from game manager");
        endLevel?.Invoke();
    }

    private void SaveData(object sender , EventArgs e)
    {
        Debug.Log("Save game event recieved");
        SaveGame();
    }

    #region PlayerData
    private void StartGame()
    {
        bool newSave = File.Exists(saveSystem.filePath);
        if (newSave)
            NewGame();
        else
            LoadGame();
    }
    private void NewGame()
    {
        ThrowingRocks.totalThrows = defaultRockCount;
        PlayerHealth.Health = defaultHealth;
        PlayerHealth.maxHealth = defaultMaxHealth;
    }
    public void Respawn()
    {
        LoadGame();
    }
    private void SaveGame()
    {
        Debug.Log("Save game called");
        playerData.currentChapter = SceneManager.GetActiveScene().buildIndex;
        playerData.currentRockCount = ThrowingRocks.totalThrows;
        playerData.currentHealth = PlayerHealth.Health;
        playerData.maxHealth = PlayerHealth.maxHealth;
        playerData.curentPosition = PlayerStateMachine.playerCurrentPosition;

        saveSystem.Save();
    }
    private void LoadGame()
    {
        playerData = saveSystem.Load();
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
