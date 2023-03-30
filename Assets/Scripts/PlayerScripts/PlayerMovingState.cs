using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovingState : PlayerBaseState
{
    private Vector3 moveInput;

    private int moveX;
    private int moveY;
    public PlayerMovingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        moveX = Animator.StringToHash("MoveX");
        moveY = Animator.StringToHash("MoveY");
        playerStateMachine.playerAnimation.SetBool("isMoving", true);
    }
    public override void UpdateState()
    {
        playerStateMachine.playerAnimation.SetFloat(moveX, playerStateMachine.playerInput.x);
        playerStateMachine.playerAnimation.SetFloat(moveY, playerStateMachine.playerInput.y);
        CheckChangeState();
    }

    public override void FixedUpdateState()
    {
        moveInput = new Vector3(playerStateMachine.playerInput.x * playerStateMachine.playerSpeed, playerStateMachine.playerRB.velocity.y, playerStateMachine.playerInput.y * playerStateMachine.playerSpeed);
        playerStateMachine.playerRB.velocity = playerStateMachine.transform.TransformDirection(moveInput);
    }
    public override void ExitState() 
    {
        playerStateMachine.playerAnimation.SetBool("isMoving", false);
    }
    public override void CheckChangeState()
    {
        if(playerStateMachine.playerInput.magnitude == 0)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerIdleState);
        }

        else if(playerStateMachine.isRunning && playerStateMachine.playerInput.y == 1)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerRunningState);
        }

        else if (playerStateMachine.isCrouched)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerCrouchState);
        }

        else if(playerStateMachine.isJumping && playerStateMachine.isGrounded)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerJumpState);
        }
    }
}
