using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorageManagerUI : MonoBehaviour
{
    [SerializeField]
    protected RectTransform content;

    protected Dictionary<ItemDefinition.ItemType, StorageItemUI> items = new Dictionary<ItemDefinition.ItemType, StorageItemUI>();

    protected void Start()
    {
        StorageManager.Instance.onStorageItemAdded += OnItemAdded;
        StorageManager.Instance.onStorageItemUpdated += OnItemUpdated;
        StorageManager.Instance.onStorageItemRemoved += OnItemRemoved;
    }

    [ContextMenu("Add Item")]
    public void TestItemAdd()
    {
        StorageItem item = new StorageItem(50, ConstructionEditor.Instance.ItemManifest.GetItemDefinition(ItemDefinition.ItemType.Fertiliser));

        OnItemAdded(item);
    }

    protected void OnItemAdded(StorageItem item)
    {
        StorageItemUI itemUI = Instantiate((Resources.Load("Prefabs/UI/StorageTile") as GameObject), content).GetComponent<StorageItemUI>();

        Debug.Log("Trying to add item");

        itemUI.SetStoredIcon(item.Definition.Icon);
        itemUI.SetStoredQuantity(item.Quantity);

        items.Add(item.Definition.Type, itemUI);
    }

    protected void OnItemUpdated(StorageItem item)
    {
        if (items.TryGetValue(item.Definition.Type, out StorageItemUI itemUI))
        {
            itemUI.SetStoredQuantity(item.Quantity);
        }
        else
        {
            Debug.LogError("[StorageManagerUI] Trying to update an item we don't have a key for: " + item.Definition.Type);
        }
    }

    protected void OnItemRemoved(StorageItem item)
    {
        if (items.TryGetValue(item.Definition.Type, out StorageItemUI itemUI))
        {
            Destroy(itemUI.gameObject);
            items.Remove(item.Definition.Type);
        }
        else
        {
            Debug.LogError("[StorageManagerUI] Trying to remove an item we don't have a key for: " + item.Definition.Type);
        }
    }
}
