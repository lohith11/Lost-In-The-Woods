﻿// Script by Marcelli Michele

using System.Linq;
using UnityEngine;

public class PadLockPassword : MonoBehaviour
{
    MoveRuller _moveRull;
    public GameObject key;

    public int[] _numberPassword = {0,0,0,0};

    private void Awake()
    {
        _moveRull = FindObjectOfType<MoveRuller>();
        key.SetActive(false);
    }

    public void Password()
    {
        if (_moveRull._numberArray.SequenceEqual(_numberPassword))
        {
            // Here enter the event for the correct combination
            Debug.Log("Password correct");

            // Es. Below the for loop to disable Blinking Material after the correct password
            for (int i = 0; i < _moveRull._rullers.Count; i++)
            {
                _moveRull._rullers[i].GetComponent<PadLockEmissionColor>()._isSelect = false;
                _moveRull._rullers[i].GetComponent<PadLockEmissionColor>().BlinkingMaterial();
                this.gameObject.SetActive(false);
                _moveRull.lockCamera.SetActive(false);
                _moveRull.playerMainCamera.SetActive(true);
                key.SetActive(true);
            }
        }
    }
}
