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
    public GameObject test;
    public GameObject test1;

    [SerializeField] PlayerStateMachine playerStateMachine;

    void Start()
    {
        playerStateMachine = FindObjectOfType<PlayerStateMachine>();
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
        bossAnimator.Play("Aoe_Attack");
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

    public float GetHealthNormalized()
    {
        return (float)health / maxHealth;
    }

    public void MoveAttack() => StartCoroutine(MoveAndReturn());

    IEnumerator MoveAndReturn()
    {
        agent.SetDestination(test.transform.position);
        yield return new WaitForSeconds(2f);

        agent.SetDestination(test1.transform.position);
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         AudioSource audioSource = other.GetComponent<AudioSource>();
    //         if (audioSource.isPlaying && playerStateMachine.currentState != playerStateMachine.playerCrouchState)
    //         {
    //             if (playerStateMachine.currentState == playerStateMachine.playerRunningState)
    //             {
    //                 PlayerInRange = true;
    //                 detectionCollider.radius = runningDetectionRadius;
    //             }

    //             else if (playerStateMachine.currentState == playerStateMachine.playerMovingState)
    //             {
    //                 PlayerInRange = true;
    //                 detectionCollider.radius = walkingDetectionRadius;
    //             }
    //             else if (playerStateMachine.currentState == playerStateMachine.playerIdleState)
    //             {
    //                 detectionCollider.radius = walkingDetectionRadius;
    //             }
    //         }
    //     }
    // }
    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         AudioSource audioSource = other.GetComponent<AudioSource>();
    //         if (audioSource.isPlaying && playerStateMachine.currentState != playerStateMachine.playerCrouchState)
    //         {
    //             if (playerStateMachine.currentState == playerStateMachine.playerRunningState)
    //             {
    //                 PlayerInRange = true;
    //                 detectionCollider.radius = runningDetectionRadius;
    //             }

    //             else if (playerStateMachine.currentState == playerStateMachine.playerMovingState)
    //             {
    //                 PlayerInRange = true;
    //                 detectionCollider.radius = walkingDetectionRadius;
    //             }

    //             else if (playerStateMachine.currentState == playerStateMachine.playerIdleState)
    //             {
    //                 detectionCollider.radius = walkingDetectionRadius;
    //             }
    //         }
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         PlayerInRange = false;
    //         Debug.Log("Ontrigger Exited");
    //     }
    // }
}
