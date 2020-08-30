using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorageItemUI : ItemUI
{
    public new Action<StorageItemUI, PointerEventData> onClicked;

    [SerializeField]
    protected StorageItem storageItem;
    public StorageItem StorageItem => storageItem;

    public void SetStorageItem(StorageItem _storageItem)
    {
        storageItem = _storageItem;
        tooltipData.text = storageItem.ToString();
    }

    public override void OnClicked(SelectableItem item, PointerEventData data)
    {
        //Debug.Log("Clicked me");
        selectableItem = item;
        onClicked?.Invoke(this, data);
    }
}
