using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathUiScreenButton : MonoBehaviour
{
    private PlayerHealth playerHealthRef;
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


}
