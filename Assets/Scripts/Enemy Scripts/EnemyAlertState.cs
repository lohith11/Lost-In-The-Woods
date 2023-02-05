using UnityEngine;

public class EnemyAlertState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager Enemy)
    {
        Debug.Log("Hello from alert state");
    }


    public override void UpdateState(EnemyStateManager Enemy)
    {
        
    }

    public override void ExitState(EnemyStateManager Enemy)
    {
       
    }
}
