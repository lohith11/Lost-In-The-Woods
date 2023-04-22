using UnityEngine;

public class MinionDieState : MinionBaseState
{
    public MinionDieState(MinionStateManager minion) : base(minion) { }
    public override void EnterState()
    {
       // Debug.Log("Entered die state");
    }

    public override void UpdateState()
    {
        if(minionStateManager.flashLight.intensity >= 500)
        {
            minionStateManager.dieTimer -= Time.deltaTime;
        }

        if(minionStateManager.dieTimer == 0)
        {
            minionStateManager.gameObject.SetActive(false);
        }
    }

    public override void ExitState()
    {
        minionStateManager.dieTimer = 0;
    }
}
