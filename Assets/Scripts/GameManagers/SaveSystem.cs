using System;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static readonly string SAVE_FOLDER = Application.persistentDataPath + "/Saves/";
    private const string SAVE_EXTENSION = ".json";
    private GameManager gameManager;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
        gameManager = GetComponent<GameManager>();
    }

    public void Save()
    {
        DateTime currentDate = DateTime.Now.Date;
        string saveData = JsonUtility.ToJson(gameManager.playerData);
        File.WriteAllText(SAVE_FOLDER + "save_" + currentDate + "." + SAVE_EXTENSION, saveData);
    }

    public PlayerData Load()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        //* Get all the save files
        FileInfo[] saveFiles = directoryInfo.GetFiles("*." + SAVE_EXTENSION);
        //* Check for the most recent save
        FileInfo mostrecentSave = null;
        foreach (FileInfo fileInfo in saveFiles)
        {
            if (mostrecentSave == null || fileInfo.LastWriteTime > mostrecentSave.LastWriteTime)
            {
                mostrecentSave = fileInfo;
            }
        }
        if (mostrecentSave != null)
        {
            string loadData = mostrecentSave.FullName;
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(loadData);
            return playerData;
        }
        else
        {
            return null;
        }

    }
}
