using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager Enemy)
    {
        Debug.Log("Enemy moving here and there");
        Enemy.enemyAnimController.SetBool("Patrol",true);
    }

    public override void UpdateState(EnemyStateManager Enemy)
    {
        if(Enemy.alert == true)
        {
            Enemy.switchState(Enemy.alertState);
        }
    }
}
