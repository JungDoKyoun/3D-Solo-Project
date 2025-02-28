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
    [SerializeField] InvenUI invenUI;
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
        inputActions.PlayerAction.Attack.performed += OnAttack;

        //UI
        inputActions.UIAction.OpenInven.performed += OnOpenInven;
        inputActions.UIAction.UILeftClick.started += OnLeftClick;
        inputActions.UIAction.UILeftClick.canceled += OnLeftClickRelease;
        inputActions.UIAction.UIDrag.performed += OnUIDrag;
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
        inputActions.PlayerAction.Attack.performed -= OnAttack;

        //UI
        inputActions.UIAction.OpenInven.performed -= OnOpenInven;
        inputActions.UIAction.UILeftClick.started -= OnLeftClick;
        inputActions.UIAction.UILeftClick.canceled -= OnLeftClickRelease;
        inputActions.UIAction.UIDrag.performed -= OnUIDrag;
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

    public void OnAttack(InputAction.CallbackContext callback)
    {
        if(!player.PlayerData.IsInveOpen)
        {
            player.SetAttack(true);
        }
    }

    public void OnOpenInven(InputAction.CallbackContext callback)
    {
        ToggleInven();
        invenUI.InvenOnAndOff(player.PlayerData.IsInveOpen);
    }

    public void OnLeftClick(InputAction.CallbackContext callback)
    {
        invenUI.OnPointerDown(callback);
        Debug.Log("´­¸²");
    }
    public void OnLeftClickRelease(InputAction.CallbackContext callback)
    {
        invenUI.OnPointerDown(callback);
        Debug.Log("¶¼Áü");
    }

    public void OnUIDrag(InputAction.CallbackContext callback)
    {
        invenUI.OnPointerDrag(callback);
    }

    public void OnRightClick(InputAction.CallbackContext callback)
    {

    }

    public void DisablePlayerControls()
    {
        inputActions.PlayerAction.Look.Disable();
        inputActions.PlayerAction.Attack.Disable();
    }

    public void EnablePlayerControls()
    {
        inputActions.PlayerAction.Look.Enable();
        inputActions.PlayerAction.Attack.Enable();
    }

    public void DisableUIControls()
    {
        inputActions.UIAction.UILeftClick.Disable();
        inputActions.UIAction.UIRightClick.Disable();
    }

    public void EnableUIControls()
    {
        inputActions.UIAction.UILeftClick.Enable();
        inputActions.UIAction.UIRightClick.Enable();
    }

    public void ToggleInven()
    {
        player.PlayerData.IsInveOpen = !player.PlayerData.IsInveOpen;

        if(player.PlayerData.IsInveOpen)
        {
            DisablePlayerControls();
            EnableUIControls();
        }

        else
        {
            EnablePlayerControls();
            DisableUIControls();
        }
    }
}
