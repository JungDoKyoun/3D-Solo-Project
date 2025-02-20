using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput inputActions;
    private PlayerController player;
    [SerializeField]CameraManager cameraManager;
    private Vector2 moveDir;
    private Vector2 lookDir;

    private void Awake()
    {
        inputActions = new PlayerInput();
        player = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.PlayerAction.Move.performed += OnMove;
        inputActions.PlayerAction.Move.canceled += OnMove;
        inputActions.PlayerAction.Look.performed += OnLook;
        inputActions.PlayerAction.Look.canceled += OnLook;
        inputActions.PlayerAction.Sprint.performed += OnSprint;
        inputActions.PlayerAction.Sprint.canceled += OnSprintCanceled;
        inputActions.PlayerAction.Jump.performed += OnJump;
        inputActions.PlayerAction.Jump.canceled += OnJumpCanceled;
    }

    private void OnDisable()
    {
        inputActions.PlayerAction.Move.performed -= OnMove;
        inputActions.PlayerAction.Move.canceled -= OnMove;
        inputActions.PlayerAction.Look.performed -= OnLook;
        inputActions.PlayerAction.Look.canceled -= OnLook;
        inputActions.PlayerAction.Sprint.performed -= OnSprint;
        inputActions.PlayerAction.Sprint.canceled -= OnSprintCanceled;
        inputActions.PlayerAction.Jump.performed -= OnJump;
        inputActions.PlayerAction.Jump.canceled -= OnJumpCanceled;
        inputActions.Disable();
    }

    public void OnLook(InputAction.CallbackContext callback)
    {
        lookDir = callback.ReadValue<Vector2>();
        cameraManager.CamRoDir = lookDir;
    }

    public void OnMove(InputAction.CallbackContext callback)
    {
        moveDir = callback.ReadValue<Vector2>();
        player.InputMoveDir = moveDir;
    }

    public void OnSprint(InputAction.CallbackContext callback)
    {
        player.SetSprint(true);
    }

    public void OnSprintCanceled(InputAction.CallbackContext callback)
    {
        player.SetSprint(false);
    }

    public void OnJump(InputAction.CallbackContext callback)
    {
        player.SetJump(true);
    }

    public void OnJumpCanceled(InputAction.CallbackContext callback)
    {
        player.SetJump(false);
    }
}
