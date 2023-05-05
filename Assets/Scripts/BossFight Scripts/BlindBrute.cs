using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlindBrute : MonoBehaviour
{

    [SerializeField] float maxHealth;
    public float health;
    [SerializeField] float slashDamage;
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
    public GameObject player;
    public PlayerHealth playerHealth;
    public GameObject attackPoint;

    [Header("Parabolic jump")]
    [Space(5f)]
    //[SerializeField] Transform targetPosition;
    public float pounceHeight = 15f;
    public float pounceDuration = 3.0f;
    public Transform playerTpPoint;
    [Space(2f)]




    public PlayerStateMachine playerStateMachine;
    public static EventHandler<dealDamageEventArg> bossDamage;

    void Start()
    {
        player = GameObject.Find("Playerrr");
        playerStateMachine = player.GetComponent<PlayerStateMachine>();
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
            //AOEAttack();
            MoveAttack();
            // PounceAttack();
        }

        agent.stoppingDistance = stoppingDistance;
        // if(PlayerInRange)
        //     transform.LookAt(playerStateMachine.transform.position);
    }

    private void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerStateMachine.transform.position);
        if (PlayerInRange && distanceToPlayer < attackRadius)
        {
            SlashAttack();
        }
        if (distanceToPlayer > attackRadius && PlayerInRange)
        {
            bossAnimator.Play("Idle Anim");
            ChasePlayer();
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

    public void DealDamage(float damage)
    {
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint.transform.position, attackRadius, targetMask);
        if (hitColliders.Length > 0)
        {
            if (bossDamage != null)
            {
                // bossDamage.Invoke(this, new dealDamageEventArg { damage = damage });
                Debug.Log("The boss dealt damage");
                PlayerHealth playerHealth = hitColliders[0].GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
            }
            if (playerHealth == null)
            {
                Debug.Log("player not found");
            }
        }

    }

    private void SlashAttack()
    {
        bossAnimator.Play("Normal_Attack");
        DealDamage(slashDamage);

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
        agent.SetDestination(teleportPoints[targetPosition].position);
        bossAnimator.Play("Walk");
        yield return new WaitForSeconds(2f);
        PounceAttack();
    }

    private void ChasePlayer()
    {

    }

    private void PounceAttack()
    {
        Vector3 startPosition = transform.position;
        float startTime = Time.time;
        StartCoroutine(PounceCoroutine(startPosition, startTime, playerTpPoint.transform));
    }

    IEnumerator PounceCoroutine(Vector3 startPosition, float startTime, Transform targetPosition)
    {
        float timeElapsed = 0.0f;

        while (timeElapsed < pounceDuration)
        {
            float normalizedTime = timeElapsed / pounceDuration;
            float parabolicTime = 1 - 4 * (normalizedTime - 0.5f) * (normalizedTime - 0.5f);
            transform.position = Vector3.Lerp(startPosition, targetPosition.position, normalizedTime) + Vector3.up * parabolicTime * pounceHeight;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition.position;
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
