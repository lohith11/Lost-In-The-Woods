using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDodgeState : PlayerBaseState
{
    private bool isDodged;
    public PlayerDodgeState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        isDodged = true;
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

        Vector3 startPosition = playerStateMachine.transform.position;
        Vector3 dodgeDirection = (playerStateMachine.transform.right * playerStateMachine.playerInput.x) * playerStateMachine.dodgeSpeed + (playerStateMachine.transform.forward * playerStateMachine.playerInput.y) * playerStateMachine.dodgeSpeed;
        Vector3 dodgeTarget = playerStateMachine.transform.position + dodgeDirection * playerStateMachine.dodgeDistance;

        while (Time.time < startTime + playerStateMachine.dodgeDuration)
        {
            float t = (Time.time - startTime) / playerStateMachine.dodgeDuration;
            playerStateMachine.transform.position = Vector3.Lerp(startPosition, dodgeTarget, t);
            yield return null;
        }

        playerStateMachine.transform.position = dodgeTarget;
        CheckChangeState();
        yield return new WaitForSeconds(playerStateMachine.dodgeCooldown);
        playerStateMachine.canDodge = true;

    }
}
