using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : SingletonDontCreate<StorageManager>
{
    protected Dictionary<ItemDefinition.ItemType, StorageItem> stock = new Dictionary<ItemDefinition.ItemType, StorageItem>();

    [SerializeField]
    protected int maxStoragePerItem = 99;
    public int MaxStoragePerItem => maxStoragePerItem;

    public Action<StorageItem> onStorageItemAdded;
    public Action<StorageItem> onStorageItemRemoved;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AddStorageItem(ItemDefinition.ItemType type, int quantity)
    {
        bool success = false;

        if (stock.TryGetValue(type, out StorageItem item))
        {
            success = item.AddQuantity(quantity);

        }
        else
        {
            item = new StorageItem(quantity);
            stock.Add(type, item);
            success = true;
        }

        onStorageItemAdded?.Invoke(item);


        return success;
    }

    public bool RemoveStorageItem(ItemDefinition.ItemType type, int quantity, ref int remainingQuantity)
    {
        if (stock.TryGetValue(type, out StorageItem item))
        {
            remainingQuantity = item.RemoveQuantity(quantity);

            if (remainingQuantity == 0)
                stock.Remove(item.Definition.Type);

            onStorageItemRemoved?.Invoke(item);

            return true;
        }
        else
        {
            Debug.LogWarning("[StorageManager] Tried to remove stored stock that didn't exist: " + type.ToString());
            return false;
        }
    }
}
