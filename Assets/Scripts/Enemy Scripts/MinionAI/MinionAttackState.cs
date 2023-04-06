using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MinionAttackState : MinionBaseState
{
    float _goToRoam = 0;
    public MinionAttackState(MinionStateManager minion) : base(minion) { }
    public override void EnterState()
    {

        // minionStateManager.startAttack();

        minionStateManager.attackPlayer = true;
        Debug.Log("Entered attack state");
    }

    public override void UpdateState()
    {
        minionStateManager.minionAgent.transform.position = minionStateManager.playerRef.transform.position;

        _goToRoam += Time.deltaTime;
      //  Debug.Log("The timer is : " + _goToRoam);
        if (minionStateManager.attackPlayer && _goToRoam > 5)
        {
            minionStateManager.switchState(minionStateManager.RoamState);
        }
        // if (_goToRoam > 2)
        // {

        //     _goToRoam = 0;
        // }


    }

    public override void ExitState()
    {
       // Debug.Log("Exit attack state and timer is : " + _goToRoam);
        // minionStateManager.stopAttack();
        _goToRoam = 0;
    }



}
