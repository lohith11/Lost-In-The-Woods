using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager Enemy)
    {
        Debug.Log("Enemy died"); //!
    }
    public override void UpdateState(EnemyStateManager Enemy)
    {
        
    }

    public override void ExitState(EnemyStateManager Enemy)
    {
        
    }


}
