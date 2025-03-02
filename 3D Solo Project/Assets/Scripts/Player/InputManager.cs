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
    [SerializeField] EquipmentManager equipmentManager;
    private Vector2 moveDir;
    private Vector2 lookDir;
    private Stack<System.Action> uiCloseStack;

    private void Awake()
    {
        inputActions = new PlayerInput();
        player = GetComponent<PlayerController>();
        uiCloseStack = new Stack<System.Action>();
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
        inputActions.PlayerAction.Attack.started += OnAttack;
        inputActions.PlayerAction.Attack.canceled += OnAttackCanceled;

        //UI
        inputActions.UIAction.OpenInven.performed += OnOpenInven;
        inputActions.UIAction.UILeftClick.started += OnLeftClick;
        inputActions.UIAction.UIDrag.performed += OnUIDrag;
        inputActions.UIAction.UILeftClick.canceled += OnLeftClickRelease;
        inputActions.UIAction.UIRightClick.started += OnRightClick;
        inputActions.UIAction.OpenEquipment.started += OnOpenEquipment;
        inputActions.UIAction.CloseUI.started += OnCloseUI;
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
        inputActions.PlayerAction.Attack.started -= OnAttack;
        inputActions.PlayerAction.Attack.canceled -= OnAttackCanceled;

        //UI
        inputActions.UIAction.OpenInven.performed -= OnOpenInven;
        inputActions.UIAction.UILeftClick.started -= OnLeftClick;
        inputActions.UIAction.UIDrag.performed -= OnUIDrag;
        inputActions.UIAction.UILeftClick.canceled -= OnLeftClickRelease;
        inputActions.UIAction.UIRightClick.started -= OnRightClick;
        inputActions.UIAction.OpenEquipment.started -= OnOpenEquipment;
        inputActions.UIAction.CloseUI.started -= OnCloseUI;
        inputActions.Disable();
    }

    public void OnLook(InputAction.CallbackContext callback)
    {
        if (!player.PlayerData.IsInveOpen || !player.PlayerData.IsEquipOpen)
        {
            lookDir = callback.ReadValue<Vector2>();
            cameraManager.CamRoDir = lookDir;
        }
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
        if(!player.PlayerData.IsInveOpen || !player.PlayerData.IsEquipOpen)
        {
            player.SetAttack(true);
        }
    }

    public void OnAttackCanceled(InputAction.CallbackContext callback)
    {
        if (!player.PlayerData.IsInveOpen || !player.PlayerData.IsEquipOpen)
        {
            if (player.EquipmentManager.ReaturnEquipmentWeaponType() == WeaponType.È°)
            {
                player.ReleaseArrow();
            }
        }
    }

    public void OnOpenInven(InputAction.CallbackContext callback)
    {
        UIManager.Instance.ToggleInven();
        OnOff();
    }

    public void OnLeftClick(InputAction.CallbackContext callback)
    {
        if (player.PlayerData.IsInveOpen || player.PlayerData.IsEquipOpen)
        {
            invenUI.OnPointerDown(callback);
            Debug.Log("´­¸²");
        }
    }
    public void OnLeftClickRelease(InputAction.CallbackContext callback)
    {
        if (player.PlayerData.IsInveOpen || player.PlayerData.IsEquipOpen)
        {
            invenUI.OnPointerUp(callback);
            Debug.Log("¶¼Áü");
        }
    }

    public void OnUIDrag(InputAction.CallbackContext callback)
    {
        if (player.PlayerData.IsInveOpen || player.PlayerData.IsEquipOpen)
        {
            invenUI.ShowOrHideItemTooltip();
            invenUI.OnPointerDrag(callback);
        }
    }

    public void OnRightClick(InputAction.CallbackContext callback)
    {
        if (player.PlayerData.IsInveOpen)
        {
            invenUI.OnRightPointDown(callback);
        }
        if(player.PlayerData.IsEquipOpen)
        {
            equipmentManager.OnRightClickEquipmentSlot(callback);
        }
    }
    public void OnOpenEquipment(InputAction.CallbackContext callback)
    {
        UIManager.Instance.ToggleEquip();
        OnOff();
    }

    public void OnCloseUI(InputAction.CallbackContext callback)
    {
        if(player.PlayerData.IsEquipOpen || player.PlayerData.IsInveOpen)
        {
            UIManager.Instance.CloseLastOpenedUI();
            OnOff();
        }
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
        inputActions.UIAction.UIDrag.Disable();
    }

    public void EnableUIControls()
    {
        inputActions.UIAction.UILeftClick.Enable();
        inputActions.UIAction.UIRightClick.Enable();
        inputActions.UIAction.UIDrag.Enable();
    }

    public void OnOff()
    {
        if (player.PlayerData.IsInveOpen || player.PlayerData.IsEquipOpen)
        {
            DisablePlayerControls();
            EnableUIControls();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else
        {
            EnablePlayerControls();
            DisableUIControls();
            Cursor.lockState = CursorLockMode.Confined; 
            Cursor.visible = false;
        }
    }
}
