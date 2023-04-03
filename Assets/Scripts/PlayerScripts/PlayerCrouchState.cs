using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    private Vector3 moveInput;
    private int CrouchMoveX;
    private int CrouchMoveY;

    public PlayerCrouchState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        CrouchMoveX = Animator.StringToHash("CrouchMoveX");
        CrouchMoveY = Animator.StringToHash("CrouchMoveY");
        playerStateMachine.playerAnimation.CrossFade("Player_Crouch", 0.1f);
        playerStateMachine.originalPosition = 1f;
        playerStateMachine.playerCamera.localPosition = new Vector3(0, 1f, 0.5f);
        playerStateMachine.GetComponent<CapsuleCollider>().height = 0.9f;
        playerStateMachine.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0.45f, 0f);
    }

    public override void UpdateState()
    {
        CheckChangeState();
    }

    public override void FixedUpdateState()
    {
        if(playerStateMachine.playerInput.magnitude != 0)
        {
            playerStateMachine.playerAnimation.SetBool("isCrouching", true);
            playerStateMachine.playerAnimation.SetFloat(CrouchMoveY, playerStateMachine.playerInput.y);
            playerStateMachine.playerAnimation.SetFloat(CrouchMoveX, playerStateMachine.playerInput.x);
        }
        else
        {
            playerStateMachine.playerAnimation.SetBool("isCrouching", false);
        }
        moveInput = new Vector3(playerStateMachine.playerInput.x * playerStateMachine.playerCrouchSpeed, playerStateMachine.playerRB.velocity.y, playerStateMachine.playerInput.y * playerStateMachine.playerCrouchSpeed);
        playerStateMachine.playerRB.velocity = playerStateMachine.transform.TransformDirection(moveInput);
    }

    public override void ExitState()
    {
        playerStateMachine.originalPosition = 1.7f;
        playerStateMachine.playerCamera.localPosition = new Vector3(0, 1.7f, 0.2f);
        playerStateMachine.GetComponent<CapsuleCollider>().height = 1.8f;
        playerStateMachine.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0.9f, 0f);
    }

    public override void CheckChangeState()
    {
        if (playerStateMachine.crouchPressed)
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
