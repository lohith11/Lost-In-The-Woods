using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float _idleTimer;
    
    private void Start() 
    {
        _idleTimer = 3f;//Random.Range(2,7);   
    }
    public override void EnterState(EnemyStateManager Enemy)
    {
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

    public override void ExitState(EnemyStateManager Enemy)
    {
        Enemy.enemyAnimController.SetBool("Idle", false);
    }
}
