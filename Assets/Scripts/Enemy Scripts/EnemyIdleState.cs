using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateManager enemy):base(enemy){}
   
    
    public override void EnterState()
    {
        
       enemyStateManager.enemyAnimController.Play("Idle_Anim");
       //enemyStateManager.idleTimer = Random.Range(0f,7f);
    }

    public override void UpdateState()
    {
        enemyStateManager.idleTimer -= Time.deltaTime;
        if(enemyStateManager.idleTimer <= 0f)
        {
            enemyStateManager.switchState(enemyStateManager.PatrolState);
            enemyStateManager.idleTimer = 0;
        }
    }

    public override void ExitState()
    {
        
    }
}
