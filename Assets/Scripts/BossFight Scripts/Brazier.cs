using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//* This script should handle turning off and on permanantly shut off
public class Brazier : MonoBehaviour
{
    [SerializeField] GameObject light_FX;
    bool isPermanentOff;
    
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void TurnOn()
    {
        light_FX.SetActive(true);
    }

    public void TurnOff()
    {
        light_FX.SetActive(false);
    }
}
