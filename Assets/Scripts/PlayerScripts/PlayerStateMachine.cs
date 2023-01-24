using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    //HideInInspector because need to use them in the other states but not be seen in the inspector  
    [HideInInspector] public Rigidbody playerRB;
    [HideInInspector] public PlayerMovingState playerMovingState;
    [HideInInspector] public PlayerIdleState playerIdleState;
    
    //Player Movement
    public float playerSpeed;
    public Vector2 playerInput;

    private PlayerBaseState currentState;
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls= new PlayerControls();
        
        playerIdleState = new PlayerIdleState(this);
        playerMovingState = new PlayerMovingState(this);
    }
    public void Start()
    {
        playerRB = GetComponent<Rigidbody>();

        SwitchState(playerIdleState);
    }
    private void OnEnable()
    {
        playerControls.Enable();

        playerControls.Player.Move.started += Moving;
        playerControls.Player.Move.performed += Moving;
        playerControls.Player.Move.canceled += Moving;
    }

    private void OnDisable()
    {
        playerControls.Disable();

        playerControls.Player.Move.started -= Moving;
        playerControls.Player.Move.performed -= Moving;
        playerControls.Player.Move.canceled -= Moving;
    }

    // Update is called once per frame
    public  void Update()
    {
        currentState.UpdateState();
    }

    public void Moving(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();
    }

    public void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState?.ExitState();
        currentState = state;
        currentState.EnterState();
    }
}
