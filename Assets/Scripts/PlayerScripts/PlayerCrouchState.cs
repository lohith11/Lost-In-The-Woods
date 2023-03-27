using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    private Vector3 moveInput;
    public PlayerCrouchState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        playerStateMachine.playerAnimation.CrossFade("Player_Crouch", 0.1f);
        //Debug.Log("Entered Crouched State");
        playerStateMachine.originalPosition = 1f;
        playerStateMachine.playerCamera.localPosition = new Vector3(0, 1f, 0.5f);
        playerStateMachine.GetComponent<CapsuleCollider>().height = 0.9f;
        playerStateMachine.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0.45f, 0f);
    }

    public override void UpdateState()
    {
        CheckChangeState();

        if(playerStateMachine.playerInput.y == 1)
        {
            playerStateMachine.playerAnimation.Play("Player_CrouchWalkFarword");
        }

        if(playerStateMachine.playerInput.y == -1)
        {
            playerStateMachine.playerAnimation.Play("Player_CrouchWalkBack");
        }    

        if(playerStateMachine.playerInput.x == 1)
        {
            playerStateMachine.playerAnimation.Play("Player_CrouchWalkRight");
        }

        if(playerStateMachine.playerInput.x == -1)
        {
            playerStateMachine.playerAnimation.Play("Player_CrouchWalkLeft");
        }

        if(playerStateMachine.playerInput.magnitude == 0)
        {
            playerStateMachine.playerAnimation.Play("Player_Crouch");
        }
    }

    public override void FixedUpdateState()
    {
        moveInput = new Vector3(playerStateMachine.playerInput.x * playerStateMachine.playerCrouchSpeed, playerStateMachine.playerRB.velocity.y, playerStateMachine.playerInput.y * playerStateMachine.playerCrouchSpeed);
        playerStateMachine.playerRB.velocity = playerStateMachine.transform.TransformDirection(moveInput);
    }

    public override void ExitState()
    {
        playerStateMachine.originalPosition = 1.7f;
        playerStateMachine.playerCamera.localPosition = new Vector3(0, 1.7f, 0.2f);
        playerStateMachine.GetComponent<CapsuleCollider>().height = 1.8f;
        playerStateMachine.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0.9f, 0f);
        //Debug.Log("Exited Crouched State");
    }

    public override void CheckChangeState()
    {
        if (!playerStateMachine.isCrouched)
        {
            if (playerStateMachine.playerInput.magnitude == 0)
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
