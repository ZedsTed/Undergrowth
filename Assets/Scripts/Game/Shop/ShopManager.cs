using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ShopManager : SingletonDontCreate<ShopManager>
{
   
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
    public Dictionary<ItemDefinition.ItemType, ShopItem> stock = new Dictionary<ItemDefinition.ItemType, ShopItem>();

    [SerializeField]
    protected int maxQuantityPerItem = 99;
    public int MaxQuantityPerItem => maxQuantityPerItem;

    public Action<ShopItem> onShopItemAdded;
    public Action<ShopItem> onShopItemUpdated;
    public Action<ShopItem> onShopItemRemoved;


    protected void Start()
    {
        //FirstTimeSetup();
    }

    public void FirstTimeSetup()
    {
        for (int i = 0, iC = ConstructionEditor.Instance.ShopManifest.stockedItems.Count; i < iC; ++i)
        {
            AddShopItem(ConstructionEditor.Instance.ShopManifest.GetItemDefinition(i).Type);   
        }
    }

    protected void Update()
    {
        
    }

    public bool AddShopItem(ItemDefinition.ItemType type)
    {
        bool success = false;


        ConstructionEditor.Instance.ShopManifest.GetItemDefinitionAndQuantity(type, out ItemDefinition itemDef, out int quantity);

        if (itemDef == null)
            return false;

        ShopItem item = new ShopItem(itemDef, quantity);
       // Debug.Log("Adding: " + itemDef.DescriptiveName);
        stock.Add(type, item);

        onShopItemAdded?.Invoke(item);

        success = true;

        return success;
    }

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
            item = new ShopItem(ConstructionEditor.Instance.ShopManifest.GetItemDefinition(type), quantity);
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
            {
                stock.Remove(item.Definition.Type);

                onShopItemRemoved?.Invoke(item);
            }
            else
            {
                onShopItemUpdated?.Invoke(item);
            }

            return true;
        }
        else
        {
            Debug.LogWarning("[ShopManager] Tried to remove stock that didn't exist: " + type.ToString());
            return false;
        }
    }
}
