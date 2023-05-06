using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    private Vector3 moveInput;
    private int CrouchMoveX;
    private int CrouchMoveY;
    private float currentPosition;

    public PlayerCrouchState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        playerStateMachine.audioSource.PlayOneShot(playerStateMachine.crouchingAudio);
        CrouchMoveX = Animator.StringToHash("CrouchMoveX");
        CrouchMoveY = Animator.StringToHash("CrouchMoveY");
        playerStateMachine.playerAnimation.CrossFade("Player_Crouch", 0.05f);
        playerStateMachine.throwingRocks.attackpoint.localPosition = new Vector3(0.27f, 0.8f, 0.27f);
        currentPosition = playerStateMachine.playerCamera.transform.localPosition.y;
        playerStateMachine.crouchCollider.SetActive(false);
    }

    public override void UpdateState()
    {
        CheckChangeState();
        if (Mathf.Abs(playerStateMachine.crouchHeight - currentPosition) > 0.05f)
        {
            currentPosition = Mathf.Lerp(currentPosition, playerStateMachine.crouchHeight, 0.1f);
            playerStateMachine.playerCamera.localPosition = new Vector3(0, currentPosition, 0f);
            playerStateMachine.originalPosition = currentPosition;
        }
    }

    public override void FixedUpdateState()
    {
        if (playerStateMachine.playerInput.magnitude != 0)
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
        moveInput = playerStateMachine.transform.right * playerStateMachine.playerInput.x + playerStateMachine.transform.forward * playerStateMachine.playerInput.y;
        moveInput = moveInput * (Time.fixedDeltaTime * playerStateMachine.playerCrouchSpeed);
        playerStateMachine.playerRB.MovePosition(playerStateMachine.transform.position + moveInput);
    }

    public override void ExitState()
    {
        playerStateMachine.crouchCollider.SetActive(true);
        playerStateMachine.throwingRocks.attackpoint.localPosition = new Vector3(0.27f, 1.6f, 0.27f);
        //playerStateMachine.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0.9f, 0f);
        //playerStateMachine.GetComponent<CapsuleCollider>().height = 1.85f;
    }

    public override void CheckChangeState()
    {
        if (!playerStateMachine.crouchPressed)
        {
            if (playerStateMachine.playerInput.magnitude == 0)
            {
                playerStateMachine.audioSource.PlayOneShot(playerStateMachine.standiingAudio);
                playerStateMachine.SwitchState(playerStateMachine.playerIdleState);
            }

            else if (playerStateMachine.playerInput.magnitude != 0)
            {
                playerStateMachine.audioSource.PlayOneShot(playerStateMachine.standiingAudio);
                playerStateMachine.SwitchState(playerStateMachine.playerMovingState);
            }
        }
    }

}
