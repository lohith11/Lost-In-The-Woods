using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    private Vector3 moveInput;
    public PlayerCrouchState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        //Crouch animation
        Debug.Log("Entered Crouched State");
        playerStateMachine.transform.localScale = playerStateMachine.crouchScale;
        playerStateMachine.transform.position = new Vector3(playerStateMachine.transform.position.x, playerStateMachine.transform.position.y - 0.3f, playerStateMachine.transform.position.z);
    }

    public override void UpdateState()
    {
        CheckChangeState();
    }

    public override void FixedUpdateState()
    {
        moveInput = new Vector3(playerStateMachine.playerInput.x * playerStateMachine.playerCrouchSpeed, playerStateMachine.playerRB.velocity.y, playerStateMachine.playerInput.y * playerStateMachine.playerCrouchSpeed);
        playerStateMachine.playerRB.velocity = playerStateMachine.transform.TransformDirection(moveInput);
    }

    public override void ExitState()
    {
        playerStateMachine.transform.localScale = playerStateMachine.playerScale;
        playerStateMachine.transform.position = new Vector3(playerStateMachine.transform.position.x, playerStateMachine.transform.position.y + 0.3f, playerStateMachine.transform.position.z);

        Debug.Log("Exited Crouched State");
    }

    public override void CheckChangeState()
    {
        if (!playerStateMachine.isCrouched)
        {
            if(playerStateMachine.playerInput.magnitude == 0)
            {
                playerStateMachine.SwitchState(playerStateMachine.playerIdleState);
            }

            else if(playerStateMachine.playerInput.magnitude != 0)
            {
                playerStateMachine.SwitchState(playerStateMachine.playerMovingState);
            }
        }
    }
}
