using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlotUI : MonoBehaviour
{
    private int index; //각 슬롯의 식별 번호

    public int Index { get => index; set => index = value; }
    public void SetIndex(int index)
    {
        this.index = index;
    }
}
