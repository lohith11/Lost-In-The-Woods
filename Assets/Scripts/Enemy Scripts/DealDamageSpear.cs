using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class dealDamageToPlayerEventArgs : EventArgs
{
    public float damage;
}
public class DealDamageSpear : MonoBehaviour
{
    public event EventHandler<dealDamageToPlayerEventArgs> dealDamageToPlayer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            dealDamageToPlayer?.Invoke(this, new dealDamageToPlayerEventArgs { damage = 50});
        }
    }
}
