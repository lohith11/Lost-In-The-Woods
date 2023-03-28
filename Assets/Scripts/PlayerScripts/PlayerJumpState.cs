using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private Vector3 moveInput;
    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        //Debug.Log("Entered Jump State");
        //Jump Animation
        playerStateMachine.playerRB.velocity = new Vector3(playerStateMachine.playerRB.velocity.x, playerStateMachine.jumpForce, playerStateMachine.playerRB.velocity.z);
        /* If you want the player to control in the air uncomment the code below or else leave it */
        //moveInput = new Vector3(playerStateMachine.playerInput.x * playerStateMachine.jumpMovementSpeed, playerStateMachine.playerRB.velocity.y, playerStateMachine.playerInput.y * playerStateMachine.jumpMovementSpeed);
        //playerStateMachine.playerRB.velocity = playerStateMachine.transform.TransformDirection(moveInput);
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
        //Debug.Log("Exited Jump State");
    }

    public override void CheckChangeState()
    {
        if (playerStateMachine.playerInput.magnitude == 0)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerIdleState);
        }

        else if (playerStateMachine.playerInput.magnitude != 0 && playerStateMachine.isGrounded)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerMovingState);
        }
    }

}
