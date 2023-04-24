using System;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private  string saveDir;
    private const string SAVE_EXTENSION = ".json";
    private GameManager gameManager;
    private string folderName = "Saves";

    public string filePath;

    private void Awake()
    {
        Init();
    }

    private void Start() {
        filePath = saveDir + "Save" + ".json";
    }

    public void Init()
    {
        saveDir = Application.persistentDataPath + "/" + "Saves" + "/";
        if (!Directory.Exists(saveDir))
        {
            Directory.CreateDirectory(saveDir);
        }
        gameManager = GetComponent<GameManager>();
    }

    public void Save()
    {
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
