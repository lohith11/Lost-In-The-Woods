using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlindBrute : MonoBehaviour
{

    [SerializeField]float maxHealth;
    public float health;
    [SerializeField] float damage;
    [SerializeField] float range;
    public NavMeshAgent agent;
    public Animator bossAnimator;

    void Start()
    {
        Barrel.explosiveDamage += TakeDamage;
        GetComponent<NavMeshAgent>();
        GetComponent<Animator>();
    }

    void Update()
    {
        Debug.Log("Boss current normalized health is : " + GetHealthNormalized());
        if(Input.GetKeyDown(KeyCode.G))
        {
            AOEAttack();
        }
    }

    public void DefaultValues()
    {
        health = maxHealth;
    }

    public void TakeDamage(object sender, dealDamageEventArg e)
    {
        health -= e.damage;
    }

    public void DealDamage()
    {
        Debug.Log("The boss dealt damage");
    }

    public void AOEAttack()
    {
        #region useless 
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
        #endregion
    }

    public float GetHealthNormalized()
    {
        return (float)health / maxHealth;
    }
}
