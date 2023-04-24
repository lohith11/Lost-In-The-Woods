using UnityEngine;

public class ShowText : MonoBehaviour
{
    public GameObject textPrompt;

    private void Awake()
    {
        textPrompt.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            textPrompt.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            textPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            textPrompt.SetActive(false);
        }
    }
}
