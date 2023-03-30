using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MakeSound : MonoBehaviour
{
    public AudioOBJ audioObj;
    private AudioSource source;
    public float soundRange;

    private void Start() 
    {
        source = GetComponent<AudioSource>();    
        source.playOnAwake = false;
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Ground")
        {
            source.clip = audioObj.clip;
            soundRange  = audioObj.range;

            if(source.isPlaying)
                return;
            source.Play();
        }    
    }

}
