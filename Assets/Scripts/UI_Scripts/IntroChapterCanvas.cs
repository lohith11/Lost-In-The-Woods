using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroChapterCanvas : MonoBehaviour
{
    public GameObject introPannel;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        introPannel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        introPannel.SetActive(false);
        Time.timeScale = 1f;
    }
}
