
public abstract class MinionBaseState 
{
    public MinionBaseState(MinionStateManager minion)
    {
        minionStateManager = minion;
    } 
    public MinionStateManager minionStateManager;
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
