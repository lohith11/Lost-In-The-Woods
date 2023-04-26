using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO make the game restart from last checkpoint when the player dies
//TODO start chapter mid checkpoint End chapter
[RequireComponent(typeof(SaveSystem)), RequireComponent(typeof(AudioManager)), RequireComponent(typeof(ToggleColliders))]
public class GameManager : MonoBehaviour
{

    [Header("Default Values")]
    [Space(2)]
    
    public int defaultChapter = 1;
    public int defaultRockCount = 0;
    public int defaultHealth = 100;
    public int defaultMaxHealth = 100;

    [Space(5)]

    [SerializeField] Transform checkPoint, endPoint;
    public PlayerData playerData;

    private SaveSystem saveSystem;



    private void Awake()
    {
        saveSystem = GetComponent<SaveSystem>();
    }

    private void Start()
    {
        StartGame();
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
}
