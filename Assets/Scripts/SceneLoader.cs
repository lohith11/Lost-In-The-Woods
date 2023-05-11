using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System;

public class SceneLoader : MonoBehaviour
{
    public UnityEvent endLevel;
    [SerializeField] BoxCollider endtrigger;
    private void Start()
    {
        endtrigger = GetComponent<BoxCollider>();
        BlindBrute.endBossBattle += EndBattle;
    }
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Slider loadingSlider;


    private void EndBattle(object sender, EventArgs e)
    {
        Debug.Log("Battle ended from game manager");
        endLevel?.Invoke();
    }
    public void LoadLevelButtons()
    {
        int levelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if (mainMenu != null || loadingScreen != null)
        {
            mainMenu.SetActive(false);
            loadingScreen.SetActive(true);
        }

        StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    private IEnumerator LoadLevelAsync(int levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            endLevel?.Invoke();
        }
    }
}
