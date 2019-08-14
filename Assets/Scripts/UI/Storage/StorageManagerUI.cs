using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorageManagerUI : MonoBehaviour
{
    protected Dictionary<ItemDefinition.ItemType, StorageItemUI> items = new Dictionary<ItemDefinition.ItemType, StorageItemUI>();

    protected void Start()
    {
        StorageManager.Instance.onStorageItemAdded += OnItemAdded;
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
        StorageItemUI itemUI = Instantiate((Resources.Load("Prefabs/UI/StorageTile") as GameObject), transform).GetComponent<StorageItemUI>();
        itemUI.SetStoredIcon(item.Definition.Icon);
        itemUI.SetStoredQuantity(item.Quantity);

        items.Add(item.Definition.Type, itemUI);
    }

    protected void OnItemRemoved(StorageItem item)
    {

    }
}
