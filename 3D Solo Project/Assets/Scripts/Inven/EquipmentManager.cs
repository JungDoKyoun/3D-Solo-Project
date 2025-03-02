using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private Image weaponSlot;
    [SerializeField] private Image armorTopSlot;
    [SerializeField] private Image armorBottomSlot;
    private Dictionary<ItemType, Item> equippedItems = new Dictionary<ItemType, Item>();
    [SerializeField] private PlayerInven playerInventory;
    [SerializeField] private PlayerData playerData;


    public void EquipOnAndOff(bool TorF)
    {
        playerData.IsEquipOpen = TorF;
        gameObject.SetActive(TorF);
    }

    public void EquipItem(Item item, int index)
    {
        if(item is EquipmentItem equipment)
        {
            ItemType type = item.data.ItemType;

            if(equippedItems.ContainsKey(type))
            {
                UnequipItem(type);
            }

            equippedItems[type] = item;

            switch(type)
            {
                case ItemType.무기:
                    weaponSlot.sprite = item.data.ItemImage;
                    break;
                case ItemType.상의방어구:
                    armorTopSlot.sprite = item.data.ItemImage;
                    break;
                case ItemType.하의방어구:
                    armorBottomSlot.sprite = item.data.ItemImage;
                    break;
            }

            playerInventory.Remove(index);
        }
    }

    public void UnequipItem(ItemType type)
    {
        if(!equippedItems.ContainsKey(type))
        {
            return;
        }

        Item unequippedItem = equippedItems[type];
        equippedItems.Remove(type);

        switch (type)
        {
            case ItemType.무기:
                weaponSlot.sprite = null;
                break;
            case ItemType.상의방어구:
                armorTopSlot.sprite = null;
                break;
            case ItemType.하의방어구:
                armorBottomSlot.sprite = null;
                break;
        }

        playerInventory.AddItemInven(unequippedItem);
    }

    public void OnRightClickEquipmentSlot(InputAction.CallbackContext callback)
    {
        if (!callback.started) return;

        ItemType clickedType = GetClickedEquipmentSlotType();
        if (clickedType == ItemType.무기 || clickedType == ItemType.상의방어구 || clickedType == ItemType.하의방어구)
        {
            UnequipItem(clickedType);
        }
    }

    public ItemType GetClickedEquipmentSlotType()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        if (IsMouseOverSlot(weaponSlot, mousePos)) return ItemType.무기;
        if (IsMouseOverSlot(armorTopSlot, mousePos)) return ItemType.상의방어구;
        if (IsMouseOverSlot(armorBottomSlot, mousePos)) return ItemType.하의방어구;
        return ItemType.재료;
    }

    public bool IsMouseOverSlot(Image slot, Vector2 mousePos)
    {
        RectTransform rectTransform = slot.rectTransform;
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos, null);
    }
}
