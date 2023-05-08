using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//* This script should handle turning off and on permanantly shut off
public class Brazier : MonoBehaviour
{
    [SerializeField] GameObject light_FX;
    public static int lightUpBrazierCount;

    
    void Start()
    {
        //TurnOff();
    }

    void Update()
    {
        
    }

    public void TurnOn()
    {
        light_FX.SetActive(true);
        lightUpBrazierCount++;
    }

    public void TurnOff()
    {
        light_FX.SetActive(false);
    }

    public bool IsTurnedOn()
    {
       return light_FX.activeSelf;
    }
}
