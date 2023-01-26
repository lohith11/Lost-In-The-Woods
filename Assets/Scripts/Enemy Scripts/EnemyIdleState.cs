using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float _idleTimer = 5f;
    public override void EnterState(EnemyStateManager Enemy)
    {
        Debug.Log("Enemy is in Idle state");
        Enemy.enemyAnimController.SetBool("Idle", true);
    }

    public override void UpdateState(EnemyStateManager Enemy)
    {
        _idleTimer -= Time.deltaTime;
        if(_idleTimer <= 0f)
        {
            Enemy.switchState(Enemy.PatrolState);
            _idleTimer = 0;
        }
        
    }

    
}
