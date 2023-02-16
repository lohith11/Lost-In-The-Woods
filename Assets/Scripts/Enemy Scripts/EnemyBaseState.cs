
public abstract class EnemyBaseState
{
    public EnemyBaseState(EnemyStateManager enemy)
    {
        enemyStateManager = enemy;
    }
    public EnemyStateManager enemyStateManager;
    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();
    
    
}