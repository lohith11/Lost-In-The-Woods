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

    //* This is for testing but can implement the same logic where ever required
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            source.clip = audioObj.clip;
            soundRange  = audioObj.range;

            if(source.isPlaying)
                return;
            source.Play();
        }   
    }

}
