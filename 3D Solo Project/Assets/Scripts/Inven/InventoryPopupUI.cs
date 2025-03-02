using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopupUI : MonoBehaviour
{
    // 1. 아이템 버리기 확인 팝업
    [Header("Confirmation Popup")]
    [SerializeField] private GameObject confirmationPopupObject;
    [SerializeField] private TextMeshProUGUI confirmationItemNameText;
    [SerializeField] private TextMeshProUGUI confirmationText;
    [SerializeField] private Button confirmationOkButton;     
    [SerializeField] private Button confirmationCancelButton;

    private event Action OnConfirmationOK;

    private void Awake()
    {
        confirmationOkButton.onClick.AddListener(() => OnConfirmationOK?.Invoke());
    }

    public void OpenConfirmationPopup(Action okCallback, string itemName)
    {
        ShowPanel();
        ShowConfirmationPopup(itemName);
        SetConfirmationOKEvent(okCallback);
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }
    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

    private void ShowConfirmationPopup(string itemName)
    {
        confirmationItemNameText.text = itemName;
        confirmationPopupObject.SetActive(true);
    }

    public void HideConfirmationPopup()
    {
        confirmationPopupObject.SetActive(false);
    }

    private void SetConfirmationOKEvent(Action handler)
    {
        OnConfirmationOK = handler;
    }
}
