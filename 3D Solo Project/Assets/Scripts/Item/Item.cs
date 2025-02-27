using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public ItemDataSO data { get; set; }

    public Item(ItemDataSO itemData) => data = itemData;
}

public abstract class EquipmentItem : Item
{
    public EquipmentItemData EquipmentItemData { get; set; }
    public int Durability;

    protected EquipmentItem(EquipmentItemData itemData) : base(itemData)
    {
        EquipmentItemData = itemData;
        //Durability = EquipmentItemData.MaxDurability;
    }
}

public class WeaponItem : EquipmentItem
{
    public WeaponItem(WeaponItemData itemData) : base(itemData)
    {
    }
}
