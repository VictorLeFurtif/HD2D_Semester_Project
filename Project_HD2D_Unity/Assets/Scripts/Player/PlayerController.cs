using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables

    private PlayerInputAction playerInputAction;
    private Vector2 moveInput;
    private Vector3 moveDirection;
    private Vector3 targetDirection;
    private PlayerDataInstance playerData;
    private bool isGrounded;
    
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cam;
    [SerializeField] private PlayerData playerDataRaw;
    [SerializeField] private float playerHeight = 2f;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        playerData = playerDataRaw.Init();
    }

    private void OnEnable()
    {
        SetupActionsMap();
    }

    private void OnDisable()
    {
        StopActionsMap();
    }

    private void Update()
    {
        CalculateMoveDirection();
        CheckGround();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    #endregion

    #region Setup

    private void SetupActionsMap()
    {
        playerInputAction.Player.Move.performed += ReceiveMove;
        playerInputAction.Player.Move.canceled += ReceiveMove;
        playerInputAction.Player.Jump.performed += TryJump;
        playerInputAction.Enable();
    }

    private void StopActionsMap()
    {
        playerInputAction.Player.Move.performed -= ReceiveMove;
        playerInputAction.Player.Move.canceled -= ReceiveMove;
        playerInputAction.Player.Jump.performed -= TryJump;
        playerInputAction.Disable();
    }

    #endregion

    #region Inputs

    private void ReceiveMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    #endregion

    #region HandleMovement

    private void CheckGround()
    {
        Vector3 rayStart = transform.position - new Vector3(0, playerHeight / 2, 0);
        
        isGrounded = Physics.Raycast(
            rayStart,
            -Vector3.up,
            playerData.GroundCheckDistance,
            playerData.GroundMask);
    }

    private void CalculateMoveDirection()
    {
        Vector3 camForward = cam.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cam.right;
        camRight.y = 0;
        camRight.Normalize();

        targetDirection = camForward * moveInput.y + camRight * moveInput.x;
        
        if (targetDirection.magnitude > 0.1f)
        {
            targetDirection.Normalize();
        }
    }

    private void ApplyMovement()
    {
        Vector3 targetVelocity = targetDirection * playerData.MoveSpeed;
        Vector3 currentVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        Vector3 smoothedVelocity = Vector3.Lerp(currentVelocity, targetVelocity, 0.2f);
        
        rb.linearVelocity = new Vector3(
            smoothedVelocity.x,
            rb.linearVelocity.y,
            smoothedVelocity.z);
    }

    private void TryJump(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * playerData.JumpForce, ForceMode.Impulse);
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        if (rb == null) return;
        
        Vector3 rayStart = transform.position - new Vector3(0, playerHeight / 2, 0);
        Vector3 rayEnd = rayStart - new Vector3(0, playerData?.GroundCheckDistance ?? 0.2f, 0);
        
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(rayStart, rayEnd);
        Gizmos.DrawWireSphere(rayStart, 0.1f);
        
        Gizmos.color = isGrounded ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(rayEnd, 0.15f);
    }

    #endregion
}