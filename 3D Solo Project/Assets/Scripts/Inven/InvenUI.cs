
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InvenUI : MonoBehaviour
{
    [SerializeField] private RectTransform dragIconParent; // 드래그 아이콘을 따로 표시할 부모 (Canvas 내 위치해야 함)
    private GameObject draggingIcon; // 현재 드래그 중인 아이콘
    [SerializeField] PlayerInven playerInven;
    [Header("슬롯 설정")]
    private List<ItemSlotUI> slotUIList;
    [SerializeField] private GameObject slotPrefab; //슬롯 프리펩
    [SerializeField] RectTransform counterAreaRT;
    [SerializeField] private int _slotCount; //슬롯 갯수

    [Header("아이템 드래그 관련 설정")]
    private GraphicRaycaster gr;
    private PointerEventData pointerEvent;
    private List<RaycastResult> raycastResults;
    private ItemSlotUI beginSlot; //드래그 시작한 슬롯
    private ItemSlotUI pointerOverSlot; //포인터가 가리키는 슬롯
    private Transform beginIconDragTr; //드래그 시작한 슬롯의 아이콘 위치
    private Vector3 beginIconPoint; //드래그 시작한 위치
    private Vector3 beginDragPoint; //드래그 시작시 마우스 위치
    private int beginSlotIndex; //시작시 슬롯의 인덱스
    private bool isDragging = false;


    private void Awake()
    {
        gr = GetComponent<GraphicRaycaster>();
        pointerEvent = new PointerEventData(EventSystem.current);
        raycastResults = new List<RaycastResult>();
        _slotCount = 48;
        InitSlot();
    }

    private void Update()
    {
        pointerEvent.position = Mouse.current.position.ReadValue();
    }

    public int SlotCount { get => _slotCount; }

    private void InitSlot()
    {
        slotUIList = new List<ItemSlotUI>(_slotCount);
        for (int i = 0; i < _slotCount; i++)
        {
            int slotIndex = i;

            //슬롯 생성 및 배치
            var slotRT = ClonSlot();
            slotRT.gameObject.SetActive(true);
            slotRT.gameObject.name = $"Item Slot[{slotIndex}]";

            //아이템 슬롯 리스트 등록
            var slotUI = slotRT.GetComponent<ItemSlotUI>();
            slotUI.SetIndex(slotIndex);
            slotUIList.Add(slotUI);
        }
    }

    //슬롯 복제
    private RectTransform ClonSlot()
    {
        GameObject slot = Instantiate(slotPrefab);
        RectTransform rt = slot.GetComponent<RectTransform>();
        rt.SetParent(counterAreaRT);

        return rt;
    }

    //인벤토리 열고 닫기
    public void InvenOnAndOff(bool TorF)
    {
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

    //인벤토리 비움
    public void ClearSlot(int index)
    {
        slotUIList[index].ClearItem();
        slotUIList[index].HideItemCountText();
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

    //인벤토리 창에서 아이템 장비 및 버리기
    public void TryRemoveItem(int index)
    {
        playerInven.RemoveItemInven(index);
    }

    //아이템 사용
    public void TryUseItem(int index)
    {
        playerInven.UseItem(index);
        Debug.Log("아이템 사용");
    }

    //마우스가 UI창 위에 있는가??
    private bool IsOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    //마우스가 가리키는 곳의 레이케스트를 쏴서 특정 컴포넌트 있나 확인하고 있으면 가져옴
    private T RayCastAndGetCom<T>() where T : Component
    {
        raycastResults.Clear();

        gr.Raycast(pointerEvent, raycastResults);
        if (raycastResults.Count == 0)
        {
            return null;
        }
        foreach(var r in raycastResults)
        {
            if(r.gameObject.GetComponent<T>())
            {
                Debug.Log(r.gameObject.GetComponent<T>());
                return r.gameObject.GetComponent<T>();
            }
        }
        return null;
    }

    //계속 깜빡임 시간 되면 해결하고 구현
    //마우스 포인터가 올라감
    //public void OnPointerEnterAndExit()
    //{
    //    var curSlot = RayCastAndGetCom<ItemSlotUI>();

        //if(prevSlot == null)
        //{
        //    if(curSlot != null)
        //    {
        //        curSlot.OnOffHigLight(true);
        //        Debug.Log("들ㅂㅂㅂ");
        //    }
        //}

        //else
        //{
        //    if(curSlot == null)
        //    {
        //        prevSlot.OnOffHigLight(false);
        //        Debug.Log("들ㅁㄴㅇ");
        //    }
        //    else if(prevSlot != curSlot)
        //    {
        //        prevSlot.OnOffHigLight(false);
        //        curSlot.OnOffHigLight(true);
        //        Debug.Log("들");
        //    }
        //}

    //    if (curSlot == pointerOverSlot)
    //    {
    //        return;
    //    }

    //    if (curSlot != pointerOverSlot)
    //    {
    //        if(pointerOverSlot != null && curSlot == null)
    //        {
    //            pointerOverSlot.OnOffHigLight(false);
    //            Debug.Log("들asd");
    //        }

    //        if(curSlot != null)
    //        {
    //            curSlot.OnOffHigLight(true);
    //            Debug.Log("들");
    //        }
    //    }

    //    pointerOverSlot = curSlot;
    //}

    //드래그 시작
    public void OnPointerDown(InputAction.CallbackContext callback)
    {
        if (!callback.started)
        {
            return;
        }

        beginSlot = RayCastAndGetCom<ItemSlotUI>();
        Debug.Log(beginSlot);

        if (beginSlot != null && beginSlot.IsHasItem)
        {
            draggingIcon = Instantiate(beginSlot.ItemIcon.gameObject, dragIconParent);
            draggingIcon.GetComponent<Image>().raycastTarget = false; // 다른 UI 클릭 방해 방지

            beginIconDragTr = draggingIcon.transform;
            beginIconDragTr.position = beginSlot.IconRect.position;
            beginIconPoint = beginIconDragTr.position;
            beginDragPoint = Mouse.current.position.ReadValue();
            isDragging = true;
            beginSlotIndex = beginSlot.Index;
            //beginSlot.transform.SetAsLastSibling();
        }
        else
        {
            beginSlot = null;
        }

        //if (beginSlot != null && beginSlot.IsHasItem)
        //{
        //    draggingIcon = Instantiate(beginSlot.ItemIcon.gameObject, dragIconParent);
        //    draggingIcon.GetComponent<Image>().raycastTarget = false; // 다른 UI 클릭 방해 방지

        //    beginIconDragTr = beginSlot.IconRect.transform;
        //    beginIconPoint = beginIconDragTr.position;
        //    beginDragPoint = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0f);
        //    isDragging = true;
        //    beginSlotIndex = beginSlot.Index;
        //    //beginSlot.transform.SetAsLastSibling();
        //}
        //else
        //{
        //    beginSlot = null;
        //}
    }

    public void OnRightPointDown(InputAction.CallbackContext callback)
    {
        ItemSlotUI slot = RayCastAndGetCom<ItemSlotUI>();

        if (slot != null && slot.IsHasItem)
        {
            Debug.Log("사용 눌림");
            TryUseItem(slot.Index);
        }
    }

    //드래그중
    public void OnPointerDrag(InputAction.CallbackContext callback)
    {
        if (!isDragging || beginSlot == null) return;

        Vector3 delta = callback.ReadValue<Vector2>();
        Vector3 mousePos = new Vector3(delta.x, delta.y, 0f);

        if (draggingIcon != null)
        {
            draggingIcon.transform.position = beginIconPoint + (mousePos - beginDragPoint);
        }

        //if (!isDragging || beginSlot == null)
        //{
        //    return;
        //}
        //Vector3 delta = callback.ReadValue<Vector2>();
        //Vector3 mousePos = new Vector3(delta.x, delta.y, 0f);
        //beginIconDragTr.position = beginIconPoint + (mousePos - beginDragPoint);
    }

    //클릭 땜
    public void OnPointerUp(InputAction.CallbackContext callback)
    {
        if (!callback.canceled) return;

        if (beginSlot != null && isDragging)
        {
            ItemSlotUI endSlot = RayCastAndGetCom<ItemSlotUI>();

            if (endSlot != null && endSlot != beginSlot)
            {
                Debug.Log("슬롯 교체");
                TrySwapItems(beginSlot, endSlot);
            }
        }

        // 드래그 종료 후 아이콘 제거
        if (draggingIcon != null)
        {
            Destroy(draggingIcon);
        }

        isDragging = false;
        beginSlot = null;

        //if(!callback.canceled)
        //{
        //    return;
        //}

        //if(beginSlot != null && isDragging)
        //{
        //    ItemSlotUI endSlot = RayCastAndGetCom<ItemSlotUI>();

        //    if(endSlot != null && endSlot != beginSlot)
        //    {
        //        Debug.Log("끝");
        //        TrySwapItems(beginSlot, endSlot);
        //    }
        //    else
        //    {
        //        beginIconDragTr.position = beginIconPoint;
        //    }
        //}

        //isDragging = false;
        //beginSlot = null;
    }
}
