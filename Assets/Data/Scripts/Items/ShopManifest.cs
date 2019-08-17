using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopManifest.asset", menuName = "Data/Shop Manifest")]
public class ShopManifest : ScriptableObject
{ 
    public List<ShopItemDefinition> stockedItems;

    //public bool AddItemDefinition(ItemDefinition itemDefinition, int quantity)
    //{
    //    if (GetItemDefinition(itemDefinition.Type, true) != null)
    //        return false;

    //    stockedItems.Add(new ShopItemDefinition());

    //    return true;
    //}

    public ItemDefinition GetItemDefinition(string name)
    {
        for (int i = stockedItems.Count; i-- > 0;)
        {            
            if (stockedItems[i].Item.DescriptiveName == name)
                return stockedItems[i].Item;
        }

        Debug.LogError("[ShopManifest] Unable to find item of name: " + name);

        return null;
    }

    public ItemDefinition GetItemDefinition(int index)
    {
        if (index >= 0 && index < stockedItems.Count)
            return stockedItems[index].Item;

        Debug.LogError("[ShopManifest] Unable to find item of index: " + index);

        return null;
    }

    public ItemDefinition GetItemDefinition(ItemDefinition.ItemType itemType, bool supressError = false)
    {
        if (stockedItems.Count == 0)
            return null;

        for (int i = stockedItems.Count; i-- > 0;)
        {
            if (stockedItems[i].Item.Type == itemType)
                return stockedItems[i].Item;
        }

        if (!supressError)
            Debug.LogError("[ShopManifest] Unable to find item of type: " + itemType);

        return null;
    }

    public int GetItemDefinitionQuantity(ItemDefinition.ItemType itemType)
    {
        for (int i = stockedItems.Count; i-- > 0;)
        {
            if (stockedItems[i].Item.Type == itemType)
                return stockedItems[i].Quantity;
        }

        Debug.LogError("[ShopManifest] Unable to find item of type: " + itemType);

        return 0;
    }

    public void GetItemDefinitionAndQuantity(ItemDefinition.ItemType itemType, out ItemDefinition item, out int quantity)
    {
        for (int i = stockedItems.Count; i-- > 0;)
        {
            if (stockedItems[i].Item.Type == itemType)
            {
                item = stockedItems[i].Item;
                quantity = stockedItems[i].Quantity;

                return;
            }
        }

        Debug.LogError("[ShopManifest] Unable to find item of type: " + itemType);

        item = null;
        quantity = 0;
    }
}
