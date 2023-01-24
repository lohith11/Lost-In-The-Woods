//This Script contains the functions which can be override by the PlayerStates 
public abstract class PlayerBaseState
{
    public readonly PlayerStateMachine playerStateMachine;

    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        this.playerStateMachine = playerStateMachine;
    }

    public abstract void EnterState();//Which checks the state entered or not & what needs to be done while entering the state

    public abstract void UpdateState();//Acts as Update

    public abstract void FixedUpdateState();//Acts as Fixedupdate

    public abstract void ExitState();//Checks if the state exited or not & what needs to be done while exiting the states

    public abstract void CheckChangeState();//Checks which state to change according to the given conditions

}
