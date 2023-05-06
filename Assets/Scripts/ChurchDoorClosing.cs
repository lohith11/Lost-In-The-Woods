using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchDoorClosing : MonoBehaviour
{
    private Animator anim;
    private Animator backDoorOpening;
    private PlayerStateMachine playerStateMachine;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        backDoorOpening = GetComponent<Animator>();
        playerStateMachine= FindObjectOfType<PlayerStateMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(anim != null)
            {
                anim.Play("ClosingChurchDoor");
            }

            if(backDoorOpening!=null && playerStateMachine.keyPicked > 0)
            {
                backDoorOpening.Play("ChurchBackDoor");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            anim = null;
            if(playerStateMachine.keyPicked > 0)
            {
                backDoorOpening = null;
            }
        }
    }
}
