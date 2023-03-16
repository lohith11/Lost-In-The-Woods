using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu Elements")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button mainMenuButton;

    private bool isPaused;

    void Start()
    {
        isPaused = false;
        pauseMenu.SetActive(false);

        resumeButton.onClick.AddListener(Resume);
        quitButton.onClick.AddListener(Quit);
        mainMenuButton.onClick.AddListener(MainMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f; // Freeze time
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f; // Unfreeze time
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Quit()
    {
        Application.Quit();
    }

    void MainMenu()
    {
        Time.timeScale = 1f; // Unfreeze time
        SceneManager.LoadScene("Main Menu");
    }
}