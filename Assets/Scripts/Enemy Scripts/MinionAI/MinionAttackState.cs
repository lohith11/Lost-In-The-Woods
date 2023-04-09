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
    }

    public override void UpdateState()
    {
        minionStateManager.minionAgent.transform.position = minionStateManager.playerRef.transform.position;
        minionStateManager.minionAnim.Play("Minion_Attack");

        _goToRoam += Time.deltaTime;
        if (minionStateManager.attackPlayer && _goToRoam > 5)
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
