
public abstract class EnemyBaseState
{
    public abstract void EnterState(EnemyStateManager Enemy);

    public abstract void UpdateState(EnemyStateManager Enemy);

    public abstract void ExitState(EnemyStateManager Enemy);
    
    
}