using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    private float currentPosition;
    private float heartbeatvolume;
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        if(playerStateMachine.stamina < 20)
        {
            playerStateMachine.audioSource.PlayOneShot(playerStateMachine.breath);
        }
        currentPosition = playerStateMachine.playerCamera.transform.localPosition.y;
        playerStateMachine.playerAnimation.CrossFade("Player_Idle", 0.1f);
    }

    public override void UpdateState()
    {
        RunningStamina();
        if (Mathf.Abs(playerStateMachine.standingHeight - currentPosition) > 0.05f)
        {
            currentPosition = Mathf.Lerp(currentPosition, playerStateMachine.standingHeight, 0.1f);
            playerStateMachine.playerCamera.localPosition = new Vector3(0, currentPosition, 0.2f);
            playerStateMachine.originalPosition = currentPosition; 
        }
        CheckChangeState();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState() 
    {
        
    }

    public void RunningStamina()
    {
        playerStateMachine.stamina += playerStateMachine.staminaDepletionRate * Time.deltaTime;

        heartbeatvolume = 1f - (playerStateMachine.stamina / 100f);
        playerStateMachine.heartBeat.GetComponent<AudioSource>().volume = heartbeatvolume;

        playerStateMachine.stamina = Mathf.Clamp(playerStateMachine.stamina, 0f, 100f);
        playerStateMachine.staminaBar.fillAmount = playerStateMachine.stamina / 100f;
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