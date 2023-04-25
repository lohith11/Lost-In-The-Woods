using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
public class EnemyStateManager : MonoBehaviour
{
    //todo : alert nearby enemies when an enemy dies
    //todo : change speed of enemy based on the state that they are in
    //todo : make the enemy two shot for the body and one shot for the head
    //todo : change enemy to alert state when they are hit with the rock in the body (AKA go for the head!)

    //* singleton
    public static EnemyStateManager manager;

    //* Unity Components 
    [HideInInspector] public Animator enemyAnimController;
    [HideInInspector] public NavMeshAgent enemyAgent;
    [HideInInspector] public Vector3 soundPosition;
    [HideInInspector] public AnimatorStateInfo stateInfo;

    //* Enemy Attributes

    public bool isWayPointPatrol; //* This changes how the enemy patrols around
    public float stoppingDistance;

    [Header("Enemy field of view")]
    [Space(2)]

    public float radius;
    [Range(0, 360)]
    public float angle;
    public GameObject playerRef;
    public LayerMask targetMask, obstructionMask;
    public bool PlayerInRange { get; private set; }

    [Space(10)]

    [Header("Idle Properties")]
    [Space(2)]

    public float idleTimer;
    [Space(5)]

    [Header("Hearing Properties")]
    [Space(2)]

    [Range(5, 10)] public float hearingRange = 10f;
    public float soundCheckInterval = 1f;
    public bool SoundInRange { get; private set; }
    public float searchForPlayer = 1.5f;

    [Space(10)]

    [Header("Way Point Patrol properties")]
    [Space(2)]

    public Transform[] waypoints;
    public Vector3 nextLocation;
    public int destinationLoop;
    public float rotationSpeed;

    [Space(10)]

    [Header("Patrol properties")]
    [Space(2)]

    public float sphereRadius;    //* The radius in which the enemy patrols
    public float startChaseTimer;
    public Transform centerPoint; //* The center point around whcich the patrol shphere is drawn 
    public float patrolSpeed;

    [Space(10)]

    [Header("Alert properties")]
    [Space(2)]

    public float backToPatrol = 2.0f;
    public float alertSpeed;
    public TMP_Text alertText;
    //* make this into explamation image and a slider 

    [Space(10)]

    [Header("Attack Properties")]
    [Space(2)]

    [Range(1, 7)] public float attackRadius;
    public float timeSinceLastSighting;
    public float minDistanceToPlayer;
    public float chaseSpeed;
    public float attackDuration;
    public float attackCooldown = 2f; //* disable this incase you want one hit kill
    public float damage;
    public bool isAttacking;
    private float _attackTimer;
    public Vector3 lastknownLocation;

    [Space(10)]

    [Header("Death properties")]
    [Space(2)]

    public float detectRange;

    [Space(10)]


    EnemyBaseState currentState;

    #region EnemyStates

    public EnemyIdleState IdleState;
    public EnemyPatrolState PatrolState;
    public EnemyAlertState AlertState;
    public EnemySearchingState SearchState;
    public EnemyChaseState ChaseState;
    public EnemyAttackState AttackState;
    public EnemyDieState DieState;

    #endregion

    private void Awake()
    {
        manager = this;
    }
    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimController = GetComponent<Animator>();

        IdleState = new EnemyIdleState(this);
        PatrolState = new EnemyPatrolState(this);
        ChaseState = new EnemyChaseState(this);
        SearchState = new EnemySearchingState(this);
        AlertState = new EnemyAlertState(this);
        AttackState = new EnemyAttackState(this);
        DieState = new EnemyDieState(this);

        alertText.enabled = false;
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
        switchState(IdleState);
    }

    void Update()
    {
        currentState.UpdateState();
        Debug.Log("UngaBungaBoi current state is :: " + currentState);
        if(PlayerInRange)
        {
            transform.LookAt(transform.position,playerRef.transform.position);
        }
    }

    public void switchState(EnemyBaseState Enemy)
    {
        currentState?.ExitState();
        currentState = Enemy;
        Enemy.EnterState();
    }

    public void searchForSounds() => StartCoroutine(CheckForSounds());
    public void AttackPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius, targetMask);

        if (hitColliders.Length > 0)
        {
            Debug.Log("Found someting"); //!
            PlayerHealth player = hitColliders[0].GetComponent<PlayerHealth>();
            if (player != null)
            {
                Debug.Log("Ready to deal damage"); //!
                player.TakeDamage(damage);
            }
            else if(player == null)
                Debug.Log("Player not found");
        }
    }
    public void SearchForPlayer() => StartCoroutine(GoTosoundLocations());
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

    private IEnumerator CheckForSounds()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, hearingRange);
            foreach (Collider collider in colliders)
            {
                MakeSound sound = collider.GetComponent<MakeSound>();
                if (sound != null)
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < sound.soundRange)
                    {
                        SoundInRange = true;
                        soundPosition = collider.transform.position;
                    }
                }
            }
            yield return new WaitForSeconds(soundCheckInterval);
        }
    }
    private IEnumerator GoTosoundLocations()
    {
        enemyAgent.SetDestination(soundPosition);
        enemyAnimController.Play("Finding_Anim");
        yield return new WaitForSeconds(searchForPlayer);
        SoundInRange = false;
        switchState(PatrolState);
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


}
