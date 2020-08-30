using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemUI : ItemUI
{
    public new Action<ShopItemUI, PointerEventData> onClicked;

    [SerializeField]
    protected ShopItem shopItem;
    public ShopItem ShopItem => shopItem;

    public void SetShopItem(ShopItem _shopItem)
    {
        shopItem = _shopItem;
        tooltipData.text = shopItem.ToString();
    }

    public override void OnClicked(SelectableItem item, PointerEventData data)
    {
       // Debug.Log("Clicked me");
        selectableItem = item;
        onClicked?.Invoke(this, data);
    }
}
