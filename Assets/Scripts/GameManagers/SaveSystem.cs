using System;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private string saveDir;
    private const string SAVE_EXTENSION = ".json";
    private GameManager gameManager;
    private SaveDataManager saveDataManager;
    private string folderName = "Saves";

    [HideInInspector] public string filePath;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        saveDir = Application.persistentDataPath + "/" + "Saves" + "/";
        if (!Directory.Exists(saveDir))
        {
            Directory.CreateDirectory(saveDir);
        }
        //gameManager = GetComponent<GameManager>();
        saveDataManager = GetComponent<SaveDataManager>();
    }

    private void Start()
    {
        filePath = saveDir + "Save" + ".json";
    }

    public void Save()
    {
        string jsonData = JsonUtility.ToJson(saveDataManager.playerData, true);
        File.WriteAllText(saveDir + string.Concat("Save", ".json"), jsonData);

    }

    public PlayerData Load()
    {
        string jsonData = File.ReadAllText(saveDir + string.Concat("Save", ".json"));
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonData);
        return playerData;
    }
}
