using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlindBrute : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] float maxHealth;
    public float health;
    [SerializeField] float slashDamage;
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

    public float minDistanceToPlayer;
    [SerializeField] bool canAttack = true;
    [SerializeField] float attackCooldown;

    [Header("Parabolic jump")]
    [Space(5f)]
    public float tpRange;
    public float pounceHeight = 15f;
    public float pounceDuration = 3.0f;
    public Transform playerTpPoint;
    [SerializeField] AudioClip[] audioClips;
    [Space(2f)]

    [Header("Audio detection")]
    [Space(5f)]

    public SphereCollider detectionCollider;
    public float walkingDetectionRadius;
    public float runningDetectionRadius;

    [Space(2f)]
    public PlayerStateMachine playerStateMachine;
    public static EventHandler<dealDamageEventArg> bossDamage;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStateMachine = player.GetComponent<PlayerStateMachine>();
        Barrel.explosiveDamage += TakeDamage;
        GetComponent<NavMeshAgent>();
        GetComponent<Animator>();

        StartCoroutine(FOVRoutine());

        detectionCollider = GetComponent<SphereCollider>();
        detectionCollider.radius = walkingDetectionRadius;
        detectionCollider.isTrigger = true;

        audioSource.PlayOneShot(audioClips[0]);
    }

    void Update()
    {
        Debug.Log("The boss health is : " + health);
        if (Input.GetKeyDown(KeyCode.G))
        {
            MoveAttack();
        }
        agent.stoppingDistance = stoppingDistance;
        if (PlayerInRange)
            transform.LookAt(playerStateMachine.transform.position);
    }

    private void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerStateMachine.transform.position);
        if (distanceToPlayer < attackRadius)
        {
            bossAnimator.Play("Normal_Attack");
        }
        if (distanceToPlayer > attackRadius && PlayerInRange)
        {

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
        HashSet<Collider> hitColliders = new HashSet<Collider>();
        Collider[] colliders = Physics.OverlapSphere(attackPoint.transform.position, attackRadius, targetMask);
        foreach (Collider collider in colliders)
        {
            if (!hitColliders.Contains(collider))
            {
                hitColliders.Add(collider);
                Debug.Log("The boss dealt damage");
                bossDamage?.Invoke(this, new dealDamageEventArg { damage = damage });
            }
        }

    }

    public void AOEAttack()
    {
        bossAnimator.Play("Aoe_Attack");
        Collider[] braziers = Physics.OverlapSphere(transform.position, radius);
        if (braziers.Length > 0)
        {
            foreach (Collider brazier in braziers)
            {
                Brazier brazierComponent = brazier.GetComponent<Brazier>();
                if (brazierComponent != null)
                {
                    brazierComponent.TurnOff();
                    agent.stoppingDistance = 2f;
                    DealDamage(30);
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

    private void PounceAttack()
    {
        Vector3 startPosition = transform.position;
        float startTime = Time.time;
        Vector3 randomPoint = player.transform.position + UnityEngine.Random.insideUnitSphere * tpRange;
        StartCoroutine(PounceCoroutine(startPosition, startTime, randomPoint));
    }

    private void ChasePlayer()
    {
        bossAnimator.Play("Walk");
        Vector3 directionToPlayer = player.transform.position - transform.position;
        float distanceToMove = Mathf.Max(directionToPlayer.magnitude - minDistanceToPlayer, 0f);
        Vector3 targetPosition = transform.position + directionToPlayer.normalized * distanceToMove;
        agent.SetDestination(targetPosition);
    }

    

    IEnumerator PounceCoroutine(Vector3 startPosition, float startTime, Vector3 targetPosition)
    {
        float timeElapsed = 0.0f;

        while (timeElapsed < pounceDuration)
        {
            float normalizedTime = timeElapsed / pounceDuration;
            float parabolicTime = 1 - 4 * (normalizedTime - 0.5f) * (normalizedTime - 0.5f);
            transform.position = Vector3.Lerp(startPosition, targetPosition, normalizedTime) + Vector3.up * parabolicTime * pounceHeight;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioSource audioSource = other.GetComponent<AudioSource>();
            if (audioSource.isPlaying && playerStateMachine.currentState != playerStateMachine.playerCrouchState)
            {
                if (playerStateMachine.currentState == playerStateMachine.playerRunningState)
                {
                    PlayerInRange = true;
                    detectionCollider.radius = runningDetectionRadius;
                }

                else if (playerStateMachine.currentState == playerStateMachine.playerMovingState)
                {
                    PlayerInRange = true;
                    detectionCollider.radius = walkingDetectionRadius;
                }
                else if (playerStateMachine.currentState == playerStateMachine.playerIdleState)
                {
                    detectionCollider.radius = walkingDetectionRadius;
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioSource audioSource = other.GetComponent<AudioSource>();
            if (audioSource.isPlaying && playerStateMachine.currentState != playerStateMachine.playerCrouchState)
            {
                if (playerStateMachine.currentState == playerStateMachine.playerRunningState)
                {
                    PlayerInRange = true;
                    detectionCollider.radius = runningDetectionRadius;
                }

                else if (playerStateMachine.currentState == playerStateMachine.playerMovingState)
                {
                    PlayerInRange = true;
                    detectionCollider.radius = walkingDetectionRadius;
                }

                else if (playerStateMachine.currentState == playerStateMachine.playerIdleState)
                {
                    detectionCollider.radius = walkingDetectionRadius;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInRange = false;
            Debug.Log("Ontrigger Exited");
        }
    }
}
