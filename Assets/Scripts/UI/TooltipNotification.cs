using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipNotification : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Action<PointerEventData> onPointerEnter;
    public Action<PointerEventData> onPointerExit;

    protected void Start()
    {
        onPointerEnter += TooltipManager.Instance.OnPointerEnter;
        onPointerExit += TooltipManager.Instance.OnPointerExit;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke(eventData);
    }
}
