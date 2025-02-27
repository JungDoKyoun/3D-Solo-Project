using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
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
    private Transform beginIconDragTr; //드래그 시작한 슬롯의 아이콘 위치
    private Vector3 beginIconPoint; //드래그 시작한 위치
    private Vector3 beginDragPoint; //드래그 시작시 마우스 위치
    private int beginSlotIndex; //시작시 슬롯의 인덱스


    private void Awake()
    {
        pointerEvent.position = Input.mousePosition;
        _slotCount = 48;
        InitSlot();
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

    //인벤토리 업데이트
    public void UpdateInven(int index, Item item)
    {
        if(index >=0 && index < slotUIList.Count)
        {
            slotUIList[index].SetItem(item);
        }
    }

    //인벤토리 비움
    public void ClearSlot(int index)
    {
        slotUIList[index].ClearItem();
    }

    //마우스가 가리키는 곳의 레이케스트를 쏴서 특정 컴포넌트 있나 확인하고 있으면 가져옴
    private T RayCastAndGetCom<T>() where T : Component
    {
        raycastResults.Clear();

        gr.Raycast(pointerEvent, raycastResults);

        if(raycastResults.Count == 0)
        {
            return null;
        }
        return raycastResults[0].gameObject.GetComponent<T>();
    }

    //드래그 시작
    public void OnPointerDown()
    {
        if(Input.GetMouseButtonDown(0))
        {
            beginSlot = RayCastAndGetCom<ItemSlotUI>();

            if (beginSlot != null && beginSlot.IsHasItem)
            {
                beginIconDragTr = beginSlot.IconRect.transform;
                beginIconPoint = beginIconDragTr.position;
                beginDragPoint = Input.mousePosition;

                beginSlotIndex = beginSlot.Index;
                beginSlot.transform.SetAsLastSibling();
            }
            else
            {
                beginSlot = null;
            }
        }
    }

    //드래그중
    public void OnPointerDrag()
    {
        if(beginSlot == null)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            beginIconDragTr.position = beginIconPoint + (Input.mousePosition - beginDragPoint);
        }
    }

    //클릭 땜
    public void OnPointerUp()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(beginSlot != null)
            {
                beginIconDragTr.position = beginIconPoint;
            }
        }
    }
}
