using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Type
{
    소모품, 장비
}

public enum ItemType
{
    재료, 무기, 상의방어구, 하의방어구, 음식, 포션
}

public abstract class ItemDataSO : ScriptableObject
{
    public int ID;
    public Sprite ItemImage;
    public string Name;
    public Type Type;
    public ItemType ItemType;
    public GameObject ItemPrefab;
    public string ToolTip;

    public abstract Item CreateItem();
}

public abstract class EquipmentItemData : ItemDataSO
{
    public int MaxDurability = 100;
}

[CreateAssetMenu(fileName = "Weapon", menuName = "ItemData/Weapon")]
public class WeaponItemData : EquipmentItemData
{
    public float Damage;

    public override Item CreateItem()
    {
        return new WeaponItem(this);
    }
}

[CreateAssetMenu(fileName = "ArmorTop", menuName = "ItemData/ArmorTop")]
public class ArmorTopItemData : EquipmentItemData
{
    public float Def;

    public override Item CreateItem()
    {
        return new ArmorTopItem(this);
    }
}

[CreateAssetMenu(fileName = "ArmorBottom", menuName = "ItemData/ArmorBottom")]
public class ArmorBottomItemData : EquipmentItemData
{
    public float Def;

    public override Item CreateItem()
    {
        return new ArmorBottomItem(this);
    }
}


public abstract class CountableItemData : ItemDataSO
{
    public int MaxAmount = 99;
}

[CreateAssetMenu(fileName = "Portion", menuName = "ItemData/Portion")]
public class PortionItemData : CountableItemData
{
    public float Value;

    public override Item CreateItem()
    {
        return new PortionItem(this);
    }
}
