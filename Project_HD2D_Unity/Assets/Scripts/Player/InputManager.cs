using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Variables

    public Vector2 MoveInput { get; private set; }
    public event Action OnJumpPressed;
    
    private PlayerInputAction playerInputAction;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        SetupActionsMap();
    }

    private void OnDisable()
    {
        StopActionsMap();
    }

    #endregion

    #region Setup

    private void SetupActionsMap()
    {
        playerInputAction.Player.Move.performed += ReceiveMove;
        playerInputAction.Player.Move.canceled += ReceiveMove;
        playerInputAction.Player.Jump.performed += ReceiveJump;
        playerInputAction.Enable();
    }

    private void StopActionsMap()
    {
        playerInputAction.Player.Move.performed -= ReceiveMove;
        playerInputAction.Player.Move.canceled -= ReceiveMove;
        playerInputAction.Player.Jump.performed -= ReceiveJump;
        playerInputAction.Disable();
    }

    #endregion

    #region Inputs

    private void ReceiveMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void ReceiveJump(InputAction.CallbackContext ctx)
    {
        OnJumpPressed?.Invoke();
    }

    #endregion
}