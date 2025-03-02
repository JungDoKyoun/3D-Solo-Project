using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;  // ΩÃ±€≈œ

    [SerializeField] private InvenUI invenUI;
    [SerializeField] private EquipmentManager equipmentManager;
    [SerializeField] private PlayerData playerData;

    private Stack<System.Action> uiCloseStack;
    private Stack<System.Action> uiSaveStack;
    private Dictionary<System.Action, bool> uiStateMap;

    private void Awake()
    {
        Instance = this;

        uiCloseStack = new Stack<System.Action>();
        uiStateMap = new Dictionary<System.Action, bool>();
        uiSaveStack = new Stack<System.Action>();
    }

    public void ToggleInven()
    {
        ToggleUI(ToggleInven, invenUI.InvenOnAndOff);
    }

    public void ToggleEquip()
    {
        ToggleUI(ToggleEquip, equipmentManager.EquipOnAndOff);
    }

    private void ToggleUI(System.Action toggleFunction, System.Action<bool> uiToggleAction)
    {
        bool isOpenInven = playerData.IsInveOpen;
        bool isOpenEquip = playerData.IsEquipOpen;

        if (toggleFunction == ToggleInven && isOpenInven)
        {
            FindTargetAndPop(toggleFunction);
            uiStateMap[toggleFunction] = false;
            uiToggleAction.Invoke(false);
            Debug.Log("µÈæÓø»");
        }

        else if (toggleFunction == ToggleEquip && isOpenEquip)
        {
            FindTargetAndPop(toggleFunction);
            uiStateMap[toggleFunction] = false;
            uiToggleAction.Invoke(false);
        }

        else
        {
            uiToggleAction.Invoke(true);
            uiCloseStack.Push(toggleFunction);
            uiStateMap[toggleFunction] = true;
            if (toggleFunction == ToggleInven)
            {
                playerData.IsInveOpen = true;
            }
            if (toggleFunction == ToggleEquip)
            {
                playerData.IsEquipOpen = true;
            }
            Debug.Log(playerData.IsInveOpen + "¿Œ∫•");
            Debug.Log(playerData.IsEquipOpen + "¿Â∫Ò");
        }
    }

    public void CloseLastOpenedUI()
    {
        if (uiCloseStack.Count > 0)
        {
            System.Action closeAction = uiCloseStack.Pop();
            if (uiStateMap.ContainsKey(closeAction) && uiStateMap[closeAction])
            {
                closeAction.Invoke();
                uiStateMap[closeAction] = false;
            }
            if (closeAction == ToggleInven)
            {
                playerData.IsInveOpen = false;
                Debug.Log(playerData.IsInveOpen);
            }
            if (closeAction == ToggleEquip)
            {
                playerData.IsEquipOpen = false;
                Debug.Log(playerData.IsEquipOpen);
            }
        }
    }

    public void FindTargetAndPop(System.Action target)
    {
        while(uiCloseStack.Count > 0 && uiCloseStack.Peek() != target)
        {
            uiSaveStack.Push(uiCloseStack.Pop());
        }

        if(uiCloseStack.Count > 0)
        {
            uiCloseStack.Pop();
        }

        if(uiSaveStack != null)
        {
            while (uiSaveStack.Count > 0)
            {
                uiCloseStack.Push(uiSaveStack.Pop());
            }
        }
    }
}
