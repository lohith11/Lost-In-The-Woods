using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovingState : PlayerBaseState
{
    private Vector3 moveInput;
    private float currentPosition;
    private int moveX;
    private int moveY;
    public PlayerMovingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        moveX = Animator.StringToHash("MoveX");
        moveY = Animator.StringToHash("MoveY");
        playerStateMachine.playerAnimation.SetBool("isMoving", true);
        currentPosition = playerStateMachine.playerCamera.transform.localPosition.y;
    }
    public override void UpdateState()
    {
        if (Mathf.Abs(playerStateMachine.standingHeight - currentPosition) > 0.05f)
        {
            currentPosition = Mathf.Lerp(currentPosition, playerStateMachine.standingHeight, 0.1f);
            playerStateMachine.playerCamera.localPosition = new Vector3(0, currentPosition, 0.3f);
            playerStateMachine.originalPosition = currentPosition;
        }
        playerStateMachine.playerAnimation.SetFloat(moveX, playerStateMachine.playerInput.x);
        playerStateMachine.playerAnimation.SetFloat(moveY, playerStateMachine.playerInput.y);
        CheckChangeState();
    }

    public override void FixedUpdateState()
    {
        moveInput = new Vector3(playerStateMachine.playerInput.x, 0f, playerStateMachine.playerInput.y) * playerStateMachine.playerSpeed * Time.fixedDeltaTime;
        //playerStateMachine.transform.Translate(moveInput);
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

        else if (playerStateMachine.crouchPressed)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerCrouchState);
        }

        //else if(playerStateMachine.isJumping && playerStateMachine.isGrounded)
        //{
        //    playerStateMachine.SwitchState(playerStateMachine.playerJumpState);
        //}
    }
}
