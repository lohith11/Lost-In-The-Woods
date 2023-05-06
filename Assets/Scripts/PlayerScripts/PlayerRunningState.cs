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
        playerStateMachine.CorStarter(playerStateMachine.FOV, playerStateMachine.FAVdelay);
       // Debug.Log("Entered Running state");
    }

    public override void UpdateState()
    {
        CheckChangeState();
        //To Do image. RunTime/5
        //if (playerStateMachine.isJumping && playerStateMachine.isGrounded)
        //{
        //    playerStateMachine.playerRB.velocity = new Vector3(playerStateMachine.playerRB.velocity.x, playerStateMachine.jumpForce, playerStateMachine.playerRB.velocity.z);
        //}
    }

    public override void FixedUpdateState()
    {
        moveInput = playerStateMachine.transform.right * playerStateMachine.playerInput.x + playerStateMachine.transform.forward * playerStateMachine.playerInput.y;
        moveInput = moveInput * (Time.fixedDeltaTime * playerStateMachine.playerRunSpeed);
        playerStateMachine.playerRB.MovePosition(playerStateMachine.transform.position + moveInput);
        //playerStateMachine.playerRB.velocity = playerStateMachine.transform.TransformDirection(moveInput);
    }

    public override void ExitState()
    {
        playerStateMachine.CorStarter(60f, playerStateMachine.FAVdelay);
       // Debug.Log("Exited Running State");
    }

    public override void CheckChangeState()
    {
        if(!playerStateMachine.isRunning || playerStateMachine.stamina <= 0 || playerStateMachine.playerInput.magnitude == 0) 
        {
            if(playerStateMachine.playerInput.magnitude != 0)
            {
                playerStateMachine.SwitchState(playerStateMachine.playerMovingState);
            }

            else if(playerStateMachine.playerInput.magnitude == 0)
            {
                playerStateMachine.SwitchState(playerStateMachine.playerIdleState);
            }
        }
        
    }
}
