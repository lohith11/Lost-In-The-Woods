using System;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private string saveDir;
    public string folderName = "Saves";
    private const string SAVE_EXTENSION = ".json";
    private GameManager gameManager;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {

    }
    public void Init()
    {
        Debug.Log("START!!");
        saveDir = Application.persistentDataPath + "/" + "Saves" + "/";
        if (!Directory.Exists(saveDir))
        {
            Directory.CreateDirectory(saveDir);
        }
        gameManager = GetComponent<GameManager>();
    }

    public void Save()
    {
        Debug.Log("Save called");
        DateTime currentDate = DateTime.Now.Date;
        string jsonData = JsonUtility.ToJson(gameManager.playerData, true);
        File.WriteAllText(saveDir + string.Concat("Save", ".json"), jsonData);

    }

    public PlayerData Load()
    {
        
        string jsonData = File.ReadAllText(saveDir + string.Concat("Save", ".json"));
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonData);
        return playerData;
    }
}
