using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//* 
public class MinionStateManager : MonoBehaviour
{
    public NavMeshAgent minionAgent;
    public Animator minionAnim;
    public Light flashLight;
    public Transform centerPoint;
    [Range(0, 10)] public float sphereRadius;
    public bool attackPlayer;

    [Header("Enemy field of view")]
    [Space(2)]

    public float radius;
    [Range(0, 360)]
    public float angle;
    public GameObject playerRef;
    public LayerMask targetMask, obstructionMask;
    public bool PlayerInRange { get; private set; }

    [Space(10)]

    MinionBaseState currentState;

    #region MinionStates

    public MinionRoamState RoamState;
    public MinionAttackState AttackState;
    public MinionDieState DieState;

    #endregion

    void Start()
    {
        AttackState = new MinionAttackState(this);
        DieState = new MinionDieState(this);
        RoamState = new MinionRoamState(this);

        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
        switchState(RoamState);

    }

    void Update()
    {
        Debug.Log("Player in range + " + PlayerInRange);
        if (PlayerInRange && flashLight.intensity == 500)
        {
            Debug.Log("Flashlight intensity is : " + flashLight.intensity); //!
            switchState(DieState);
        }
        else if (PlayerInRange && flashLight.intensity == 100)
        {
           Debug.Log("Flashlight intensity is : " + flashLight.intensity); //!
        }
        currentState.UpdateState();


    }

    public void switchState(MinionBaseState minion)
    {
        currentState?.ExitState();
        currentState = minion;
        minion.EnterState();
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

    #region Commented code
    // public void startAttack() => StartCoroutine(Attack());
    // public void BackToAttack() => StartCoroutine(Roam());
    // public void stopAttack() => StopCoroutine(Attack());
    // public void stopRoam() => StopCoroutine(Roam());


    // private IEnumerator Attack()
    // {

    //     // while (attackPlayer)
    //     // {

    //     //     if (attackPlayer)
    //     //     {
    //     //         minionAgent.SetDestination(playerRef.transform.position);
    //     //     }
    //     //     yield return new WaitForSeconds(10f);
    //     //     attackPlayer = false;
    //     // }
    //     // yield return new WaitForSeconds(2f);
    //     // attackPlayer = true;



    //     switchState(AttackState);
    //     yield return new WaitForSeconds(10f);
    //     switchState(RoamState);
    // }

    // private IEnumerator Roam()
    // {
    //     yield return new WaitForSeconds(3f);
    //     switchState(AttackState);
    // }
    #endregion
}
