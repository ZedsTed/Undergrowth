using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField]
    protected Image storedIcon;

    [SerializeField]
    protected TextMeshProUGUI storedQuantity;

    [SerializeField]
    protected SelectableItem selectableItem;

    [SerializeField]
    protected TooltipData tooltipData;

    public Action<ItemUI, PointerEventData> onClicked;


    // Start is called before the first frame update
    void Start()
    {
        selectableItem.onClicked += OnClicked;

        
    }

    public virtual void SetStoredIcon(Sprite icon)
    {
        storedIcon.sprite = icon;
    }

    public virtual void SetStoredQuantity(int quantity)
    {
        storedQuantity.text = quantity.ToString();
    }

    public virtual void OnClicked(SelectableItem item, PointerEventData data)
    {
        Debug.Log("Clicked me");
        selectableItem = item;
        onClicked?.Invoke(this, data);
    }
}
