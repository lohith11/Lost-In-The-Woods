using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovingState : PlayerBaseState
{
    private Vector3 moveInput;
    public PlayerMovingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        //Moving Animation
        Debug.Log("Entered Moving State");
    }
    public override void UpdateState()
    {
        //playerStateMachine.mouseLook.StartCoroutine(playerStateMachine.mouseLook.CameraShakeWhileMoving(0.1f, 0.2f));
        CheckChangeState();
    }

    public override void FixedUpdateState()
    {
        moveInput = new Vector3(playerStateMachine.playerInput.x * playerStateMachine.playerSpeed, playerStateMachine.playerRB.velocity.y, playerStateMachine.playerInput.y * playerStateMachine.playerSpeed);
        playerStateMachine.playerRB.velocity = playerStateMachine.transform.TransformDirection(moveInput);
    }
    public override void ExitState() 
    {
        Debug.Log("Exited Moving State");
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
