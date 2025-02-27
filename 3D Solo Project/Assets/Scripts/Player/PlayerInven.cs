using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInven : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;
    private Item[] items;
    private int _maxItemCount;
    [SerializeField] private WeaponItemData Wea;

    private void Awake()
    {
        _maxItemCount = 48;
        items = new Item[_maxItemCount];
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

    //아이템 추가
    public void AddItemInven(Item item)
    {
        int index = FindEmptySlotIndex();
        
        if(index != -1)
        {
            items[index] = item;
            inventoryUI.UpdateInven(index, item);
            Debug.Log(items[index].data.name);
        }
        else
        {
            Debug.Log("꽉참");
        }
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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // A 키를 누르면 포션 추가
        {
            WeaponItem weap = new WeaponItem(Wea);
            AddItemInven(weap);
            Debug.Log(weap.data.name);
        }
    }
}
