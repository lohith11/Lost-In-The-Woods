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
    //HideInInspector because need to use them in the other states but not be seen in the inspector  
    [HideInInspector] public Rigidbody playerRB;
    [HideInInspector] public PlayerMovingState playerMovingState;
    [HideInInspector] public PlayerIdleState playerIdleState;
    [HideInInspector] public PlayerDodgeState playerDodgeState;
    [HideInInspector] public PlayerRunningState playerRunningState;
    [HideInInspector] public PlayerCrouchState playerCrouchState;
    [HideInInspector] public Animator playerAnimation;
    [HideInInspector] public Coroutine cor;
    [HideInInspector] public PlayerControls playerControls;
    [HideInInspector] public ThrowingRocks throwingRocks;

    public static Vector3 playerCurrentPosition;
    public static event EventHandler hidePlayer;

    private MouseLook mouseLookRef;
    private PlayerInput playerInputRef;

    #region Variables
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
    public float FAVdelay;

    [Range(60, 120)]
    public float FOV;
    [Space(10)]

    //Player Crouch
    [Header("< Player Crouch >")]
    [Space(5)]
    public float playerCrouchSpeed;
    public bool crouchPressed;
    public GameObject crouchCollider;
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
    [Space(10)]

    //Camera Smoothness
    [Space(5)]
    [Header("< SmoothCamera Transition >")]
    public float standingHeight;
    public float crouchHeight;
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
    private float getCurrentOffset => currentState == playerRunningState ? baseStepSpeed * sprintStepSpeed : crouchPressed ? baseStepSpeed * crouchStepSpeed : baseStepSpeed;
    [Space(10)]

    [Space(5)]
    [Header("< PlayerDodge >")]
    public float dodgeDistance;
    public float dodgeDuration;
    public float dodgeCooldown;
    public float dodgeSpeed;

    public bool isDodging;
    public bool canDodge = true;
    [Space(10)]

    [Space(5)]
    [Header("< Booleans >")]
    public bool isAtttacking;
    public bool isAiming;
    public bool isPicking;
    public bool isBarrel;
    private bool canPickKey = false;
    public bool canPickHerb = false;
    private bool inGrass;
    private int keyPicked = 0;
    public float barrelIgniteTime;
    public int herbs;
    public TMP_Text forPickingHerb;
    [Space(10)]

    private GameObject herbInRange;
    private GameObject keyInRange;

    [HideInInspector]public PlayerBaseState currentState;
    private MoveRuller moveRuller;
    private Coroutine barrelCoroutine;
    #endregion

    private void Awake()
    {
        playerControls = new PlayerControls();

        playerIdleState = new PlayerIdleState(this);
        playerMovingState = new PlayerMovingState(this);
        playerDodgeState = new PlayerDodgeState(this);
        playerRunningState = new PlayerRunningState(this);
        playerCrouchState = new PlayerCrouchState(this);

    }

    public void Start()
    {
        staminaBar.enabled = false;
        terrainDetector = new TerrainDetector();
        originalPosition = playerCamera.transform.localPosition.y;
        throwingRocks = GetComponent<ThrowingRocks>();
        playerRB = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<Animator>();

        mouseLookRef = FindObjectOfType<MouseLook>();
        playerInputRef = FindObjectOfType<PlayerInput>();
        SwitchState(playerIdleState);
    }

    #region Enabling and Disabling Input Controls
    private void OnEnable()
    {
        playerControls.Enable();

        playerControls.Player.Move.started += Moving;
        playerControls.Player.Move.performed += Moving;
        playerControls.Player.Move.canceled += Moving;

        playerControls.Player.PlayerRotation.started += Rotation;
        playerControls.Player.PlayerRotation.performed += Rotation;
        playerControls.Player.PlayerRotation.canceled += Rotation;

        playerControls.Player.Run.started += Running;
        playerControls.Player.Run.performed += Running;
        playerControls.Player.Run.canceled += Running;

        playerControls.Player.Crouch.started += Crouched;
        playerControls.Player.Crouch.performed += Crouched;
        playerControls.Player.Crouch.canceled += Crouched;

        playerControls.Player.Dodge.started += Dodge;
        playerControls.Player.Dodge.performed += Dodge;
        playerControls.Player.Dodge.canceled += Dodge;

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

        playerControls.Player.PlayerRotation.started -= Rotation;
        playerControls.Player.PlayerRotation.performed -= Rotation;
        playerControls.Player.PlayerRotation.canceled -= Rotation;

        playerControls.Player.Run.started -= Running;
        playerControls.Player.Run.performed -= Running;
        playerControls.Player.Run.canceled -= Running;

        playerControls.Player.Crouch.started -= Crouched;
        playerControls.Player.Crouch.performed -= Crouched;
        playerControls.Player.Crouch.canceled -= Crouched;

        playerControls.Player.Dodge.started -= Dodge;
        playerControls.Player.Dodge.performed -= Dodge;
        playerControls.Player.Dodge.canceled -= Dodge;

        playerControls.Player.Projectile.started -= ProjectileRock;
        playerControls.Player.Projectile.performed -= ProjectileRock;
        playerControls.Player.Projectile.canceled -= ProjectileRock;

        playerControls.Player.Attack.started -= Attacking;
        playerControls.Player.Attack.performed -= Attacking;
        playerControls.Player.Attack.canceled -= Attacking;
    }
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

    public void Dodge(InputAction.CallbackContext context)
    {
        isDodging = context.ReadValueAsButton();
    }

    public void Rotation(InputAction.CallbackContext context)
    {
        playerRotation = context.ReadValue<Vector2>();
        mouseLookRef.Rotation(playerInputRef, playerRotation);
    }

    public void PickingRock(InputAction.CallbackContext context)
    {
         GetComponent<ThrowingRocks>().RockPicking();
    }

    public void HerbsPickUp(InputAction.CallbackContext context)
    {
        HerbsPicking();
    }

    public void KeyPickUp(InputAction.CallbackContext context)
    {
         KeyPickUp();
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

    private void Update()
    {
        RunningStaminaBar();

        CameraShake();
        Step();
        currentState.UpdateState();

        playerCurrentPosition = this.transform.position;
    }

    private void FixedUpdate()
    {
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
            default:
                return dirtSound[Random.Range(0, dirtSound.Length - 1)];
        }
    }
    #endregion

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Herbs"))
        {
            forPickingHerb.enabled = true;
            canPickHerb = true;
            playerControls.Player.Picking.performed += HerbsPickUp;
            forPickingHerb.text = "Press E or Controller Y";
            herbInRange = other.gameObject;
        }

        if (other.CompareTag("Key"))
        {
            forPickingHerb.enabled = true;
            canPickKey = true;
            playerControls.Player.Picking.performed += KeyPickUp;
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
            playerControls.Player.Picking.performed += BarrelIgnite;
            playerControls.Player.Picking.canceled += BarrelIgnite;
            forPickingHerb.text = "Hold E or Y";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Herbs"))
        {
            forPickingHerb.enabled = false;
            playerControls.Player.Picking.performed -= HerbsPickUp;
            herbInRange = null;
        }

        if (other.CompareTag("Key"))
        {
            forPickingHerb.enabled = false;
            canPickKey = false;
            playerControls.Player.Picking.performed -= KeyPickUp;
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
            playerControls.Player.Picking.performed -= BarrelIgnite;
            playerControls.Player.Picking.canceled -= BarrelIgnite;
        }
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
            PlayerHealth.maxHealth = PlayerHealth.baseHealth + (herbs * 20);
            PlayerHealth.Health = PlayerHealth.maxHealth;
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
        yield return new WaitForSeconds(barrelIgniteTime);

        BarrelSwitchingOn();//Access this in barrel script 
        Debug.Log("Couritien finished");
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
    public void EnterLockRegion(MoveRuller MR)//Calling this functions in MoveRuller which is lockScript
    {
        moveRuller = MR;
        playerControls.Player.DpadUp.performed += DpadUP;
        playerControls.Player.DpadDown.performed += DpadDOWN;
        playerControls.Player.DpadRight.performed += DpadRIGHT;
        playerControls.Player.DpadLeft.performed += DpadLEFT;
    }

    public void ExitLockRegion(MoveRuller MR)//Calling this functions in MoveRuller which is lockScript
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

    private void RunningStaminaBar()
    {
        if (currentState == playerRunningState)
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
        staminaBar.fillAmount = stamina / 100f;
    }

    private void CameraShake()
    {
        if (Mathf.Abs(playerInput.x) > 0.1f || Mathf.Abs(playerInput.y) > 0.1f)
        {
            timer += Time.deltaTime * (currentState == playerCrouchState ? croucSpeed : currentState == playerRunningState ? sprintSpeed : walkSpeed);
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, originalPosition + Mathf.Sin(timer) * (currentState == playerCrouchState ? croucSpeedAmount : isRunning && playerInput.y == 1 ? sprintSpeedAmount : walkSpeedAmount), playerCamera.transform.localPosition.z);
        }
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState?.ExitState();
        currentState = state;
        currentState.EnterState();
    }
}