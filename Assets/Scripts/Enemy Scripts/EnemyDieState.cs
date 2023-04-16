using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : EnemyBaseState
{
    public EnemyDieState(EnemyStateManager enemy):base(enemy){}
    public override void EnterState()
    {
        Debug.Log("Enemy died"); //!
        AlertEnemies();
    }
    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }


    private void AlertEnemies()
    {
        Collider[] nearEnemies = Physics.OverlapSphere(enemyStateManager.enemyAgent.transform.position,enemyStateManager.detectRange);
        foreach(Collider enemies in nearEnemies)
        {
            var enemyManager = enemies.GetComponent<EnemyStateManager>();
            if(enemyManager != null)
            {
                enemyManager.switchState(enemyManager.AlertState);
                Debug.Log("Alert enemies");
            }
        }
    }

}
