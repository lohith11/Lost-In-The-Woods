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
        
    }

    public override void ExitState()
    {

    }
}
