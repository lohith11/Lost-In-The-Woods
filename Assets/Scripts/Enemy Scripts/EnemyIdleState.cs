using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateManager enemy):base(enemy){}
    private float _idleTimer;
    
    public override void EnterState()
    {
        _idleTimer = enemyStateManager.idleTimer;
       enemyStateManager.enemyAnimController.Play("Idle_Anim");
       //_idleTimer = 2f; //Random.Range(0f,7f);
    }

    public override void UpdateState()
    {
        _idleTimer -= Time.deltaTime;
        if(_idleTimer <= 0f)
        {
            enemyStateManager.switchState(enemyStateManager.PatrolState);
            _idleTimer = 0;
        }
    }

    public override void ExitState()
    {
        
    }
}
