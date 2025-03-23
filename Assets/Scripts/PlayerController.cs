using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float jumpForce = 8f;
    public float rotationSpeed = 2f;
    private Vector3 lastPosition;

    [Header("Input Smoothing")]
    public float movementSmoothTime = 0.1f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Physics")]
    public float gravity = -19.62f;
    
    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed;
    private bool isRunning;
    private Transform cameraTransform;
    private bool isJumping;
    
    // Smoothing variables
    private Vector3 currentVelocityRef;
    private Vector3 smoothInputVelocity;
    private Vector3 smoothInputRef;
    private float currentHorizontalSpeed;
    
    // Animation parameter names
    private readonly string SPEED_PARAM = "Speed";
    private readonly string IS_RUNNING_PARAM = "IsRunning";
    private readonly string IS_JUMPING_PARAM = "IsJumping";
    private readonly string IS_GROUNDED_PARAM = "IsGrounded";
    private readonly string JUMP_TRIGGER = "Jump";
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        cameraTransform = Camera.main.transform;
        lastPosition = transform.position;

        if (groundCheck == null)
        {
            GameObject groundCheckObj = new GameObject("GroundCheck");
            groundCheckObj.transform.parent = transform;
            groundCheckObj.transform.localPosition = new Vector3(0, -0.9f, 0);
            groundCheck = groundCheckObj.transform;
        }
    }
    
    void Update()
    {
        if (!isGrounded)
        {
            Debug.Log("Player is not grounded.");
        }
        else
        {
            Debug.Log("Player is grounded.");
        }

        // // Visualize ground check (for debugging)
        // Debug.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundDistance, isGrounded ? Color.green : Color.red);
        // isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

          // More reliable ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance + 0.1f);
        
        // Debug ray (should be visible in Scene view)
        Debug.DrawRay(transform.position, Vector3.down * (groundDistance + 0.1f), isGrounded ? Color.green : Color.red);


        // Reset velocity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            isJumping = false;
            animator.SetBool(IS_JUMPING_PARAM, false);
        }

        HandleMovement();
        HandleJump();
        ApplyGravity();
        UpdateAnimations();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(horizontal, 0f, vertical);
        input = Vector3.ClampMagnitude(input, 1f);

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 desiredMoveDirection = (forward * input.z + right * input.x);
        desiredMoveDirection = Vector3.ClampMagnitude(desiredMoveDirection, 1f);

        isRunning = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isRunning ? runSpeed : walkSpeed;

        // if (Mathf.Abs(horizontal) > 0.1f)
        // {
        //     // Rotate towards movement direction
        //     Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
        //     transform.rotation = Quaternion.Slerp(
        //         transform.rotation,
        //         targetRotation,
        //         rotationSpeed * Time.deltaTime
        //     );

        //     // Move in that direction
        //     Vector3 move = desiredMoveDirection * currentSpeed;
        //     controller.Move(move * Time.deltaTime);
        // }

        if (Mathf.Abs(horizontal) > 0.1f)
        {
            // Rotate towards movement direction
            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Handle movement (for any input)
        if (input.magnitude >= 0.1f)
        {
            Vector3 move = desiredMoveDirection * currentSpeed;
            controller.Move(move * Time.deltaTime);
        }

        // Calculate speed for animation
        Vector3 horizontalMovement = transform.position - lastPosition;
        horizontalMovement.y = 0;
        currentHorizontalSpeed = horizontalMovement.magnitude / Time.deltaTime;
        lastPosition = transform.position;
    }

    void HandleJump()
    {
        // Jump when space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key pressed.");
            // // Allow jumping if grounded OR if already jumping (for multiple jumps)
            // if (isGrounded || isJumping)
            // {
            //     Debug.Log("Jump initiated!");
            //     // Set vertical velocity for jump
            //     velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                
            //     // Set animation states
            //     animator.SetTrigger(JUMP_TRIGGER);
            //     animator.SetBool(IS_JUMPING_PARAM, true);
            //     isJumping = true;
            // }

        // Allow jumping unconditionally
        Debug.Log("Jump initiated!");
        
        // Set vertical velocity for jump
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        
        // Set animation states
        animator.SetTrigger(JUMP_TRIGGER);
        animator.SetBool(IS_JUMPING_PARAM, true);
        isJumping = true;
        }
    }

    void ApplyGravity()
    {
        // Apply gravity
        // velocity.y += gravity * Time.deltaTime;

        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        
        // Move based on vertical velocity
        controller.Move(velocity * Time.deltaTime);
    }
    
    void UpdateAnimations()
    {
        float normalizedSpeed = currentHorizontalSpeed / walkSpeed;
        normalizedSpeed = Mathf.Clamp(normalizedSpeed, 0, isRunning ? 2 : 1);
        
        animator.SetFloat(SPEED_PARAM, normalizedSpeed);
        animator.SetBool(IS_RUNNING_PARAM, isRunning);
        animator.SetBool(IS_GROUNDED_PARAM, isGrounded);
    }
}