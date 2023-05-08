using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    [Header("Default Values")]
    [Space(2)]

    public int defaultChapter = 1;
    public int defaultRockCount = 0;
    public int defaultHealth = 100;
    public int defaultMaxHealth = 100;

    [Space(5)]

    public PlayerData playerData;
    private SaveSystem saveSystem;

    private void Awake()
    {
        saveSystem = GetComponent<SaveSystem>();
    }
    void Start()
    {
        PlayerStateMachine.saveGame += SaveData;
        DeathUiScreenButton.respawnEvent += Respawn;
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

    public void SaveData(object sender , EventArgs e)
    {
        Debug.Log("Save game event recieved");
        SaveGame();
    }
    public void Respawn(object sender , EventArgs e)
    {
        LoadGame();
    }
    public void NewGame()
    {
        ThrowingRocks.totalThrows = defaultRockCount;
        PlayerHealth.Health = defaultHealth;
        PlayerHealth.maxHealth = defaultMaxHealth;
    }

    public void StartGame()
    {
        bool newSave = File.Exists(saveSystem.filePath);
        if (newSave)
            NewGame();
        else
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

    public void LoadGame()
    {
        playerData = saveSystem.Load();
        SceneManager.LoadScene(playerData.currentChapter);
        PlayerStateMachine.playerCurrentPosition = playerData.curentPosition;
        ThrowingRocks.totalThrows = playerData.currentRockCount;
        PlayerHealth.Health = playerData.currentHealth;
    }

}
