
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class InvenUI : MonoBehaviour
{
    [SerializeField] private RectTransform dragIconParent; // 드래그 아이콘을 따로 표시할 부모 (Canvas 내 위치해야 함)
    private GameObject draggingIcon; // 현재 드래그 중인 아이콘
    [SerializeField] private PlayerInven playerInven;
    [SerializeField] private ItemTooltipUI itemTooltipUI;
    [SerializeField] private InventoryPopupUI inventoryPopupUI;
    [SerializeField] private PlayerData playerData;
    [Header("슬롯 설정")]
    private List<ItemSlotUI> slotUIList;
    [SerializeField] private GameObject slotPrefab; //슬롯 프리펩
    [SerializeField] private RectTransform slotParent;
    [SerializeField] RectTransform counterAreaRT;
    [SerializeField] private int _slotCount; //슬롯 갯수

    [Header("아이템 드래그 관련 설정")]
    private GraphicRaycaster gr;
    private PointerEventData pointerEvent;
    private List<RaycastResult> raycastResults;
    private ItemSlotUI beginSlot; //드래그 시작한 슬롯
    private ItemSlotUI pointerOverSlot; //포인터가 가리키는 슬롯
    private Transform beginIconDragTr; //드래그 시작한 슬롯의 아이콘 위치
    private Vector3 beginDragIconPoint; //드래그 시작한 위치
    private Vector3 beginDragCursorPoint; //드래그 시작시 마우스 위치
    private int beginDragSlotSiblingIndex; //시작시 슬롯의 인덱스
    private bool isDragging = false;


    private void Awake()
    {
        Debug.Log("일어났다");
        gr = GetComponent<GraphicRaycaster>();
        pointerEvent = new PointerEventData(EventSystem.current);
        raycastResults = new List<RaycastResult>();
        _slotCount = 48;
    }

    private void Update()
    {
        pointerEvent.position = Mouse.current.position.ReadValue();
    }

    public int SlotCount { get => _slotCount; }

    public void InitSlots(PlayerInven inven)
    {
        playerInven = inven;
        slotUIList = new List<ItemSlotUI>();

        for (int i = 0; i < playerInven.MaxItemCount; i++)
        {
            var slotGO = Instantiate(slotPrefab, slotParent);
            var slotUI = slotGO.GetComponent<ItemSlotUI>();
            slotUI.SetIndex(i);
            slotUIList.Add(slotUI);
        }
        gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    //private void InitSlot()
    //{
    //    slotUIList = new List<ItemSlotUI>(_slotCount);
    //    for (int i = 0; i < _slotCount; i++)
    //    {
    //        int slotIndex = i;

    //        //슬롯 생성 및 배치
    //        var slotRT = ClonSlot();
    //        slotRT.gameObject.SetActive(true);
    //        slotRT.gameObject.name = $"Item Slot[{slotIndex}]";

    //        //아이템 슬롯 리스트 등록
    //        var slotUI = slotRT.GetComponent<ItemSlotUI>();
    //        slotUI.SetIndex(slotIndex);
    //        slotUIList.Add(slotUI);
    //    }
    //}

    ////슬롯 복제
    //private RectTransform ClonSlot()
    //{
    //    GameObject slot = Instantiate(slotPrefab);
    //    RectTransform rt = slot.GetComponent<RectTransform>();
    //    rt.SetParent(counterAreaRT);

    //    return rt;
    //}

    //인벤토리 열고 닫기
    public void InvenOnAndOff(bool TorF)
    {
        playerData.IsInveOpen = TorF;
        gameObject.SetActive(TorF);
    }

    //인벤토리 이미지 업데이트
    public void SetItemIcon(int index, Sprite itemImage)
    {
        if (index >= 0 && index < slotUIList.Count)
        {
            slotUIList[index].SetItemIcon(itemImage);
        }
    }

    //인벤토리 제거 시도
    public void TryRemoveItem(int index)
    {
        playerInven.Remove(index);
    }

    public void RemoveItem(int index)
    {
        slotUIList[index].RemoveItem();
    }

    //인벤토리의 수량 텍스트 변경
    public void SetItemCountText(int index, int count)
    {
        slotUIList[index].SetItemCount(count);
    }

    //인벤토리창에서 마우스로 끌어서 서로 위치 바꾸기
    public void TrySwapItems(ItemSlotUI from, ItemSlotUI to)
    {
        if (from == to)
        {
            return;
        }

        from.SwapOrMoveIcon(to);
        playerInven.Swap(from.Index, to.Index);
    }

    //아이템 사용
    public void TryUseItem(int index)
    {
        playerInven.UseItem(index);
    }

    public void ShowOrHideItemTooltip()
    {
        pointerOverSlot = RayCastAndGetCom<ItemSlotUI>();

        bool isValid = pointerOverSlot != null && pointerOverSlot.IsHasItem && pointerOverSlot != beginSlot;

        if(isValid)
        {
            UpdateTooltipUI(pointerOverSlot);
            itemTooltipUI.Show();
        }
        else
        {
            itemTooltipUI.Hide();
        }
    }

    public void UpdateTooltipUI(ItemSlotUI slot)
    {
        if(!slot.IsHasItem)
        {
            return;
        }

        itemTooltipUI.SetItemInfo(playerInven.GetItemData(slot.Index));
    }

    //마우스가 UI창 위에 있는가??
    private bool IsOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        bool isOverUI = results.Count > 0;
        return isOverUI;
    }

    //마우스가 가리키는 곳의 레이케스트를 쏴서 특정 컴포넌트 있나 확인하고 있으면 가져옴
    private T RayCastAndGetCom<T>() where T : Component
    {
        if(raycastResults != null)
        raycastResults.Clear();

        if (gr == null || pointerEvent == null)
        {
            return null;
        }

        gr.Raycast(pointerEvent, raycastResults);
        if (raycastResults.Count == 0)
        {
            return null;
        }
        foreach(var r in raycastResults)
        {
            if(r.gameObject.GetComponent<T>())
            {
                return r.gameObject.GetComponent<T>();
            }
        }
        return null;
    }

    private void SetIconToFront(RectTransform iconTransform)
    {
        Canvas canvas = iconTransform.GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = 100; // 드래그 중일 때 최상위 표시
        }
    }

    private void ResetIconSortingOrder(RectTransform iconTransform)
    {
        Canvas canvas = iconTransform.GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = 1; // 원래 UI 정렬로 복구
        }
    }

    //드래그 시작
    public void OnPointerDown(InputAction.CallbackContext callback)
    {
        if (!callback.started)
        {
            return;
        }
        beginSlot = RayCastAndGetCom<ItemSlotUI>();
        Debug.Log(beginSlot);
        Debug.Log(beginSlot.IsHasItem);
        if (beginSlot != null && beginSlot.IsHasItem)
        {
            Debug.Log("인식함");
            beginIconDragTr = beginSlot.IconRect.transform;
            //beginIconDragTr.position = beginSlot.IconRect.position;
            beginDragIconPoint = beginIconDragTr.position;
            beginDragCursorPoint = Mouse.current.position.ReadValue();
            isDragging = true;
            Debug.Log(isDragging);
            beginDragSlotSiblingIndex = beginSlot.transform.GetSiblingIndex();
            //beginSlot.transform.SetAsLastSibling();
            SetIconToFront(beginSlot.IconRect);
        }
        else
        {
            beginSlot = null;
        }
    }

    public void OnRightPointDown(InputAction.CallbackContext callback)
    {
        ItemSlotUI slot = RayCastAndGetCom<ItemSlotUI>();

        if (slot != null && slot.IsHasItem)
        {
            TryUseItem(slot.Index);
        }
    }

    //드래그중
    public void OnPointerDrag(InputAction.CallbackContext callback)
    {
        if (!isDragging || beginSlot == null) return;
        Vector3 mousePos = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0f);

        beginIconDragTr.position =
                    beginDragIconPoint + (mousePos - beginDragCursorPoint);
    }

    //클릭 땜
    public void OnPointerUp(InputAction.CallbackContext callback)
    {
        if (!callback.canceled) return;

        if(beginSlot != null)
        {
            beginIconDragTr.position = beginDragIconPoint;

            ResetIconSortingOrder(beginSlot.IconRect);

            EndDrag();

            beginSlot = null;
            isDragging = false;
            beginIconDragTr = null;
        }
        
    }

    public void EndDrag()
    {
        if (beginSlot != null && isDragging)
        {
            ItemSlotUI endSlot = RayCastAndGetCom<ItemSlotUI>();

            if (endSlot != null)
            {
                Debug.Log("슬롯 교체");
                TrySwapItems(beginSlot, endSlot);
            }
        }

        if(!IsOverUI())
        {
            int index = beginSlot.Index;
            string itemName = playerInven.GetItemData(index).Name;
            inventoryPopupUI.OpenConfirmationPopup(() => TryRemoveItem(index), itemName);
        }
    }
}
