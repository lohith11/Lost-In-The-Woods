using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : EnemyBaseState
{
    public EnemyDieState(EnemyStateManager enemy):base(enemy){}
    public override void EnterState()
    {
        Debug.Log("Enemy died"); //!
    }
    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }


}
