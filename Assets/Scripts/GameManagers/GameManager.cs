using System.IO;
using UnityEngine;

//TODO make the game restart from last checkpoint when the player dies
[RequireComponent(typeof(SaveSystem))]
public class GameManager : MonoBehaviour
{
    public PlayerData playerData;

    private SaveSystem saveSystem;

    private void Awake()
    {
        saveSystem = GetComponent<SaveSystem>();
    }

    void Start()
    {

    }


    void Update()
    {

    }

    public void SaveGame()
    {
        //! get all the details of the player and assign them to the player data script
        //! ask Vicky if he wants auto save or manual save 
    }

    public void LoadGame()
    {
        playerData = saveSystem.Load();
        //! assign all the loaded values to the player
    }
}
