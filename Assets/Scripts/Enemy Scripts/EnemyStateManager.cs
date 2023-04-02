using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
using UnityEngine.Animations.Rigging;


public class EnemyStateManager : MonoBehaviour
{
    //todo : alert nearby enemies when an enemy dies
    //todo : make the enemy two shot for the body and one shot for the head
    //todo : change enemy to alert state when they are hit with the rock in the body (AKA go for the head!)

    //* singleton
    public static EnemyStateManager manager;

    //* Unity Components 
    [HideInInspector] public Animator enemyAnimController;
    [HideInInspector] public NavMeshAgent enemyAgent;
    [HideInInspector] public Vector3 soundPosition;
    public RigBuilder builder;
    public ThrowingRocks rockThrowing;

    //* Enemy Attributes

    public bool isWayPointPatrol;
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

    [Header("Hearing Properties")]
    [Space(2)]

    [Range(5, 10)] public float hearingRange = 10f;
    public float soundCheckInterval = 1f;
    public bool SoundInRange { get; private set; }
    public float searchForPlayer = 1.5f;

    [Space(10)]

    [Header("Idle state")]
    [Space(2)]

    public float idleTimer;
    
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
    public float chaseSpeed;
    public float attackDuration;
    public float attackCooldown = 2f; //* disable this incase you want one hit kill
    public float damage;
    public float timeSinceLastSighting;
    public float minDistanceToPlayer;
    public bool isAttacking;
    private float _attackTimer;
    public Vector3 lastknownLocation;
    public GameObject spherePrefab;

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

    void Start()
    {
        
        manager = this;

        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimController = GetComponent<Animator>();

        IdleState = new EnemyIdleState(this);
        PatrolState = new EnemyPatrolState(this);
        ChaseState = new EnemyChaseState(this);
        DieState = new EnemyDieState(this);
        AlertState = new EnemyAlertState(this);
        AttackState = new EnemyAttackState(this);
        SearchState = new EnemySearchingState(this);

        alertText.enabled = false;
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
        

        builder.layers[0].active = false;
        spherePrefab.SetActive(false);

        rockThrowing.dealDamage += TakeDamage;

        switchState(IdleState);
    }

    void Update()
    {
        currentState.UpdateState();
    }

    public void switchState(EnemyBaseState Enemy)
    {
        currentState?.ExitState();
        currentState = Enemy;
        Enemy.EnterState();
    }

    public void searchForSounds() => StartCoroutine(CheckForSounds());
    public void AttackPlayer() => StartCoroutine(Attack());
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

    public void TakeDamage(object sender , ThrowingRocks.dealDamageEventArg e)
    {
        if(e.damage == 100)
        {
            switchState(DieState);
        }
        else if(e.damage == 50)
        {
            switchState(AlertState);
        }
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

    private IEnumerator Attack()

    {
        Debug.LogError("Attack Coroutine called");
        while (isAttacking)
        {
            if (_attackTimer <= 0f)
            {
                // spawn spear weapon and set its direction towards player
              //  GameObject spear = Instantiate(spherePrefab, sphereSpawnPoint.position, Quaternion.identity);

                // play attack animation
                //* play the enemy animation here

                //* wait for attack animation to finish
                yield return new WaitForSeconds(attackDuration);

                //* check if player is within attack range
                if (Vector3.Distance(transform.position, playerRef.transform.position) <= attackRadius)
                {
                    //* Deal damage to the player here
                    //* Make a sphere cast from the sphere point and check if there is player -> deal damage
                   //! Debug.LogError("Dealing damage to the player");
                }

                //* reset attack timer and play attack cooldown animation
                _attackTimer = attackCooldown;
            }
            else
            {
                // decrease attack timer
                _attackTimer -= Time.deltaTime;
            }

            yield return null;
        }
    }


}
