using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    private float currentPosition;
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        //Debug.Log("Entered IdleState");
        currentPosition = playerStateMachine.playerCamera.transform.localPosition.y;
        playerStateMachine.playerAnimation.CrossFade("Player_Idle", 0.1f);
    }

    public override void UpdateState()
    {
        if(Mathf.Abs(playerStateMachine.standingHeight - currentPosition) > 0.05f)
        {
            currentPosition = Mathf.Lerp(currentPosition, playerStateMachine.standingHeight, 0.1f);
            playerStateMachine.playerCamera.localPosition = new Vector3(0, currentPosition, 0.3f);
            playerStateMachine.originalPosition = currentPosition; 
        }
        CheckChangeState();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState() 
    {
       // Debug.Log("Exited IdleState");
    }

    public override void CheckChangeState()
    {
        if(playerStateMachine.playerInput.magnitude != 0)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerMovingState);
        }
        
        else if(playerStateMachine.crouchPressed)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerCrouchState);
        }

        else if(playerStateMachine.isDodging && playerStateMachine.canDodge)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerDodgeState);
        }
        //else if(playerStateMachine.isJumping && playerStateMachine.isGrounded)
        //{
        //    playerStateMachine.SwitchState(playerStateMachine.playerJumpState);
        //}
    }
}