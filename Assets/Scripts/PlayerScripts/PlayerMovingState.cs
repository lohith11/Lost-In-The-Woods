using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovingState : PlayerBaseState
{
    private Vector3 moveInput;
    public PlayerMovingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        //Moving Animation
    }
    public override void UpdateState()
    {
        CheckChangeState();
    }

    public override void FixedUpdateState()
    {
        moveInput = new Vector3(playerStateMachine.playerInput.x, playerStateMachine.playerRB.velocity.y, playerStateMachine.playerInput.y);
        playerStateMachine.playerRB.velocity = playerStateMachine.transform.TransformDirection(moveInput * playerStateMachine.playerSpeed);
    }
    public override void ExitState() 
    {
        Debug.Log("Exited MovingState");
    }
    public override void CheckChangeState()
    {
        if(playerStateMachine.playerInput.magnitude == 0)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerIdleState);
        }
    }
}
