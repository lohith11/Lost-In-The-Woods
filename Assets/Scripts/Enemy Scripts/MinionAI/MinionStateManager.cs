using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionStateManager : MonoBehaviour
{
    public NavMeshAgent minionAgent;
    public GameObject playerRef;
    public float sphereRadius;
    public Vector3 targetLocation;
    public bool attackPlayer;



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

        switchState(AttackState);
    }

    void Update()
    {
        currentState.UpdateState();
    }

    public void switchState(MinionBaseState minion)
    {
        currentState?.ExitState();
        currentState = minion;
        minion.EnterState();
    }

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
}
