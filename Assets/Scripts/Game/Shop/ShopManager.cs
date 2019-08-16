using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : SingletonDontCreate<ShopManager>
{
    protected Dictionary<ItemDefinition.ItemType, ShopItem> stock = new Dictionary<ItemDefinition.ItemType, ShopItem>();

    [SerializeField]
    protected int maxShopPerItem = 99;
    public int MaxShopPerItem => maxShopPerItem;

    public Action<ShopItem> onShopItemAdded;
    public Action<ShopItem> onShopItemUpdated;
    public Action<ShopItem> onShopItemRemoved;


    public bool AddShopItem(ItemDefinition.ItemType type, int quantity)
    {
        bool success = false;

        if (stock.TryGetValue(type, out ShopItem item))
        {
            success = item.AddQuantity(quantity);
            onShopItemUpdated?.Invoke(item);
        }
        else
        {
            item = new ShopItem(quantity, ConstructionEditor.Instance.ItemManifest.GetItemDefinition(type));
            stock.Add(type, item);

            onShopItemAdded?.Invoke(item);

            success = true;
        }

        return success;
    }

    public bool RemoveShopItem(ItemDefinition.ItemType type, int quantity, ref int remainingQuantity)
    {
        if (stock.TryGetValue(type, out ShopItem item))
        {
            remainingQuantity = item.RemoveQuantity(quantity);

            if (remainingQuantity == 0)
                stock.Remove(item.Definition.Type);

            onShopItemRemoved?.Invoke(item);

            return true;
        }
        else
        {
            Debug.LogWarning("[ShopManager] Tried to remove stock that didn't exist: " + type.ToString());
            return false;
        }
    }
}
