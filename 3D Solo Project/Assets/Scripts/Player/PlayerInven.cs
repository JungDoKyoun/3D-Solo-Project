using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInven : MonoBehaviour
{
    [SerializeField] private InvenUI inventoryUI;
    private Item[] items;
    private int[] itemCount; //셀 수있는 아이템 숫자 저장
    private int _maxItemCount;
    [SerializeField] private WeaponItemData Wea;
    [SerializeField] private PortionItemData po;

    private void Awake()
    {
        _maxItemCount = 48;
        items = new Item[_maxItemCount];
        itemCount = new int[_maxItemCount];
    }

    //인벤토리의 빈 슬롯 찾기
    public int FindEmptySlotIndex()
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    //같은 소모품이 이미 인벤토리에 등록되어있나 확인
    public int FindSameItemSlot(CountableItem newItem)
    {
        for(int i = 0; i < items.Length; i++)
        {
            var current = items[i];

            if (current != null && current.data.ID == newItem.data.ID && current is CountableItem ci)
            {
                if (!ci.IsMax)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    //아이템 하나 업데이트
    public void UpdateSlot(int index)
    {
        Item item = items[index];

        if(item != null)
        {
            inventoryUI.SetItemIcon(index, item.data.ItemImage);

            if(item is CountableItem ci)
            {
                if(ci.IsEmpty)
                {
                    items[index] = null;
                    inventoryUI.ClearSlot(index);
                    return;
                }
                else
                {
                    inventoryUI.SetItemCountText(index, ci.Count);
                }
            }
        }
        else
        {
            inventoryUI.ClearSlot(index);
        }
    }

    //아이템 2개 이상
    public void UpdateSlot(params int[] index)
    {
        foreach(int i in index)
        {
            UpdateSlot(i);
        }
    }

    //아이템 추가
    public int AddItemInven(Item newItems, int count = 1)
    {
        int index;
        bool hasSameItem = true;

        if(newItems is CountableItem countItem)
        {
            while(count > 0)
            {
                if(hasSameItem)
                {
                    index = FindSameItemSlot(countItem);

                    if (index == -1)
                    {
                        hasSameItem = false;
                    }
                    else
                    {
                        CountableItem ci = items[index] as CountableItem;
                        count = ci.AddCountAndGetExcess(count);

                        UpdateSlot(index);
                    }
                }

                else
                {
                    index = FindEmptySlotIndex();

                    if(index == -1)
                    {
                        break;
                    }
                    else
                    {
                        items[index] = newItems;
                        CountableItem ci = items[index] as CountableItem;
                        ci.SetCount(count);
                        if(count > ci.MaxCount)
                        {
                            count -= ci.MaxCount;
                        }
                        else
                        {
                            count = 0;
                        }

                        UpdateSlot(index);
                    }
                }
            }
        }
        else
        {
            index = FindEmptySlotIndex();
            if (index != -1)
            {
                items[index] = newItems;
                count = 0;

                UpdateSlot(index);
            }
        }

        return count;
    }

    //아이템 제거
    public void RemoveItemInven(int index)
    {
        if (items[index] != null)
        {
            items[index] = null;
            inventoryUI.ClearSlot(index);
        }
    }

    //아이템 스왑
    public void Swap(int index1, int index2)
    {
        Item item1 = items[index1];
        Item item2 = items[index2];

        if(item1 != null && item2 != null
            && item1.data.ID == item2.data.ID
            && item1 is CountableItem ci1&& item2 is CountableItem ci2)
        {
            int sum = ci1.Count + ci2.Count;

            if(sum <= ci2.MaxCount)
            {
                ci1.SetCount(0);
                ci2.SetCount(sum);
            }
            else
            {
                ci1.SetCount(sum - ci2.MaxCount);
                ci2.SetCount(ci2.MaxCount);
            }
        }

        else
        {
            items[index1] = item2;
            items[index2] = item1;
        }

        UpdateSlot(index1, index2);
    }

    //아이템 사용
    public void UseItem(int index)
    {
        if (items[index] == null)
        {
            return;
        }

        if (items[index] is IUsableItem uItem)
        {
            if(uItem.Use())
            {
                UpdateSlot(index);
                Debug.Log("사용되었음");
            }
        }
    }

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Debug.Log("눌림");
            WeaponItem weap = new WeaponItem(Wea);
            AddItemInven(weap);
            Debug.Log(weap.data.name);
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            PortionItem por = new PortionItem(po);
            AddItemInven(por);
            Debug.Log(por.data.name);
        }
    }
}
