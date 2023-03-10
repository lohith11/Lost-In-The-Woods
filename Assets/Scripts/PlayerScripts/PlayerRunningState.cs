using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningState : PlayerBaseState
{
    private Vector3 moveInput;
    public PlayerRunningState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        playerStateMachine.playerAnimation.Play("Player_Running");
        playerStateMachine.CorStarter(80f, playerStateMachine.FAVdelay);
        Debug.Log("Entered Running state");
    }

    public override void UpdateState()
    {
        CheckChangeState();
        if (playerStateMachine.isJumping && playerStateMachine.isGrounded)
        {
            playerStateMachine.playerRB.velocity = new Vector3(playerStateMachine.playerRB.velocity.x, playerStateMachine.jumpForce, playerStateMachine.playerRB.velocity.z);
        }
    }

    public override void FixedUpdateState()
    {
        moveInput = new Vector3(playerStateMachine.playerInput.x * playerStateMachine.playerRunSpeed, playerStateMachine.playerRB.velocity.y, playerStateMachine.playerInput.y * playerStateMachine.playerRunSpeed);
        playerStateMachine.playerRB.velocity = playerStateMachine.transform.TransformDirection(moveInput);
    }

    public override void ExitState()
    {
        playerStateMachine.CorStarter(60f, playerStateMachine.FAVdelay);
        Debug.Log("Exited Running State");
    }

    public override void CheckChangeState()
    {
        if(!playerStateMachine.isRunning) 
        {
            if(playerStateMachine.playerInput.magnitude != 0 && playerStateMachine.isGrounded)
            {
                playerStateMachine.SwitchState(playerStateMachine.playerMovingState);
            }

            else if(playerStateMachine.playerInput.magnitude == 0 && playerStateMachine.isGrounded)
            {
                playerStateMachine.SwitchState(playerStateMachine.playerIdleState);
            }
        }
        
    }
}
