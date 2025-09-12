using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Player : MonoBehaviour
{
    private static readonly int IdleAnimation1 = Animator.StringToHash("IdleAnimation1");
    private static readonly int IdleAnimation2 = Animator.StringToHash("IdleAnimation2");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int CastBow = Animator.StringToHash("CastBow");
    private static readonly int Pew = Animator.StringToHash("Pew");
    
    private Rigidbody rb;
    PlayerData Data = PlayerData.Instance;
    public PlayerInputActions InputAction;
    [SerializeField] private SwitchCamera switchCamera;

    public Transform playerObj;
    private Camera mainCamera;
    public LayerMask whatIsGround;

    public Animator PlayerAnimator;

    private Renderer[] playerRenderers; 
    private float timeSinceLastMove = 0f;
    
    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject fishingRod;
    
    [SerializeField] private GameObject axe;
    [SerializeField] private GameObject bow;
    [SerializeField] private GameObject arrowBundle;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject arrowPosition;
    public GameObject initArrow;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform aimTransform;
    private bool almostAim = false;


    public float cooldownTime = 1.5f; 
    private float nextFireTime = 0f;
    public static int noOfClick = 0;
    float lastClickTime = 0f;
    float maxComboDelay = 1f;
    private void Awake()
    {
        switchCamera.ToggleCamera();
        switchCamera.ToggleCamera();
        InputAction = new PlayerInputActions();
        arrow.SetActive(true);
    }

    private void OnEnable()
    {
        InputAction.Player.Move.Enable();
        InputAction.Player.Jump.Enable();
        InputAction.Player.WalkToggle.Enable();
        InputAction.Player.WalkToggle.performed += OnWalkTogglePerformed;
        InputAction.Player.ToggleBuilderMode.Enable();
        InputAction.Player.ToggleBuilderMode.performed += OnToggleBuilderModePerformed;
        InputAction.Player.Aiming.Enable();
        InputAction.Player.Aiming.performed += OnAiming;
        InputAction.Player.Aiming.canceled += OnAimCanceled;
        InputAction.Player.Attack.Enable();
        InputAction.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        InputAction.Player.WalkToggle.performed -= OnWalkTogglePerformed;
        InputAction.Player.ToggleBuilderMode.performed -= OnToggleBuilderModePerformed;
        InputAction.Player.Attack.performed -= OnAttack;
        InputAction.Player.Aiming.performed -= OnAiming;
        InputAction.Player.Aiming.canceled -= OnAimCanceled;
        InputAction.Player.Aiming.Disable();
    }
    
    
    private void OnAttack(InputAction.CallbackContext obj)
    {
        if (PlayerData.Instance.equipBow)
        {
            if (PlayerData.Instance.isAiming)
            {
                PlayerAnimator.SetTrigger(Pew);
                Vector3 aimDir = (mouseWorldPosition - arrowPosition.transform.position).normalized;
                
                Quaternion arrowRotation = Quaternion.LookRotation(aimDir, Vector3.up);
                GameObject instantiatedArrow = Instantiate(arrow, arrowPosition.transform.position, arrowRotation * Quaternion.Euler(270, 0, 7));
                Vector3 forceDirection = transform.forward + transform.right * 0.09f;
                instantiatedArrow.GetComponent<Rigidbody>().AddForce(forceDirection * 25f, ForceMode.Impulse);
  
            }
        }
        else if (PlayerData.Instance.equipSword && Time.time > nextFireTime)
        {
            if (!PlayerData.Instance.canWalk)
            {
                return;
            }
            lastClickTime = Time.time;
            noOfClick++;
            if (noOfClick == 1)
            {
                PlayerAnimator.SetBool("Sword1", true);
            }
            noOfClick = Mathf.Clamp(noOfClick, 0, 2);
            Debug.Log("Clicking" + noOfClick);
            if (noOfClick >= 2 && PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f)
            {            
                PlayerAnimator.SetBool("Sword1", false);
                PlayerAnimator.SetBool("Sword2", true);

            }
        }
    }
    
    
    public void OnAiming(InputAction.CallbackContext obj)
    {
        if (PlayerData.Instance.equipBow)
        {
            PlayerAnimator.SetBool(CastBow, true);
            if (SceneManager.GetActiveScene().name != "Dungeon")
            {
                switchCamera.ToggleCamera();
            }
            Data.walkToggle = !Data.walkToggle;
            almostAim = true;
        }
        
    }

    private void OnAimCanceled(InputAction.CallbackContext obj)
    {
        if (PlayerData.Instance.equipBow)
        {
            PlayerData.Instance.isAiming = false;
            PlayerAnimator.SetBool(CastBow, false);
            if (SceneManager.GetActiveScene().name != "Dungeon")
            {
                switchCamera.ToggleCamera();
            }
            Data.walkToggle = !Data.walkToggle;
            almostAim = false;
        }
    }

    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        playerRenderers = GetComponentsInChildren<Renderer>();
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        mainCamera = Camera.main;
        ChangeTool(null);
    }
    
    private Vector3 mouseWorldPosition;
    private void Update()
    {
        if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f &&
            PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Sword1"))
        {
            PlayerAnimator.SetBool("Sword1", false);
        } 
        
        if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f &&
            PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Sword2"))
        {
            PlayerAnimator.SetBool("Sword2", false);
            noOfClick = 0;
        }

        if (Time.time - lastClickTime > maxComboDelay)
        {
            noOfClick = 0;
        }

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
        {
            aimTransform.position = hit.point;
            mouseWorldPosition = hit.point;
        }
        
        if (almostAim)
        {
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }

        if (Data.builderMode)
        {
            HandleBuilderMode();
            return;
        }

        Data.grounded = Physics.Raycast(
            transform.position, 
            Vector3.down, 
            Data.playerHeight * 0.5f + 0.2f, 
            whatIsGround
        );
        rb.drag = Data.grounded ? Mathf.Lerp(rb.drag, Data.groundDrag, Time.deltaTime * 5f) : 0.5f;
        if (PlayerData.Instance.canWalk)
        {
            Move();
        }
        SpeedControl();
        HandleIdleAnimation();
    }

    private float currentSpeed;
    private void Move()
    {
        Vector2 input = InputAction.Player.Move.ReadValue<Vector2>();
        Vector3 moveDirection = (mainCamera.transform.forward * input.y + mainCamera.transform.right * input.x).normalized;
        moveDirection.y = 0f;

        if (moveDirection != Vector3.zero)
        {    
            PlayerAnimator.SetBool("Chop", false);
            playerObj.forward = Vector3.Slerp(playerObj.forward, moveDirection, Time.deltaTime * Data.RotateSpeed);
            timeSinceLastMove = 0f;
            currentSpeed = Data.walkToggle ? Data.WalkSpeed : Data.RunSpeed;
            float speedMultiplier = Data.grounded ? Data.BaseSpeed : Data.airMultiplier * Data.BaseSpeed;
            rb.AddForce(moveDirection * (currentSpeed * speedMultiplier), ForceMode.Force);
            PlayerAnimator.SetBool(Walk, Data.walkToggle);
            PlayerAnimator.SetBool(Run, !Data.walkToggle);
        }
        else
        {
            PlayerAnimator.SetBool(Walk, false);
            PlayerAnimator.SetBool(Run, false);
        }
    }
    
    private void ResetJump()
    {
        Data.readyToJump = true;
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float currentSpeedLimit = Data.walkToggle ? Data.WalkSpeed : Data.RunSpeed;

        if (flatVel.magnitude > currentSpeedLimit)
        {
            Vector3 limitedVel = flatVel.normalized * currentSpeedLimit;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    private void HandleIdleAnimation()
    {
        timeSinceLastMove += Time.deltaTime;
        if (timeSinceLastMove >= 10f)
        {
            bool idleAnimation1 = Random.value > 0.5f;
            if (idleAnimation1)
            {
                PlayerAnimator.SetBool(IdleAnimation1, true);
            }
            else
            {
                PlayerAnimator.SetBool(IdleAnimation2, true);
            }
            timeSinceLastMove = 0f;
            Invoke(nameof(ResetIdleAnimations), 1f);
        }
    }
    
    private void ResetIdleAnimations()
    {
        PlayerAnimator.SetBool(IdleAnimation1, false);
        PlayerAnimator.SetBool(IdleAnimation2, false);
    }
    private void OnWalkTogglePerformed(InputAction.CallbackContext obj)
    {
        Data.walkToggle = !Data.walkToggle;
    }
    private void OnToggleBuilderModePerformed(InputAction.CallbackContext obj)
    {
        if (SceneManager.GetActiveScene().name != "Dungeon")
        {
            Data.builderMode = !Data.builderMode;
            HandleBuilderMode();
        }
    }
    private void HandleBuilderMode()
    {
        if (Data.builderMode)
        {
            rb.velocity = Vector3.zero; 
            foreach (var renderer in playerRenderers) 
                renderer.enabled = false; 
        }
        else
        {
            foreach (var renderer in playerRenderers)
                renderer.enabled = true; 
        }
    }
    public void ChangeTool(Tools tool)
    {
        if (tool == null)
        {
            PlayerData.Instance.equipBow = false;
            sword.SetActive(false);
            fishingRod.SetActive(false);
            axe.SetActive(false);
            arrowBundle.SetActive(false);
            bow.SetActive(false);
            Data.currentEquipmentName = "";
            return;
        }
        smoke_animation();
        smoke_animation();
        PlayerData.Instance.equipBow = false;
        PlayerData.Instance.equipSword = false;
        if (tool.toolName == "Fishing Rod")
        {
            sword.SetActive(false);
            fishingRod.SetActive(true);
            axe.SetActive(false);
            arrowBundle.SetActive(false);
            bow.SetActive(false);
            
        } else if (tool.toolName == "Axe")
        {
            sword.SetActive(false);
            fishingRod.SetActive(false);
            axe.SetActive(true);
            arrowBundle.SetActive(false);
            bow.SetActive(false);
        } else if (tool.toolName == "Sword")
        {
            PlayerData.Instance.equipSword = true;
            sword.SetActive(true);
            fishingRod.SetActive(false);
            axe.SetActive(false);
            arrowBundle.SetActive(false);
            bow.SetActive(false);
        } else if (tool.toolName == "Bow and Arrow")
        {
            PlayerData.Instance.equipBow = true;
            sword.SetActive(false);
            fishingRod.SetActive(false);
            axe.SetActive(false);
            arrowBundle.SetActive(true);
            bow.SetActive(true);
            
        }
        Data.currentEquipmentName = tool.toolName;
    }
    public void smoke_animation()
    {
        GameObject smokeEffectInstance = Instantiate(smokeEffect, playerObj.position, Quaternion.identity);
        Destroy(smokeEffectInstance, 3f);
    }
}
