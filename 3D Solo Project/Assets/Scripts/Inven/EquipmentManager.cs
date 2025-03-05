using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private Image weaponSlot;
    [SerializeField] private Image armorTopSlot;
    [SerializeField] private Image armorBottomSlot;
    private Dictionary<ItemType, Item> equippedItems;
    [SerializeField] private PlayerInven playerInventory;
    [SerializeField] private PlayerController player;

    public void Init()
    {
        if (equippedItems == null)
        {
            equippedItems = new Dictionary<ItemType, Item>();
        }
    }

    public void EquipOnAndOff(bool TorF)
    {
        player.PlayerData.IsEquipOpen = TorF;
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
                    ApplyWeaponEffect(equipment as WeaponItem);
                    break;
                case ItemType.상의방어구:
                    armorTopSlot.sprite = item.data.ItemImage;
                    ApplyArmorEffect(equipment as ArmorTopItem);
                    break;
                case ItemType.하의방어구:
                    armorBottomSlot.sprite = item.data.ItemImage;
                    ApplyArmorEffect(equipment as ArmorBottomItem);
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
                RemoveWeaponEffect();
                break;
            case ItemType.상의방어구:
                armorTopSlot.sprite = null;
                RemoveArmorEffect();
                break;
            case ItemType.하의방어구:
                armorBottomSlot.sprite = null;
                RemoveArmorEffect();
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

    public WeaponType ReaturnEquipmentWeaponType()
    {
        if (equippedItems == null)
        {
            equippedItems = new Dictionary<ItemType, Item>();
        }

        if (equippedItems.ContainsKey(ItemType.무기))
        {
            var weapon = equippedItems[ItemType.무기];
            if(weapon is WeaponItem weaponItem)
            {
                return weaponItem.WeaponItemData.weaponType;
            }
        }
        return WeaponType.주먹;
    }

    private void ApplyWeaponEffect(WeaponItem weapon)
    {
        if (weapon == null)
        {
            return;
        }
        player.CurrentAtk = player.PlayerData.PlayerAtk + (int)weapon.WeaponItemData.Damage;
        player.UpdateWeaponPrefab(weapon.WeaponItemData.ItemPrefab);
        Debug.Log(player.CurrentAtk);
    }

    private void RemoveWeaponEffect()
    {
        player.CurrentAtk = player.PlayerData.PlayerAtk;
        player.UpdateWeaponPrefab(null);
    }

    private void ApplyArmorEffect(EquipmentItem armor)
    {
        if (armor == null)
        {
            return;
        }
        player.CurrentDef = player.PlayerData.PlayerDef + (int)(armor.EquipmentItemData as ArmorTopItemData)?.Def;
    }

    private void RemoveArmorEffect()
    {
        player.CurrentDef = player.PlayerData.PlayerDef;
    }
}
