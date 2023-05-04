using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlindBrute : MonoBehaviour
{

    [SerializeField] float maxHealth;
    public float health;
    [SerializeField] float damage;
    [SerializeField] float range;
    public NavMeshAgent agent;
    public Animator bossAnimator;
    public Transform[] teleportPoints;
    [SerializeField] float stoppingDistance;
    [SerializeField] bool PlayerInRange;
    public float radius;
    public LayerMask targetMask, obstructionMask;
    public float angle;
    public float attackRadius;


    public PlayerStateMachine playerStateMachine;

    void Start()
    {
        playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        Barrel.explosiveDamage += TakeDamage;
        GetComponent<NavMeshAgent>();
        GetComponent<Animator>();

        StartCoroutine(FOVRoutine());
    }

    void Update()
    {
        Debug.Log("The boss health is : " + health);
        if (Input.GetKeyDown(KeyCode.G))
        {
            AOEAttack();
        }
        if(PlayerInRange)
            transform.LookAt(playerStateMachine.transform.position);
    }

    private void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerStateMachine.transform.position);
        if(PlayerInRange && distanceToPlayer< attackRadius)
        {
            SlashAttack();
        }
        if(distanceToPlayer > attackRadius)
            bossAnimator.Play("Idle Anim");
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

    private void SlashAttack()
    {
        bossAnimator.Play("Normal_Attack");
    }

    public void AOEAttack()
    {
        bossAnimator.Play("Aoe_Attack");
        Collider[] braziers = Physics.OverlapSphere(transform.position, range);
        if (braziers.Length > 0)
        {
            foreach (Collider brazier in braziers)
            {
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
        Debug.Log("Move attack called");
        int targetPosition = UnityEngine.Random.Range(0, teleportPoints.Length);
        Debug.Log("Target position is : " + targetPosition);
        agent.SetDestination(teleportPoints[targetPosition].position);
        bossAnimator.Play("Walk");
        agent.speed = 25f;
        yield return new WaitForSeconds(15f);
        agent.stoppingDistance = stoppingDistance;
        agent.SetDestination(playerStateMachine.transform.position);
    }


    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    PlayerInRange = true;
                else
                    PlayerInRange = false;
            }
            else
                PlayerInRange = false;
        }
        else if (PlayerInRange)
            PlayerInRange = false;
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
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
