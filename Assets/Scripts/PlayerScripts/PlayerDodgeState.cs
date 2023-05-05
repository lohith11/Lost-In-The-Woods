using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDodgeState : PlayerBaseState
{
    public PlayerDodgeState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        playerStateMachine.StartCoroutine(DoDodge());
    }

    public override void UpdateState()
    {
        
    }
    public override void FixedUpdateState()
    {
       
    }
    public override void ExitState()
    {
        
    }
    public override void CheckChangeState()
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

    private IEnumerator DoDodge()
    {
        playerStateMachine.canDodge = false;
        float startTime = Time.time;

        Vector3 dodgeDirection = (playerStateMachine.transform.right * playerStateMachine.playerInput.x) + (playerStateMachine.transform.forward * playerStateMachine.playerInput.y);
        float t = 0f;
        while (Time.time < startTime + playerStateMachine.dodgeDuration)
        {
            t = (Time.time - startTime) / playerStateMachine.dodgeDuration;
            playerStateMachine.playerRB.velocity = dodgeDirection * playerStateMachine.dodgeSpeed;/*Vector3.Lerp(playerStateMachine.playerRB.velocity, dodgeDirection * playerStateMachine.dodgeSpeed, t);*/
            yield return null;
        }

        CheckChangeState();
        yield return new WaitForSeconds(playerStateMachine.dodgeCooldown);
        playerStateMachine.canDodge = true;

    }
}
