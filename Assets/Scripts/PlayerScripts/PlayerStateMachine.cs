using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerStateMachine : MonoBehaviour
{
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
    public bool canPickHerb;
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
    public AudioSource footStepSound = default;
    private TerrainDetector terrainDetector;
    public AudioClip[] woodSound = default;
    public AudioClip[] grassSound = default;
    public AudioClip[] dirtSound = default;
    private float footStepTimer;
    [Space(10)]

    public bool isAtttacking;
    public bool isAiming;
    public bool isPicking;
    public float FAVdelay; 
    private PlayerBaseState currentState;
    public PlayerControls playerControls;

    public CapsuleCollider standingCollider;
    public CapsuleCollider crouchCollider;

    private float getCurrentOffset => isRunning ? baseStepSpeed * sprintStepSpeed : crouchPressed ? baseStepSpeed * crouchStepSpeed : baseStepSpeed;

    private bool canPickKey = true;
    private int keyPicked = 0;

    private void Awake()
    {
        playerControls = new PlayerControls();
        
        playerIdleState = new PlayerIdleState(this);
        playerMovingState = new PlayerMovingState(this);
        playerJumpState = new PlayerJumpState(this);
        playerRunningState = new PlayerRunningState(this);
        playerCrouchState = new PlayerCrouchState(this);
        standingCollider = GetComponent<CapsuleCollider>();
        crouchCollider = GetComponent<CapsuleCollider>();
    }

    public void Start()
    {
        crouchCollider.enabled = false;
        canPickHerb = true;
        terrainDetector = new TerrainDetector();
        originalPosition = playerCamera.transform.localPosition.y;
        rayCastUp.transform.position = new Vector3(rayCastUp.transform.position.x, stepHeight, rayCastUp.transform.position.z);
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
    #endregion

    public void Update()
    {
        CameraShake();
        currentState.UpdateState();
        //HandleFootSteps();
        Step();

        isGrounded = Physics.CheckSphere(groundPosition.position, groundRadius, groundLayer);

        playerCurrentPosition = this.transform.position;
    }

    public void FixedUpdate()
    {
        currentState.FixedUpdateState();
        PlayerSteppingUp();
    }

    public void Step()
    {
        if(playerInput.magnitude == 0)
        {
            return;
        }
        footStepTimer -= Time.deltaTime;
        if(footStepTimer <= 0)
        {
            AudioClip clip = GetRandomClip();
            footStepSound.PlayOneShot(clip);
            footStepTimer = getCurrentOffset;
        }
    }

    private AudioClip GetRandomClip()
    {
        int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);
        switch (terrainTextureIndex)
        {
            case 0:
                return woodSound[Random.Range(0, woodSound.Length -1)];
            case 1:
                return dirtSound[Random.Range(0, dirtSound.Length -1)];
            case 2:
                return grassSound[Random.Range(0, grassSound.Length -1)];
            default:
                return dirtSound[Random.Range(0, dirtSound.Length -1)];
        }
    }

# region AnimationIK
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

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Herbs"))
        {
            forPickingHerb.enabled = true;
            forPickingHerb.text = "Press E or Controller Y";
            if (canPickHerb && isPicking)
            {
                herbs++;
                forPickingHerb.enabled = false;
                PlayerHealth.Health = PlayerHealth.maxHealth;
                Destroy(other.gameObject);
            }
        }

        if(other.CompareTag("Key"))
        {
            //TextFeild
            if(canPickKey && isPicking)
            {
                //text disable
                keyPicked++;
                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Herbs"))
        {
            forPickingHerb.enabled = true;
            forPickingHerb.text = "Press E or Controller Y";
            if (canPickHerb && isPicking)
            {
                herbs++;
                forPickingHerb.enabled = false;
                //PlayerHealth.Health = PlayerHealth.maxHealth;
                Destroy(other.gameObject);
            }
        }

        if (other.CompareTag("Key"))
        {
            //TextFeild
            if (canPickKey && isPicking)
            {
                //text disable
                keyPicked++;
                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Herbs"))
        {
            forPickingHerb.enabled = false;
        }

        if(other.CompareTag("Key"))
        {
            //Disable the text
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

        StopCoroutine(cor);
        cor = null;
    }
    #endregion

# region handlefeet
   /* public void HandleFootSteps()
    {
        if(playerInput.magnitude == 0)
        {
            return;
        }
        footStepTimer -= Time.deltaTime;

        if(footStepTimer <= 0)
        {
            if(Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 5))
            {
                switch(hit.collider.tag)
                {
                    case "FootSteps/Dirt":
                        footStepSound.PlayOneShot(dirtSound[Random.Range(0, dirtSound.Length - 1)]);
                        break;
                    case "FootSteps/Wood":
                        footStepSound.PlayOneShot(woodSound[Random.Range(0, woodSound.Length - 1)]);
                        break;
                    case "FootSteps/Grass":
                        footStepSound.PlayOneShot(grassSound[Random.Range(0, grassSound.Length - 1)]);
                        break;
                    default:
                        footStepSound.PlayOneShot(dirtSound[Random.Range(0, dirtSound.Length - 1)]);
                        break;
                }
            }
            footStepTimer = getCurrentOffset;
        }
    }*/
    #endregion

    public void PlayerSteppingUp()
    {
        RaycastHit hitLower;
        if(Physics.Raycast(rayCastDown.transform.position, transform.TransformDirection(Vector3.forward),out hitLower, 0.1f))
        {
            RaycastHit hitUpper;
            if(!Physics.Raycast(rayCastUp.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
            {
                playerRB.position -= new Vector3(0f, -smoothStep, 0f);
            }
        }

        RaycastHit hitLower45Degrees;
        if(Physics.Raycast(rayCastDown.transform.position, transform.TransformDirection(1.5f, 0, 1),out hitLower45Degrees, 0.1f))
        {
            RaycastHit hitUpper45Degrees;
            if(!Physics.Raycast(rayCastUp.transform.position, transform.TransformDirection(1.5f, 0, 1f),out hitUpper45Degrees, 0.2f))
            {
                playerRB.position -= new Vector3(0f, -smoothStep, 0f);
            }
        }

        RaycastHit hitLowerMinusDegrees;
        if (Physics.Raycast(rayCastDown.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinusDegrees, 0.1f))
        {
            RaycastHit hitUpperMinusDegrees;
            if (!Physics.Raycast(rayCastUp.transform.position, transform.TransformDirection(-1.5f, 0, 1f), out hitUpperMinusDegrees, 0.2f))
            {
                playerRB.position -= new Vector3(0f, -smoothStep, 0f);
            }
        }
    }

    public void CameraShake()
    {
        if (!isGrounded)
        {
            return;
        }

        else if (Mathf.Abs(playerInput.x) > 0.1f || Mathf.Abs(playerInput.y) > 0.1f)
        {
            timer += Time.deltaTime * (crouchPressed ? croucSpeed : isRunning ? sprintSpeed : walkSpeed);
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, originalPosition + Mathf.Sin(timer) * (crouchPressed ? croucSpeedAmount : isRunning ? sprintSpeedAmount : walkSpeedAmount), playerCamera.transform.localPosition.z);
        }
    }
    
    public void SwitchState(PlayerBaseState state)
    {
        currentState?.ExitState();
        currentState = state;
        currentState.EnterState();
    }
}