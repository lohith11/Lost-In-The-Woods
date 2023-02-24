using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        Debug.Log("Entered IdleState");
        playerStateMachine.playerAnimation.Play("Player_Idle");
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
        if(playerStateMachine.playerInput.magnitude != 0 && playerStateMachine.isGrounded)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerMovingState);
        }
        
        else if(playerStateMachine.isCrouched)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerCrouchState);
        }

        else if(playerStateMachine.isJumping && playerStateMachine.isGrounded)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerJumpState);
        }
    }
}