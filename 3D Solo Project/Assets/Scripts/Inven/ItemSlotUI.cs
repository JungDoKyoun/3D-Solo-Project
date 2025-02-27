using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    private int _index; //각 슬롯의 식별 번호
    private bool _isHasItem; //슬롯에 아이템이 있는지?
    RectTransform iconRect;

    public int Index { get => _index; set => _index = value; }
    public bool IsHasItem { get => _isHasItem; set => _isHasItem = value; }
    public RectTransform IconRect { get => iconRect; set => iconRect = value; }

    private void Awake()
    {
        IsHasItem = false;
        iconRect = itemIcon.rectTransform;
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void SetItem(Item item)
    {
        if(item != null)
        {
            itemIcon.sprite = item.data.ItemImage;
            itemIcon.enabled = true;
            IsHasItem = true;
            Debug.Log("이미지 들어옴");
        }
    }

    public void ClearItem()
    {
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        IsHasItem = false;
    }
}
