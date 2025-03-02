using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText; //아이템 이름
    [SerializeField] private TextMeshProUGUI contentText; //설명
    private RectTransform rt;
    private CanvasScaler canvasScaler;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        canvasScaler = GetComponentInParent<CanvasScaler>();
    }

    //클릭 안되게 하기
    private void DisableAllChildrenRaycastTarget(Transform tr)
    {
        tr.TryGetComponent(out Graphic gr);
        if (gr != null)
        {
            gr.raycastTarget = false;
        }

        int childCount = tr.childCount;
        if (childCount == 0)
        {
            return;
        }

        for (int i = 0; i < childCount; i++)
        {
            DisableAllChildrenRaycastTarget(tr.GetChild(i));
        }
    }

    public void SetItemInfo(ItemDataSO data)
    {
        titleText.text = data.Name;
        contentText.text = data.ToolTip;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
