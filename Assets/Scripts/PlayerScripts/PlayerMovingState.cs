using UnityEngine;

public class PlayerMovingState : PlayerBaseState
{
    private Vector3 moveInput;
    private float currentPosition;
    private int moveX;
    private int moveY;

    public PlayerMovingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void EnterState()
    {
        moveX = Animator.StringToHash("MoveX");
        moveY = Animator.StringToHash("MoveY");
        playerStateMachine.playerAnimation.SetBool("isMoving", true);
        currentPosition = playerStateMachine.playerCamera.transform.localPosition.y;
    }
    public override void UpdateState()
    {
        CheckChangeState();
        if (Mathf.Abs(playerStateMachine.standingHeight - currentPosition) > 0.05f)
        {
            currentPosition = Mathf.Lerp(currentPosition, playerStateMachine.standingHeight, 0.1f);
            playerStateMachine.playerCamera.localPosition = new Vector3(0, currentPosition, 0.3f);
            playerStateMachine.originalPosition = currentPosition;
        }
        playerStateMachine.playerAnimation.SetFloat(moveX, playerStateMachine.playerInput.x);
        playerStateMachine.playerAnimation.SetFloat(moveY, playerStateMachine.playerInput.y);
    }

    public override void FixedUpdateState()
    {
        moveInput = playerStateMachine.transform.right * playerStateMachine.playerInput.x + playerStateMachine.transform.forward * playerStateMachine.playerInput.y;
        moveInput = moveInput * (Time.fixedDeltaTime * playerStateMachine.playerSpeed);
        playerStateMachine.playerRB.MovePosition(playerStateMachine.transform.position + moveInput);
        //moveInput = new Vector3(-playerStateMachine.playerInput.x * playerStateMachine.playerSpeed * Time.fixedDeltaTime, playerStateMachine.playerRB.velocity.y, -playerStateMachine.playerInput.y * playerStateMachine.playerSpeed * Time.fixedDeltaTime);
        //playerStateMachine.transform.Translate(moveInput);
        //playerStateMachine.playerRB.velocity = playerStateMachine.transform.TransformDirection(moveInput);
    }
    public override void ExitState() 
    {
        playerStateMachine.playerAnimation.SetBool("isMoving", false);
    }
    public override void CheckChangeState()
    {
        if(playerStateMachine.playerInput.magnitude == 0)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerIdleState);
        }

        else if(playerStateMachine.isRunning && playerStateMachine.playerInput.y == 1 && playerStateMachine.stamina > 50)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerRunningState);
        }

        else if (playerStateMachine.crouchPressed)
        {
            playerStateMachine.SwitchState(playerStateMachine.playerCrouchState);
        }

        //else if(playerStateMachine.isJumping && playerStateMachine.isGrounded)
        //{
        //    playerStateMachine.SwitchState(playerStateMachine.playerJumpState);
        //}
    }
}
