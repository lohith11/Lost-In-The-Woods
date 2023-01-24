using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        Debug.Log("Entered IdleState");
    }

    public override void UpdateState()
    {
        CheckChangeState();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState() 
    {
        Debug.Log("Exited IdleState");
    }

    public override void CheckChangeState()
    {
        if(playerStateMachine.playerInput.magnitude != 0)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerMovingState);
        }
    }
}
