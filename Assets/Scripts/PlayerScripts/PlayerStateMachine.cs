using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class PlayerStateMachine : MonoBehaviour
{

    public static event EventHandler hidePlayer;
    //IsGrass and isCrouching enemy won't detect
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
    public static Vector3 playerCurrentPosition;

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
    public float stamina;
    public float staminaDepletionRate;
    public float staminaRegenerationRate;
    public Image staminaBar;
    [Space(10)]

    //Player Crouch
    [Header("< Player Crouch >")]
    [Space(5)]
    public float playerCrouchSpeed;
    public bool crouchPressed;
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

    //Player Prefs
    [Header("< Player Prefs >")]
    [Space(5)]
    public int herbs;
    public TMP_Text forPickingHerb;
    public bool canPickHerb = false;
    [Space(10)]

    //Camera Smoothness
    [Space(5)]
    [Header("< SmoothCamera Transition >")]
    public float standingHeight;
    public float crouchHeight;
    [Space(10)]

    //PlayerSteping on Obstacles
    [Space(5)]
    [Header("< PlayerStep Height >")]
    public GameObject rayCastUp;
    public GameObject rayCastDown;
    public float stepHeight;
    public float smoothStep;
    [Space(10)]

    [Space(5)]
    [Header("< FootSteps Sounds >")]
    public float baseStepSpeed;
    public float crouchStepSpeed;
    public float sprintStepSpeed;
    public AudioSource audioSource = default;
    private TerrainDetector terrainDetector;
    public AudioClip[] woodSound;
    public AudioClip[] grassSound;
    public AudioClip[] dirtSound;
    public AudioClip[] mudSound;
    public AudioClip[] leavesSound;
    private float footStepTimer;
    [Space(10)]

    [Space(5)]
    [Header(" < PlayerDodge > ")]
    [SerializeField] private float dodgeDistance;
    [SerializeField] private float dodgeDuration;
    [SerializeField] private float dodgeCooldown;
    [SerializeField] private float dodgeSpeed;

    private bool canDodge = true;
    [Space(10)]

    public bool isAtttacking;
    public bool isAiming;
    public bool isPicking;
    public float FAVdelay;
    private PlayerBaseState currentState;
    public PlayerControls playerControls;

    public GameObject crouchCollider;
    private GameObject herbInRange;
    private GameObject keyInRange;

    [HideInInspector] public ThrowingRocks throwingRocks;
    private float getCurrentOffset => currentState == playerRunningState ? baseStepSpeed * sprintStepSpeed : crouchPressed ? baseStepSpeed * crouchStepSpeed : baseStepSpeed;

    private bool canPickKey = false;
    private int keyPicked = 0;
    private MoveRuller moveRuller;
    private bool inGrass;
    public bool isBarrel;
    private Coroutine barrelCoroutine;

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerIdleState = new PlayerIdleState(this);
        playerMovingState = new PlayerMovingState(this);
        playerJumpState = new PlayerJumpState(this);
        playerRunningState = new PlayerRunningState(this);
        playerCrouchState = new PlayerCrouchState(this);
    }

    public void Start()
    {
        staminaBar.enabled = false;
        terrainDetector = new TerrainDetector();
        originalPosition = playerCamera.transform.localPosition.y;
        rayCastUp.transform.position = new Vector3(rayCastUp.transform.position.x, stepHeight, rayCastUp.transform.position.z);
        throwingRocks=GetComponent<ThrowingRocks>();
        mouseLook = FindObjectOfType<MouseLook>();
        playerRB = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<Animator>();

        SwitchState(playerIdleState);
    }

    #region Enabling and Disabling Input Controls
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

        playerControls.Player.Picking.performed += PickingRock;
        playerControls.Player.Picking.performed += HerbsPickUp;
        playerControls.Player.Picking.performed += KeyPickUp;

        playerControls.Player.Picking.performed += BarrelIgnite;
        playerControls.Player.Picking.canceled += BarrelIgnite;

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

        playerControls.Player.Picking.performed -= PickingRock;
        playerControls.Player.Picking.performed -= HerbsPickUp;
        playerControls.Player.Picking.performed -= KeyPickUp;

        playerControls.Player.Picking.performed -= BarrelIgnite;
        playerControls.Player.Picking.canceled -= BarrelIgnite;

        playerControls.Player.Projectile.started -= ProjectileRock;
        playerControls.Player.Projectile.performed -= ProjectileRock;
        playerControls.Player.Projectile.canceled -= ProjectileRock;

        playerControls.Player.Attack.started -= Attacking;
        playerControls.Player.Attack.performed -= Attacking;
        playerControls.Player.Attack.canceled -= Attacking;
    }
    #endregion

    public void Update()
    {
        if(currentState == playerRunningState)
        {
            staminaBar.enabled = true;
            stamina -= staminaDepletionRate * Time.deltaTime;
        }
        else
        {
            stamina += staminaRegenerationRate * Time.deltaTime;
            staminaBar.enabled = false;
        }
        stamina = Mathf.Clamp(stamina, 0f, 100f);
        staminaBar.fillAmount = stamina/100f;

        CameraShake();
        currentState.UpdateState();
        Step();

        isGrounded = Physics.CheckSphere(groundPosition.position, groundRadius, groundLayer);

        playerCurrentPosition = this.transform.position;
    }

    public void FixedUpdate()
    {
        if (canDodge && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DoDodge());
        }
        PlayerSteppingUp();
        currentState.FixedUpdateState();
    }

    #region PlayerSteppingAudio
    public void Step()
    {
        if (playerInput.magnitude == 0)
        {
            return;
        }
        footStepTimer -= Time.deltaTime;
        if (footStepTimer <= 0)
        {
            AudioClip clip = GetRandomClip();
            audioSource.PlayOneShot(clip);
            footStepTimer = getCurrentOffset;
        }
    }

    private AudioClip GetRandomClip()
    {
        int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);
        switch (terrainTextureIndex)
        {
            case 0:
                return dirtSound[Random.Range(0, dirtSound.Length - 1)];
            case 1:
                return mudSound[Random.Range(0, mudSound.Length - 1)];
            case 2:
                return grassSound[Random.Range(0, grassSound.Length - 1)];
            case 3:
                return leavesSound[Random.Range(0, leavesSound.Length - 1)];
            /*case 4:
                return woodSound[Random.Range(0, woodSound.Length - 1)];*/
            default:
                return dirtSound[Random.Range(0, dirtSound.Length - 1)];
        }
    }
    #endregion

    #region animationIK
    /*  private void OnAnimatorIK(int layerIndex)
    {
        if(playerAnimation)
        {
            playerAnimation.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            playerAnimation.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f); 
            playerAnimation.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            playerAnimation.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

            //Left Leg
            RaycastHit hit;
            Ray ray = new Ray(playerAnimation.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down); 
            if(Physics.Raycast(ray, out hit, groucdDistance + 1f, layerMask))
            {
                if(hit.transform.tag=="Ground")
                {
                    Vector3 footposition = hit.point;
                    footposition.y += groucdDistance;
                    playerAnimation.SetIKPosition(AvatarIKGoal.LeftFoot, footposition);
                    playerAnimation.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }
            }

            //Right Leg
            ray = new Ray(playerAnimation.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, groucdDistance + 1f, layerMask))
            {
                if (hit.transform.tag == "Ground")
                {
                    Vector3 footposition = hit.point;
                    footposition.y += groucdDistance;
                    playerAnimation.SetIKPosition(AvatarIKGoal.RightFoot, footposition);
                    playerAnimation.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }
            }
        }
    }*/
    #endregion

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
        if (context.performed)
        {
            crouchPressed = !crouchPressed;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //isJumping = context.ReadValueAsButton();
    }

    public void Rotation(InputAction.CallbackContext context)
    {
        playerRotation = context.ReadValue<Vector2>();
    }

    public void PickingRock(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            GetComponent<ThrowingRocks>().RockPicking();
        }
    }

    public void HerbsPickUp(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            HerbsPicking();
        }
    }

    public void KeyPickUp(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            KeyPickUp();
        }
    }

    public void BarrelIgnite(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            barrelCoroutine = StartCoroutine(barrel());
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            StopCoroutine(barrelCoroutine);
        }
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

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Herbs"))
        {
            forPickingHerb.enabled = true;
            canPickHerb = true;
            forPickingHerb.text = "Press E or Controller Y";
            herbInRange = other.gameObject;
        }

        if (other.CompareTag("Key"))
        {
            forPickingHerb.enabled = true;
            canPickKey = true;
            forPickingHerb.text = "Press E or Cotroller Y";
            keyInRange = other.gameObject;
        }

        if (other.gameObject.CompareTag("Grass") && crouchPressed)
        {
            Debug.Log("Entered");
            inGrass = true;
            GrassStay();
        }

        if(other.gameObject.CompareTag("Barrel"))
        {
            forPickingHerb.enabled = true;
            isBarrel = true;
            forPickingHerb.text = "Hold E or Y";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Herbs"))
        {
            forPickingHerb.enabled = false;
            canPickHerb = false;
            herbInRange = null;
        }

        if (other.CompareTag("Key"))
        {
            forPickingHerb.enabled = false;
            canPickKey = false;
            keyInRange = null;
        }

        if (other.gameObject.CompareTag("Grass") && crouchPressed)
        {
            Debug.Log("Exited");
            inGrass = false;
        }

        if (other.gameObject.CompareTag("Barrel"))
        {
            forPickingHerb.enabled = false;
            isBarrel = false;
        }
    }
    #endregion

    #region IEnumerators
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
    }
    #endregion

    #region Lock Dpad
    public void EnterLockRegion(MoveRuller MR)
    {
        moveRuller = MR;
        playerControls.Player.DpadUp.performed += DpadUP;
        playerControls.Player.DpadDown.performed += DpadDOWN;
        playerControls.Player.DpadRight.performed += DpadRIGHT;
        playerControls.Player.DpadLeft.performed += DpadLEFT;
    }

    public void ExitLockRegion(MoveRuller MR)
    {
        moveRuller = null;
        playerControls.Player.DpadUp.performed -= DpadUP;
        playerControls.Player.DpadDown.performed -= DpadDOWN;
        playerControls.Player.DpadRight.performed -= DpadRIGHT;
        playerControls.Player.DpadLeft.performed -= DpadLEFT;
    }

    public void DpadUP(InputAction.CallbackContext context)
    {
        moveRuller?.RotateRullersUp();
    }

    public void DpadDOWN(InputAction.CallbackContext context)
    {
        moveRuller?.RotateRullerDown();
    }

    public void DpadRIGHT(InputAction.CallbackContext context)
    {
        moveRuller?.MoveRullerRight();
    }

    public void DpadLEFT(InputAction.CallbackContext context)
    {
        moveRuller?.MoveRullerLeft();
    }
    #endregion

    #region Interactables 
    public void GrassStay()
    {
        if(inGrass)
        {
            hidePlayer?.Invoke(this, EventArgs.Empty);
        }
    }

    public void KeyPickUp()
    {
        if (canPickKey)
        {
            keyPicked++;
            forPickingHerb.enabled = false;
            Destroy(keyInRange);
            keyInRange = null;
        }
    }

    public void HerbsPicking()
    {
        if (canPickHerb)
        {
            herbs++;
            forPickingHerb.enabled = false;
            Destroy(herbInRange);
            herbInRange = null;
        }
    }

    public void BarrelSwitchingOn()
    {
        if(isBarrel)
        {
            //Get the barrel to blast after 5 sec
        }
    }

    public IEnumerator barrel()
    {
        Debug.Log("Couritne entered");
        yield return new WaitForSeconds(3f);

        BarrelSwitchingOn();
        Debug.Log("Couritien finished");
    }
    #endregion

    public void PlayerSteppingUp()
    {
        //RaycastHit hitLower;
        //if(Physics.Raycast(rayCastDown.transform.position, transform.TransformDirection(Vector3.forward),out hitLower, 0.1f))
        //{
        //    RaycastHit hitUpper;
        //    if(!Physics.Raycast(rayCastUp.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
        //    {
        //        playerRB.position -= new Vector3(0f, -smoothStep, 0f);
        //    }
        //}

        //RaycastHit hitLower45Degrees;
        //if(Physics.Raycast(rayCastDown.transform.position, transform.TransformDirection(1.5f, 0, 1),out hitLower45Degrees, 0.1f))
        //{
        //    RaycastHit hitUpper45Degrees;
        //    if(!Physics.Raycast(rayCastUp.transform.position, transform.TransformDirection(1.5f, 0, 1f),out hitUpper45Degrees, 0.2f))
        //    {
        //        playerRB.position -= new Vector3(0f, -smoothStep, 0f);
        //    }
        //}

        //RaycastHit hitLowerMinusDegrees;
        //if (Physics.Raycast(rayCastDown.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinusDegrees, 0.1f))
        //{
        //    RaycastHit hitUpperMinusDegrees;
        //    if (!Physics.Raycast(rayCastUp.transform.position, transform.TransformDirection(-1.5f, 0, 1f), out hitUpperMinusDegrees, 0.2f))
        //    {
        //        playerRB.position -= new Vector3(0f, -smoothStep, 0f);
        //    }
        //}
    }

    public void CameraShake()
    {
        if (Mathf.Abs(playerInput.x) > 0.1f || Mathf.Abs(playerInput.y) > 0.1f)
        {
            timer += Time.deltaTime * (crouchPressed ? croucSpeed : currentState == playerRunningState ? sprintSpeed : walkSpeed);
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, originalPosition + Mathf.Sin(timer) * (crouchPressed ? croucSpeedAmount : isRunning && playerInput.y == 1 ? sprintSpeedAmount : walkSpeedAmount), playerCamera.transform.localPosition.z);
        }
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState?.ExitState();
        currentState = state;
        currentState.EnterState();
    }

    private IEnumerator DoDodge()
    {
        canDodge = false;
        float startTime = Time.time;

        Vector3 startPosition = transform.position;
        Vector3 dodgeDirection = (transform.right * playerInput.x) * dodgeSpeed + (transform.forward * playerInput.y) * dodgeSpeed;
        Vector3 dodgeTarget = transform.position + dodgeDirection * dodgeDistance;

        while (Time.time < startTime + dodgeDuration)
        {
            float t = (Time.time - startTime) / dodgeDuration;
            transform.position = Vector3.Lerp(startPosition, dodgeTarget, t);
            yield return null;
        }

        transform.position = dodgeTarget;
        yield return new WaitForSeconds(dodgeCooldown);

        canDodge = true;
    }
}