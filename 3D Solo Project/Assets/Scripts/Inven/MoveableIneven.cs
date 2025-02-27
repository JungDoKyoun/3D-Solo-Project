using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveableIneven : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private Transform targetTr; //¿Å±æ °Í
    private Vector2 beginPoint;
    private Vector2 moveBegin;

    private void Awake()
    {
        if(targetTr == null)
        {
            targetTr = transform.parent;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        beginPoint = targetTr.position;
        moveBegin = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        targetTr.position = beginPoint + (eventData.position - moveBegin);
    }
}
