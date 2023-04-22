using UnityEngine;

public class MinionAttackState : MinionBaseState
{
    float _goToRoam = 0;
    public MinionAttackState(MinionStateManager minion) : base(minion) { }
    public override void EnterState()
    {
        if(minionStateManager.PlayerInRange)
        {
            minionStateManager.attackPlayer = true;
        }
        if(!minionStateManager.PlayerInRange)
        {
            minionStateManager.attackPlayer = false;
            minionStateManager.switchState(minionStateManager.RoamState);
            //Debug.Log("Switching to die state");
        }
        //Debug.Log("Entered attack state");
        minionStateManager.minionAnim.Play("Minion_Attack");
    }

    public override void UpdateState()
    {
        //minionStateManager.minionAgent.transform.position = minionStateManager.minionTPPoint.position;
        minionStateManager.minionAnim.Play("Minion_Attack");
        minionStateManager.minionAgent.SetDestination(minionStateManager.minionTPPoint.position);
        minionStateManager.minionAgent.speed = 15f;
        minionStateManager.transform.LookAt(minionStateManager.playerRef.transform.position);

        _goToRoam += Time.deltaTime;
        if (minionStateManager.attackPlayer && _goToRoam > 5)
        {
            minionStateManager.minionAgent.transform.position = minionStateManager.centerPoint.transform.position;
            //minionStateManager.switchState(minionStateManager.RoamState);
            //Debug.LogWarning("Going to roaming");
        }
        if(!minionStateManager.PlayerInRange)
        {
            minionStateManager.switchState(minionStateManager.RoamState);
            Debug.LogWarning("Going to roaming");
        }

    }

    public override void ExitState()
    {
        _goToRoam = 0;
    }



}
