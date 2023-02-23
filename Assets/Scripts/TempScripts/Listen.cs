using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Listen : MonoBehaviour
{
    public AudioOBJ audioObj;
    public AudioSource source;
    public float range;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            source.clip = audioObj.clip;
            range = audioObj.range;
            if(source.isPlaying)
                return;
            
            source.Play();

           
        }
    }
}
