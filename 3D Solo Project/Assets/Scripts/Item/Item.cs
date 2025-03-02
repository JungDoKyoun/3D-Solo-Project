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
    public int Durability { get; set; }

    protected EquipmentItem(EquipmentItemData itemData) : base(itemData)
    {
        EquipmentItemData = itemData;
        Durability = EquipmentItemData.MaxDurability;
    }
}

public class WeaponItem : EquipmentItem
{
    public WeaponItem(WeaponItemData itemData) : base(itemData){ }
}

public class ArmorTopItem : EquipmentItem
{
    public ArmorTopItem(ArmorTopItemData itemData) : base(itemData) { }
}

public class ArmorBottomItem : EquipmentItem
{
    public ArmorBottomItem(ArmorBottomItemData itemData) : base(itemData) { }
}

public abstract class CountableItem : Item
{
    public CountableItemData CountableItemData { get; set; }
    public int Count { get; set; }

    public int MaxCount => CountableItemData.MaxAmount;
    public bool IsMax => Count >= CountableItemData.MaxAmount;
    public bool IsEmpty => Count <= 0;

    protected CountableItem(CountableItemData itemData, int count = 1) : base(itemData)
    {
        CountableItemData = itemData;
    }

    public void SetCount(int count)
    {
        Count = Mathf.Clamp(count, 0, MaxCount);
    }

    public int AddCountAndGetExcess(int count)
    {
        int nextCount = Count + count;
        SetCount(nextCount);
        if(nextCount > MaxCount)
        {
            return nextCount - MaxCount;
        }
        return 0;
    }
}

public class PortionItem : CountableItem, IUsableItem
{
    public PortionItem(PortionItemData itemData, int count = 1) : base(itemData)
    {
    }

    public bool Use()
    {
        Count--;
        return true;
    }
}

