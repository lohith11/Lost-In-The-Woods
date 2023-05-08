using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.UI;

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
    public static event EventHandler levelEnd;
    public static event EventHandler saveGame;

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
    public GameObject heartBeat;
    [Range(60, 120)]
    public float FOV;
    [Space(10)]

    //Player Crouch
    [Header("< Player Crouch >")]
    [Space(5)]
    public float playerCrouchSpeed;
    public bool crouchPressed;
    public GameObject crouchCollider;
    public AudioClip crouchingAudio;
    public AudioClip standiingAudio;
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
    public bool canPickKey = false;
    public bool canPickHerb = false;
    public bool isBrazier;
    public bool inGrass;
    public bool isSaveGame;
    public int keyPicked = 0;
    public float barrelIgniteTime;
    public int herbs;
    public GameObject pickingThings;
    public AudioClip herbPickUpAudio;
    public AudioClip breath;
    [Space(10)]

    private GameObject herbInRange;
    private GameObject keyInRange;

    [HideInInspector] public PlayerBaseState currentState;
    private MoveRuller moveRuller;
    private Coroutine barrelCoroutine;
    private MouseLook mouseLookRef;
    private PlayerInput playerInputRef;
    [HideInInspector] public CapsuleCollider playerCollider;
    public Image healthPickUpEffect;
    private GameObject barrel1;
    private GameObject brazier1;
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
        if (PlayerHealth.isPlayerDead)
        {
            return;
        }
        pickingThings.SetActive(false);
        healthPickUpEffect.enabled = false;
        staminaBar.enabled = false;
        terrainDetector = new TerrainDetector();
        originalPosition = playerCamera.transform.localPosition.y;
        throwingRocks = GetComponent<ThrowingRocks>();
        playerRB = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
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

    public void PickingPot(InputAction.CallbackContext context)
    {
        GetComponent<ThrowingRocks>().PotPicking();
    }

    public void HerbsPickUp(InputAction.CallbackContext context)
    {
        HerbsPicking();
    }

    public void KeyPickUp(InputAction.CallbackContext context)
    {
        KeyPickUp();
    }

    public void BrazierIgnite(InputAction.CallbackContext context)
    {
        BrazierOn();
    }

    public void SaveGame(InputAction.CallbackContext context)
    {
        SavingGame();
    }

    public void BarrelIgnite(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            barrelCoroutine = StartCoroutine(barrel());
        }

        if (context.phase == InputActionPhase.Canceled)
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
        if (PlayerHealth.isPlayerDead)
        {
            playerControls.Disable();
        }
        CameraShake();
        Step();
        currentState.UpdateState();

        playerCurrentPosition = this.transform.localPosition;
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
            pickingThings.SetActive(true);
            canPickHerb = true;
            playerControls.Player.Picking.performed += HerbsPickUp;
            herbInRange = other.gameObject;
        }

        if (other.CompareTag("Key"))
        {
            pickingThings.SetActive(true);
            canPickKey = true;
            playerControls.Player.Picking.performed += KeyPickUp;
            keyInRange = other.gameObject;
        }

        if (other.gameObject.CompareTag("Grass") && crouchPressed)
        {
            Debug.Log("Entered");
           hidePlayer?.Invoke(this, EventArgs.Empty);
        }

        if (other.gameObject.CompareTag("Barrel"))
        {
            pickingThings.SetActive(true);
            isBarrel = true;
            playerControls.Player.Picking.performed += BarrelIgnite;
            playerControls.Player.Picking.canceled += BarrelIgnite;
            barrel1 = other.gameObject;
        }

        if (other.gameObject.CompareTag("Brazier"))
        {
            pickingThings.SetActive(true);
            isBrazier = true;
            playerControls.Player.Picking.performed += BrazierIgnite;
            playerControls.Player.Picking.performed += BrazierIgnite;
            brazier1 = other.gameObject;
        }

        if (other.gameObject.CompareTag("ChapterEnd"))
        {
            levelEnd?.Invoke(this, EventArgs.Empty);
        }

        if (other.gameObject.CompareTag("SaveGame"))
        {
            pickingThings.SetActive(true);
            isSaveGame = true;
            playerControls.Player.Picking.performed += SaveGame;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Grass") && crouchPressed)
        {
            hidePlayer?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Herbs"))
        {
            pickingThings.SetActive(false);
            canPickHerb = false;
            playerControls.Player.Picking.performed -= HerbsPickUp;
            herbInRange = null;
        }

        if (other.CompareTag("Key"))
        {
            pickingThings.SetActive(false);
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
            pickingThings.SetActive(false);
            isBarrel = false;
            playerControls.Player.Picking.performed -= BarrelIgnite;
            playerControls.Player.Picking.canceled -= BarrelIgnite;
        }

        if (other.gameObject.CompareTag("Brazier"))
        {
            pickingThings.SetActive(false);
            isBrazier = false;
            playerControls.Player.Picking.performed -= BrazierIgnite;
        }
        if (other.gameObject.CompareTag("SaveGame"))
        {
            pickingThings.SetActive(false);
            isSaveGame = false;
            playerControls.Player.Picking.performed -= SaveGame;
        }
    }
    #endregion

    #region Interactables

    public void KeyPickUp()
    {
        if (canPickKey)
        {
            keyPicked++;
            pickingThings.SetActive(false);
            Destroy(keyInRange);
            keyInRange = null;
            canPickKey = false;
        }
    }

    public void BrazierOn()
    {
        if (isBrazier)
        {
            brazier1.GetComponent<Brazier>().TurnOn();
        }
    }

    public void HerbsPicking()
    {
        StartCoroutine(HerbPickUpEffect());
        if (canPickHerb)
        {
            herbs++;
            pickingThings.SetActive(false);
            Destroy(herbInRange);
            herbInRange = null;
            PlayerHealth.maxHealth = PlayerHealth.baseHealth + (herbs * 20);
            PlayerHealth.Health = PlayerHealth.maxHealth;
            canPickHerb = false;
        }
    }

    public void BarrelSwitchingOn()
    {
        if (isBarrel)
        {
            barrel1.GetComponent<Barrel>().Explode();
            isBarrel = false;
        }
    }

    public IEnumerator barrel()
    {
        Debug.Log("Couritne entered");
        yield return new WaitForSeconds(barrelIgniteTime);

        BarrelSwitchingOn();//Access this in barrel script 
        Debug.Log("Couritien finished");
    }

    public void SavingGame()
    {
        if(isSaveGame)
        {
            Debug.Log("Is save game called");
            saveGame?.Invoke(this, EventArgs.Empty);
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

    private IEnumerator HerbPickUpEffect()
    {
        audioSource.PlayOneShot(herbPickUpAudio);
        healthPickUpEffect.enabled = true;
        yield return new WaitForSeconds(0.2f);
        healthPickUpEffect.enabled = false;
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