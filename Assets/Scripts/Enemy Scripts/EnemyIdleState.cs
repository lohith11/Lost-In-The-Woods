using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateManager enemy):base(enemy){}
    private float _idleTimer;
    
    public override void EnterState()
    {
        _idleTimer = enemyStateManager.idleTimer;
       enemyStateManager.enemyAnimController.CrossFade("Idle_Anim", 0.1f);
    }

    public override void UpdateState()
    {
        _idleTimer -= Time.deltaTime;
        if(_idleTimer <= 0f)
        {
            enemyStateManager.switchState(enemyStateManager.PatrolState);
            _idleTimer = 0;
        }
        else if(enemyStateManager.PlayerInRange)
            enemyStateManager.switchState(enemyStateManager.AlertState);
        if(enemyStateManager.SoundInRange)
            enemyStateManager.switchState(enemyStateManager.SearchState);
    }

    public override void ExitState()
    {
        
    }
}
