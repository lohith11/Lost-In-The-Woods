using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    //HideInInspector because need to use them in the other states but not be seen in the inspector  
    [HideInInspector] public Rigidbody playerRB;
    [HideInInspector] public PlayerMovingState playerMovingState;
    [HideInInspector] public PlayerIdleState playerIdleState;
    [HideInInspector] public PlayerRunningState playerRunningState;
    [HideInInspector] public PlayerCrouchState playerCrouchState;
    [HideInInspector] public PlayerJumpState playerJumpState;
    [HideInInspector] public MouseLook mouseLook;
    [HideInInspector] public Animator playerAnimation;
    [HideInInspector] public Coroutine cor;

    //Player Walking
    [Header("< Player Walking >")]
    [Space(5)]
    public float playerSpeed;
    public Vector2 playerInput;
    public Vector2 playerRotation;
    [Space(10)]

    //Player Running
    [Header("< Player Running >")]
    [Space(5)]
    public float playerRunSpeed;
    public bool isRunning;
    [Space(10)]

    //Player Crouch
    [Header("< Player Crouch >")]
    [Space(5)]
    public float playerCrouchSpeed;
    public bool isCrouched;
    [Space(10)]

    //Player Jump
    [Header("< Player Jump >")]
    [Space(5)]
    public Transform groundPosition;
    public LayerMask groundLayer;
    public float jumpForce;
    public float groundRadius;
    public float jumpMovementSpeed;
    public bool isJumping;
    public bool isGrounded = true;
    [Space(10)]

    //PlayerCamera Shakes
    [Header("< PlayerCamera Shake >")]
    [Space(5)]
    public Transform playerCamera;
    private float timer;
    public float originalPosition;
    public float walkSpeed;
    public float walkSpeedAmount;
    public float sprintSpeed;
    public float sprintSpeedAmount;
    public float croucSpeed;
    public float croucSpeedAmount;

    [Range(60, 120)]
    public float FOV;
    [Space(10)]

    public bool isAtttacking;
    public bool isAiming;
    public bool isPicking;
    public float FAVdelay; 
    private PlayerBaseState currentState;
    public PlayerControls playerControls;
    public Transform headTarget;

    private void Awake()
    {
        originalPosition = playerCamera.transform.localPosition.y;
        playerControls = new PlayerControls();
        
        playerIdleState = new PlayerIdleState(this);
        playerMovingState = new PlayerMovingState(this);
        playerJumpState = new PlayerJumpState(this);
        playerRunningState = new PlayerRunningState(this);
        playerCrouchState = new PlayerCrouchState(this);
    }

    public void Start()
    {
        mouseLook = FindObjectOfType<MouseLook>();
        playerRB = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<Animator>();

        SwitchState(playerIdleState);
    }

    private void OnEnable()
    {
        playerControls.Enable();

        playerControls.Player.Move.started += Moving;
        playerControls.Player.Move.performed += Moving;
        playerControls.Player.Move.canceled += Moving;

        playerControls.Player.MouseRotation.started += Rotation;
        playerControls.Player.MouseRotation.performed += Rotation;
        playerControls.Player.MouseRotation.canceled += Rotation;

        playerControls.Player.Run.started += Running;
        playerControls.Player.Run.performed += Running;
        playerControls.Player.Run.canceled += Running;

        playerControls.Player.Crouch.started += Crouched;
        playerControls.Player.Crouch.performed += Crouched;
        playerControls.Player.Crouch.canceled += Crouched;

        playerControls.Player.Jump.started += Jump;
        playerControls.Player.Jump.performed += Jump;
        playerControls.Player.Jump.canceled += Jump;  

        playerControls.Player.Picking.started += PickingRock;
        playerControls.Player.Picking.performed += PickingRock;
        playerControls.Player.Picking.canceled += PickingRock; 
        
        playerControls.Player.Projectile.started += ProjectileRock;
        playerControls.Player.Projectile.performed += ProjectileRock;
        playerControls.Player.Projectile.canceled += ProjectileRock;

        playerControls.Player.Attack.started += Attacking;
        playerControls.Player.Attack.performed += Attacking;
        playerControls.Player.Attack.canceled += Attacking;  
        
    }

    private void OnDisable()
    {
        playerControls.Disable();

        playerControls.Player.Move.started -= Moving;
        playerControls.Player.Move.performed -= Moving;
        playerControls.Player.Move.canceled -= Moving;

        playerControls.Player.MouseRotation.started -= Rotation;
        playerControls.Player.MouseRotation.performed -= Rotation;
        playerControls.Player.MouseRotation.canceled -= Rotation;

        playerControls.Player.Run.started -= Running;
        playerControls.Player.Run.performed -= Running;
        playerControls.Player.Run.canceled -= Running;

        playerControls.Player.Crouch.started -= Crouched;
        playerControls.Player.Crouch.performed -= Crouched;
        playerControls.Player.Crouch.canceled -= Crouched;

        playerControls.Player.Jump.started -= Jump;
        playerControls.Player.Jump.performed -= Jump;
        playerControls.Player.Jump.canceled -= Jump;

        playerControls.Player.Picking.started -= PickingRock;
        playerControls.Player.Picking.performed -= PickingRock;
        playerControls.Player.Picking.canceled -= PickingRock;
        
        playerControls.Player.Projectile.started -= ProjectileRock;
        playerControls.Player.Projectile.performed -= ProjectileRock;
        playerControls.Player.Projectile.canceled -= ProjectileRock;

        playerControls.Player.Attack.started -= Attacking;
        playerControls.Player.Attack.performed -= Attacking;
        playerControls.Player.Attack.canceled -= Attacking;
    }

    public void Update()
    {
        CameraShake();
        currentState.UpdateState();

        isGrounded = Physics.CheckSphere(groundPosition.position, groundRadius, groundLayer);
    }

    public void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }

    #region Player Input Controls
    public void Moving(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();
    }

    public void Running(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    public void Crouched(InputAction.CallbackContext context)
    {
        isCrouched = context.ReadValueAsButton();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        isJumping = context.ReadValueAsButton();
    }

    public void Rotation(InputAction.CallbackContext context)
    {
        playerRotation = context.ReadValue<Vector2>();
    }

    public void PickingRock(InputAction.CallbackContext context)
    {
        isPicking = context.ReadValueAsButton();
    }

    public void ProjectileRock(InputAction.CallbackContext context)
    {
        isAiming = context.ReadValueAsButton();
    }

    public void Attacking(InputAction.CallbackContext context)
    {
        isAtttacking = context.ReadValueAsButton();
    }
    #endregion

    public void CameraShake()
    {
        if (!isGrounded)
        {
            return;
        }

        else if (Mathf.Abs(playerInput.x) > 0.1f || Mathf.Abs(playerInput.y) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouched ? croucSpeed : isRunning ? sprintSpeed : walkSpeed);
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, originalPosition + Mathf.Sin(timer) * (isCrouched ? croucSpeedAmount : isRunning ? sprintSpeedAmount : walkSpeedAmount), playerCamera.transform.localPosition.z);
        }
    }

    public void CorStarter(float target, float delay)
    {
        if (cor != null)
        {
            StopCoroutine(cor);
            cor = null;
        }

        cor = StartCoroutine(FOVLerper(target, delay));
    }

    public IEnumerator FOVLerper(float target, float delay)
    {
        float timer = 0f;
        float start = Camera.main.fieldOfView;
        while (timer <= delay)
        {
            timer += Time.deltaTime;
            Camera.main.fieldOfView = Mathf.Lerp(start, target, timer / delay);
            yield return null;
        }

        StopCoroutine(cor);
        cor = null;
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState?.ExitState();
        currentState = state;
        currentState.EnterState();
    }


}