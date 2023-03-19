using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogues;
    public float textSpeed;
    public Dialogues dialoguesforNPC;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        dialogues.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            if(dialogues.text == dialoguesforNPC.lines[index])
            {
                nextDialogue();
            }
            else
            {
                StopAllCoroutines();
                dialogues.text = dialoguesforNPC.lines[index];
            }
        }
    }

    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLines());
    }

    private void nextDialogue()
    {
        if(index < dialoguesforNPC.lines.Length - 1)
        {
            index++;
            dialogues.text = string.Empty;
            StartCoroutine(TypeLines());
        }

        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator TypeLines()
    {
        foreach(char c in dialoguesforNPC.lines[index].ToCharArray())
        {
            dialogues.text += c;
            yield return new WaitForSeconds(textSpeed);
        } 
    }
}
