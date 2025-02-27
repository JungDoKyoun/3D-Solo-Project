using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("슬롯 설정")]
    private List<ItemSlotUI> slotUIList;
    [SerializeField] private GameObject slotPrefab; //슬롯 프리펩
    [SerializeField] RectTransform counterAreaRT;
    [SerializeField] private float _itemSlotSize; //슬롯 사이즈
    [SerializeField] private int _horizontalSlotCount; //가로 슬롯 갯수
    [SerializeField] private int _verticalSlotCount; //세로 슬롯 갯수
    [SerializeField] private float _slotMargin; //슬롯 사이의 여백 
    [SerializeField] private float _counterAreaPadding; // 인벤토리 내부 여백

    private void Awake()
    {
        _itemSlotSize = 100;
        _horizontalSlotCount = 5;
        _verticalSlotCount = 5;
        _slotMargin = 8;
        _counterAreaPadding = 20;
        InitSlot();
    }

    private void InitSlot()
    {
        //아이템 슬롯 크기 설정
        slotPrefab.TryGetComponent(out RectTransform slotSize);
        slotSize.sizeDelta = new Vector2(_itemSlotSize, _itemSlotSize);
        slotPrefab.SetActive(false);

        //아이템 슬롯이 들어갈 기준점 설정
        Vector2 beginPos = new Vector2(_counterAreaPadding, _counterAreaPadding);
        Vector2 curPos = beginPos;

        slotUIList = new List<ItemSlotUI>(_horizontalSlotCount * _verticalSlotCount);

        for(int j = 0; j < _verticalSlotCount; j++)
        {
            for(int i = 0; i < _horizontalSlotCount; i++)
            {
                int slotIndex = (_horizontalSlotCount * j) + i;

                //슬롯 생성 및 배치
                var slotRT = ClonSlot();
                slotRT.pivot = new Vector2(0, 1);
                slotRT.anchoredPosition = curPos;
                slotRT.gameObject.SetActive(true);
                slotRT.gameObject.name = $"Item Slot[{slotIndex}]";

                //아이템 슬롯 리스트 등록
                var slotUI = slotRT.GetComponent<ItemSlotUI>();
                slotUI.SetIndex(slotIndex);
                slotUIList.Add(slotUI);

                //다음 배치될 가로 슬롯 위치 지정
                curPos.x += (_itemSlotSize + _slotMargin);
            }

            //다음 배치될 세로 슬롯 위치 지정
            curPos.x = beginPos.x;
            curPos.y += (_itemSlotSize + _slotMargin);
        }
    }

    //슬롯 복제
    private RectTransform ClonSlot()
    {
        GameObject slot = Instantiate(slotPrefab);
        RectTransform rt = slot.GetComponent<RectTransform>();
        rt.SetParent(counterAreaRT, false); 

        return rt;
    }
}
