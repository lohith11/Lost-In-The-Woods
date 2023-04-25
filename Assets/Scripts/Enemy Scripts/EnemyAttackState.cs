using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState (EnemyStateManager enemy):base(enemy){}
    public override void EnterState()
    {
        enemyStateManager.enemyAnimController.Play("Torch_Attack");
    }


    public override void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(enemyStateManager.transform.position , enemyStateManager.playerRef.transform.position);
        if(distanceToPlayer > enemyStateManager.attackRadius)
        {
            enemyStateManager.switchState(enemyStateManager.ChaseState);
        }
        else
        {
           // enemyStateManager.AttackPlayer();
        }
    }

    public override void ExitState()
    {

    }

}

