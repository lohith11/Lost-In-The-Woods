using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateManager enemy):base(enemy){}
    private float _idleTimer;
    
    private void Start() 
    {
        _idleTimer = 3f;//Random.Range(2,7);   
    }
    public override void EnterState()
    {
        enemyStateManager.enemyAnimController.SetBool("Idle", true);
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
        enemyStateManager.enemyAnimController.SetBool("Idle", false);
    }
}
