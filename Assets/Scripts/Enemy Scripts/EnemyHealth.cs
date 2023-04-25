using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
   
    [SerializeField] float health;
    void Awake()
    {
        RockDestroy.dealDamage += TakeDamage;
    }

    private void Update()
    {
        Debug.Log("Health :-" + health);
    }

    private void TakeDamage(object sender, dealDamageEventArg e)
    {
        if(health != 0)
        {
            health -= e.damage;
        }
    }

}
