using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorageManagerUI : WindowUI
{
    //TODO: ShopManagerUI and StorageManagerUI should inherit from a base ManagerUI.

    [SerializeField]
    protected RectTransform content;

    [SerializeField]
    protected TextMeshProUGUI emptyText;

    protected Dictionary<ItemDefinition.ItemType, StorageItemUI> items = new Dictionary<ItemDefinition.ItemType, StorageItemUI>();

    protected void Start()
    {
        transform.localScale = hideScale;

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

        //Debug.Log("Trying to add item");

        itemUI.onClicked += OnItemClicked;

        itemUI.SetStorageItem(item);
        itemUI.SetStoredIcon(item.Definition.Icon);
        itemUI.SetStoredQuantity(item.Quantity);

        items.Add(item.Definition.Type, itemUI);

        if (items.Count > 0 && emptyText.enabled)
            emptyText.enabled = false;
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
            itemUI.onClicked -= OnItemClicked;
            Destroy(itemUI.gameObject);
            items.Remove(item.Definition.Type);

            if (items.Count == 0 && !emptyText.enabled)
                emptyText.enabled = true;
        }
        else
        {
            Debug.LogError("[StorageManagerUI] Trying to remove an item we don't have a key for: " + item.Definition.Type);
        }
    }

    protected void OnItemClicked(StorageItemUI storageItemUI, PointerEventData data)
    {
        int remaining = 0;
        StorageItem item = storageItemUI.StorageItem;

        ShopManager.Instance.AddShopItem(item.Definition.Type, 1);
        StorageManager.Instance.RemoveStorageItem(item.Definition.Type, 1, ref remaining);

        Accounts.Instance.SellItem(item.Definition.Cost);        
        //shopItemUI.SetStoredQuantity(item.Quantity);
       // Debug.Log(item.Definition.DescriptiveName);
    }
}
