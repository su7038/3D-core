using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float crouchSpeed = 2.5f;

    [Header("Jumping")]
    public float jumpHeight = 1.5f;
    public int maxJumps = 1; // set to 2 for double jump

    [Header("Gravity")]
    public float gravity = -20f;
    public float groundedGravity = -2f;

    [Header("Crouching")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public float crouchTransitionSpeed = 8f;

    [Header("Look")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 85f;

    // Private state
    private CharacterController controller;
    private Vector3 velocity;
    private float verticalLookRotation;
    private int jumpsRemaining;
    private bool isCrouching;
    private bool isSprinting;

    // Input values (set by callbacks)
    private Vector2 moveInput;
    private Vector2 lookInput;

    // Jump buffering (allows jumping immediately after landing)
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    private bool jumpRequested;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Ensure jump state is initialized correctly on start.
        jumpsRemaining = maxJumps;
        jumpBufferCounter = 0f;

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ── Input System callbacks ──────────────────────────────────────────────
    // These are called automatically by PlayerInput (Behavior: Send Messages)

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpRequested = true;
            jumpBufferCounter = jumpBufferTime;
        }
    }

    void OnSprint(InputValue value)
    {
        if (value.isPressed)
            isSprinting = !isSprinting;
    }

    void OnCrouch(InputValue value)
    {
        if (value.isPressed)
            isCrouching = !isCrouching;
    }

    // ── Update ──────────────────────────────────────────────────────────────

    void Update()
    {
        HandleMouseLook();
        HandleCrouch();
        HandleMovement();

        // Countdown jump buffer so the player can press jump slightly before landing.
        if (jumpBufferCounter > 0f)
            jumpBufferCounter -= Time.deltaTime;
        else
            jumpRequested = false;

        HandleJump();
        ApplyGravity();
    }

    void HandleMouseLook()
    {
        if (cameraTransform == null) return;

        // Rotate player body horizontally
        transform.Rotate(Vector3.up * lookInput.x * mouseSensitivity);

        // Rotate camera vertically (clamped)
        verticalLookRotation -= lookInput.y * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }

    void HandleCrouch()
    {
        
        float targetHeight = isCrouching ? crouchHeight : standingHeight;

        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);
        controller.center = new Vector3(0f, controller.height / 2f, 0f);
        
        // Camera crouch
        float targetCamY = isCrouching ? 0.5f : .8f; 
        Vector3 camPos = cameraTransform.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetCamY, Time.deltaTime * crouchTransitionSpeed);
        cameraTransform.localPosition = camPos;
    }

    void HandleMovement()
    {
        // Sprint only when moving forward and not crouching
        bool sprinting = isSprinting && !isCrouching && moveInput.y > 0;
        float speed = isCrouching ? crouchSpeed : (sprinting ? sprintSpeed : walkSpeed);

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (move.magnitude > 1f)
            move.Normalize();

        controller.Move(move * speed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (controller.isGrounded)
            jumpsRemaining = maxJumps;

        if (jumpRequested && jumpsRemaining > 0)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpsRemaining--;
            jumpRequested = false;
            jumpBufferCounter = 0f;
        }
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = groundedGravity;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}