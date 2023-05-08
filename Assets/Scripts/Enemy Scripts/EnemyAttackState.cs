using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState (EnemyStateManager enemy):base(enemy){}
    public override void EnterState()
    {
        enemyStateManager.enemyAnimController.CrossFade("Torch_Attack", 0.05f);
    }


    public override void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(enemyStateManager.transform.position , enemyStateManager.playerRef.transform.position);
        if(distanceToPlayer > enemyStateManager.attackRadius)
        {
            enemyStateManager.switchState(enemyStateManager.ChaseState);
        }
        else if (distanceToPlayer < enemyStateManager.attackRadius)
        {
            enemyStateManager.enemyAgent.speed = 0f;
        }
    }

    public override void ExitState()
    {
        enemyStateManager.enemyAgent.speed = enemyStateManager.alertSpeed;
    }

}

