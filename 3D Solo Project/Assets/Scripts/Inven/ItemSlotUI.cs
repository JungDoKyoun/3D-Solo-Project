using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private GameObject higLight;
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

    public void OnOffHigLight(bool TorF)
    {
        higLight.SetActive(TorF);
    }

    public void SetItemIcon(Sprite itemImage)
    {
        if(itemImage != null)
        {
            itemIcon.sprite = itemImage;
            itemIcon.enabled = true;
            IsHasItem = true;
        }
        else
        {
            ClearItem();
        }
    }

    public void ClearItem()
    {
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        IsHasItem = false;
    }

    public void SetItemCount(int count)
    {
        if(IsHasItem && count > 1)
        {
            itemCountText.gameObject.SetActive(true);
        }
        else
        {
            itemCountText.gameObject.SetActive(false);
        }

        itemCountText.text = count.ToString();
    }

    public void HideItemCountText()
    {
        itemCountText.gameObject.SetActive(false);
    }

    public void SwapOrMoveIcon(ItemSlotUI other)
    {
        if(other == null)
        {
            return;
        }
        if(other == this)
        {
            return;
        }

        var temp = itemIcon.sprite;

        if(other.IsHasItem)
        {
            SetItemIcon(other.itemIcon.sprite);
        }
        else
        {
            ClearItem();
        }

        other.SetItemIcon(temp);
    }
}
