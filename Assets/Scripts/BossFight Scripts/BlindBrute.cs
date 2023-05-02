using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindBrute : MonoBehaviour
{

    float maxHealth;
    [SerializeField] float health;
    [SerializeField] float damage;
    [SerializeField] float range;
    
    void Start()
    {
        Barrel.explosiveDamage += TakeDamage;
    }

    void Update()
    {

    }

    public void DefaultValues()
    {
        health = maxHealth;
    }

    public void TakeDamage(object sender , dealDamageEventArg e)
    {
        Debug.Log("The boss took damage");
        health -= e.damage;
    }

    public void DealDamage()
    {
        Debug.Log("The boss dealt damage");
    }

    public void AOEAttack()
    {
        Collider[] braziers = Physics.OverlapSphere(transform.position, range);
        if (braziers.Length > 0)
        {
            Debug.Log("Entered if");
            foreach (Collider brazier in braziers)
            {
                Debug.Log("Entered for each");
                Brazier brazierComponent = brazier.GetComponent<Brazier>();
                if (brazierComponent != null)
                {
                    brazierComponent.TurnOff();
                }
            }
        }
    }
}
