using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager Enemy)
    {
        Debug.Log("Chasing!");
    }

    public override void UpdateState(EnemyStateManager Enemy)
    {
        throw new System.NotImplementedException();
    }
}
