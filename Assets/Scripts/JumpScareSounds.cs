using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareSounds : MonoBehaviour
{
    private AudioSource jumpScareAudios;
    private void Start()
    {
        jumpScareAudios = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            jumpScareAudios.Play();
        }
    }

   /* private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

        }
    }*/
}
