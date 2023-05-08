using System;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathUiScreenButton : MonoBehaviour
{
    private PlayerHealth playerHealthRef;
    public static event EventHandler respawnEvent;
    private void Start()
    {
        playerHealthRef = FindObjectOfType<PlayerHealth>();
    }
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            AfterDeath();
        }
    }

    private void AfterDeath()
    {
        if(playerHealthRef != null)
        {
            respawnEvent?.Invoke(this, EventArgs.Empty);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


}
