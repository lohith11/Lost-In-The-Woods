using System.IO;
using UnityEngine;

//TODO make the game restart from last checkpoint when the player dies
//TODO start chapter mid checkpoint End chapter
[RequireComponent(typeof(SaveSystem))]
public class GameManager : MonoBehaviour
{
    public PlayerData playerData;

    private SaveSystem saveSystem;

    private void Awake()
    {
        saveSystem = GetComponent<SaveSystem>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            SaveGame();
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            LoadGame();
        }
    }

    public void SaveGame()
    {
        playerData.currentRockCount = ThrowingRocks.totalThrows;
        playerData.currentHealth = PlayerHealth.Health;
        playerData.maxHealth = PlayerHealth.maxHealth;
        playerData.curentPosition = PlayerStateMachine.playerCurrentPosition;
        saveSystem.Save();
    }

    public void LoadGame()
    {
        playerData = saveSystem.Load();
    }
}
