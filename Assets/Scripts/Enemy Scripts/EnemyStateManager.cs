using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{

    //todo : alert nearby enemies when the player is in attack range
    //todo : alert nearby enemies when an enemy dies
    //todo : change speed of enemy based on the state that they are in
    //todo :  make the enemy two shot 
    //todo : change enemy to alert state when they are hit with the rock in the body (AKA go for the head!)
    //todo: make a blend tree for enemy animation 
    //todo : make the enemy switch to alert state and then change the position 
    //* singleton
    public static EnemyStateManager manager;

    //* Unity Components 

    [HideInInspector] public Animator enemyAnimController;
    [HideInInspector] public NavMeshAgent EnemyAgent;
    [HideInInspector] public Vector3 targetPosition;

    //* Enemy Attributes

    [Header("Enemy field of view")]
    [Space(2)]

    public float radius;
    [Range(0, 360)]
    public float angle;
    public GameObject playerRef;
    public LayerMask targetMask, obstructionMask;
    public bool PlayerInRange { get; private set; }
    //public bool playerInRange;

    [Space(10)]

    [Header("Patrol properties")]
    [Space(2)]

    public float sphereRadius;    //* The radius in which the enemy patrols
    public float startChaseTimer;
    public Transform centerPoint; //* The center point around whcich the patrol shphere is drawn 

    [Space(10)]

    [Header("Hearing Properties")]
    [Space(2)]

    [Range(10, 50)] public float hearingRange = 10f;
    public float soundCheckInterval = 1f;
    public bool SoundInRange { get; private set; }
    //private bool soundInRange;

    [Space(10)]

    [Header("Attack Properties")]
    [Space(2)]

    public float attackRadius;
    public float attackSpeed;
    public float damage;
    public float timeSinceLastSighting;
    public float attackCooldown = 2f; //* disable this incase you want one hit kill
    public float minDistanceToPlayer;
    public bool isAttacking;
    public Vector3 lastknownLocation;

    [Space(10)]


    EnemyBaseState currentState;

    #region EnemyStates

    public EnemyIdleState IdleState;
    public EnemyPatrolState PatrolState;
    public EnemyChaseState ChaseState;
    public EnemyDieState DieState;
    public EnemyAlertState AlertState;

    #endregion

    void Start()
    {
        manager = this;

        EnemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimController = GetComponent<Animator>();

        IdleState = new EnemyIdleState(this);
        PatrolState = new EnemyPatrolState(this);
        ChaseState = new EnemyChaseState(this);
        DieState = new EnemyDieState(this);
        AlertState = new EnemyAlertState(this);

        switchState(IdleState);

        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    void Update()
    {
        currentState.UpdateState();
        //Debug.Log("Player in range : " + PlayerInRange);
        Debug.Log("The chase timer is : " + startChaseTimer);
    }

    public void switchState(EnemyBaseState Enemy)
    {
        currentState?.ExitState();
        currentState = Enemy;
        Enemy.EnterState();
    }

    public void searchForSounds() => StartCoroutine(CheckForSounds());

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
                        targetPosition = collider.transform.position;
                    }
                }
            }
            yield return new WaitForSeconds(soundCheckInterval);
        }
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
