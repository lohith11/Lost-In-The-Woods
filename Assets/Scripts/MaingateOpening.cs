using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaingateOpening : MonoBehaviour
{
    public TMP_Text keyNotPickedText;
    PlayerStateMachine playerStateMachine;
    private Animator anim;

    private void Start()
    {
        keyNotPickedText.enabled = false;
        playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) 
        {
            if(playerStateMachine.keyPicked == 0)
            {
                keyNotPickedText.enabled = true;
                keyNotPickedText.text = "You need the Key";
            }
            if(playerStateMachine.keyPicked > 0)
            {
                if(anim!=null)
                {
                    anim.Play("MainGateAnimation");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            keyNotPickedText.enabled = false;
        }
    }
}
