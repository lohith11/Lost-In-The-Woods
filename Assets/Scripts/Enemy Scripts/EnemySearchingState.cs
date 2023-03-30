using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchingState : EnemyBaseState
{
    public EnemySearchingState(EnemyStateManager enemy) : base(enemy) { }
    public override void EnterState()
    {
        enemyStateManager.SearchForPlayer();
    }

    public override void UpdateState()
    {
      
    }

    public override void ExitState()
    {
       
        
        Debug.LogError("Exit the state");
    }
}

