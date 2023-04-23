using UnityEngine;

public class MinionAttackState : MinionBaseState
{
    float _goToRoam = 0;
    public MinionAttackState(MinionStateManager minion) : base(minion) { }
    public override void EnterState()
    {
        minionStateManager.minionAgent.speed = 15f;
        if (minionStateManager.PlayerInRange)
        {
            minionStateManager.attackPlayer = true;
        }
        if (!minionStateManager.PlayerInRange)
        {
            minionStateManager.attackPlayer = false;
            minionStateManager.switchState(minionStateManager.RoamState);
        }
        minionStateManager.minionAnim.Play("Minion_Attack");
    }

    public override void UpdateState()
    {

        _goToRoam += Time.deltaTime;

        if (minionStateManager.PlayerInRange)
        {
            minionStateManager.minionAgent.SetDestination(minionStateManager.minionTPPoint.position);
            minionStateManager.transform.LookAt(minionStateManager.playerRef.transform.position);
        }

        if (minionStateManager.PlayerInRange && _goToRoam > 5)
        {
            minionStateManager.minionAgent.transform.position = minionStateManager.centerPoint.transform.position;
            minionStateManager.switchState(minionStateManager.RoamState);
        }
        if (!minionStateManager.PlayerInRange)
        {
            minionStateManager.switchState(minionStateManager.RoamState);
        }



    }

    public override void ExitState()
    {
        _goToRoam = 0;
    }



}
