using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningState : PlayerBaseState
{
    private Vector3 moveInput;
    private float heartbeatvolume;
    public PlayerRunningState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        playerStateMachine.playerAnimation.Play("Player_Running");
        playerStateMachine.CorStarter(playerStateMachine.FOV, playerStateMachine.FAVdelay);
        playerStateMachine.staminaBar.enabled = true;
        playerStateMachine.heartBeat.GetComponent<AudioSource>().Play();
    }

    public override void UpdateState()
    {
        RunningStamina();
        CheckChangeState();
    }

    public override void FixedUpdateState()
    {
        moveInput = playerStateMachine.transform.right * playerStateMachine.playerInput.x + playerStateMachine.transform.forward * playerStateMachine.playerInput.y;
        moveInput = moveInput * (Time.fixedDeltaTime * playerStateMachine.playerRunSpeed);
        playerStateMachine.playerRB.MovePosition(playerStateMachine.transform.position + moveInput);
    }

    public override void ExitState()
    {
        playerStateMachine.CorStarter(60f, playerStateMachine.FAVdelay);
        playerStateMachine.staminaBar.enabled = false;
    }

    public void RunningStamina()
    {
        playerStateMachine.stamina -= playerStateMachine.staminaDepletionRate * Time.deltaTime;

        heartbeatvolume = 1f - (playerStateMachine.stamina / 80f);
        playerStateMachine.heartBeat.GetComponent<AudioSource>().volume = heartbeatvolume;

        playerStateMachine.stamina = Mathf.Clamp(playerStateMachine.stamina, 0f, 100f);
        playerStateMachine.staminaBar.fillAmount = playerStateMachine.stamina / 100f;
    }

    public override void CheckChangeState()
    {
        if(!playerStateMachine.isRunning || playerStateMachine.stamina <= 0 || playerStateMachine.playerInput.magnitude == 0) 
        {
            if(playerStateMachine.playerInput.magnitude != 0)
            {
                playerStateMachine.SwitchState(playerStateMachine.playerMovingState);
            }

            else if(playerStateMachine.playerInput.magnitude == 0)
            {
                playerStateMachine.SwitchState(playerStateMachine.playerIdleState);
            }
        }
        
    }
}
