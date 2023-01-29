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
        throw new System.NotImplementedException();
    }
}
