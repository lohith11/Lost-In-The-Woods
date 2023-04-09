using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerCrouchState : PlayerBaseState
{
    private Vector3 moveInput;
    private int CrouchMoveX;
    private int CrouchMoveY;
    private float currentPosition;

    public PlayerCrouchState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        CrouchMoveX = Animator.StringToHash("CrouchMoveX");
        CrouchMoveY = Animator.StringToHash("CrouchMoveY");
        playerStateMachine.playerAnimation.CrossFade("Player_Crouch", 0.05f);
        currentPosition = playerStateMachine.playerCamera.localPosition.y;
        playerStateMachine.GetComponent<CapsuleCollider>().height = 0.9f;
        playerStateMachine.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0.45f, 0f);
    }

    public override void UpdateState()
    {
        if (Mathf.Abs(playerStateMachine.crouchHeight - currentPosition) > 0.05f)
        {
            currentPosition = Mathf.Lerp(currentPosition, playerStateMachine.crouchHeight, 0.1f);
            playerStateMachine.playerCamera.localPosition = new Vector3(0, currentPosition, 0.5f);
            playerStateMachine.originalPosition = currentPosition;
        }
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
            playerStateMachine.playerAnimation.CrossFade("Player_Crouch", 0.05f);
        }
        moveInput = new Vector3(playerStateMachine.playerInput.x, 0f, playerStateMachine.playerInput.y) * playerStateMachine.playerCrouchSpeed * Time.fixedDeltaTime;
        playerStateMachine.transform.Translate(moveInput);
    }

    public override void ExitState()
    {
        playerStateMachine.GetComponent<CapsuleCollider>().height = 1.8f;
        playerStateMachine.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0.9f, 0f);
    }

    public override void CheckChangeState()
    {
        if (!playerStateMachine.crouchPressed)
        {
            if (playerStateMachine.playerInput.magnitude == 0)
            {
                playerStateMachine.SwitchState(playerStateMachine.playerIdleState);
            }
        }
    }
}
