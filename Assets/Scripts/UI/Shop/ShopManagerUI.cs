using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManagerUI : MonoBehaviour
{
    [SerializeField]
    protected RectTransform content;

    protected Dictionary<ItemDefinition.ItemType, ShopItemUI> items = new Dictionary<ItemDefinition.ItemType, ShopItemUI>();

    protected void Start()
    {
        ShopManager.Instance.onShopItemAdded += OnItemAdded;
        ShopManager.Instance.onShopItemUpdated += OnItemUpdated;
        ShopManager.Instance.onShopItemRemoved += OnItemRemoved;
    }

    [ContextMenu("Add Item")]
    public void TestItemAdd()
    {
        ShopItem item = new ShopItem(50, ConstructionEditor.Instance.ItemManifest.GetItemDefinition(ItemDefinition.ItemType.Fertiliser));

        OnItemAdded(item);
    }

    protected void OnItemAdded(ShopItem item)
    {
        ShopItemUI itemUI = Instantiate((Resources.Load("Prefabs/UI/ShopTile") as GameObject), content).GetComponent<ShopItemUI>();

        Debug.Log("Trying to add item");

        itemUI.SetStoredIcon(item.Definition.Icon);
        itemUI.SetStoredQuantity(item.Quantity);

        items.Add(item.Definition.Type, itemUI);
    }

    protected void OnItemUpdated(ShopItem item)
    {
        if (items.TryGetValue(item.Definition.Type, out ShopItemUI itemUI))
        {
            itemUI.SetStoredQuantity(item.Quantity);
        }
        else
        {
            Debug.LogError("[ShopManagerUI] Trying to update an item we don't have a key for: " + item.Definition.Type);
        }
    }

    protected void OnItemRemoved(ShopItem item)
    {
        if (items.TryGetValue(item.Definition.Type, out ShopItemUI itemUI))
        {
            Destroy(itemUI.gameObject);
            items.Remove(item.Definition.Type);
        }
        else
        {
            Debug.LogError("[ShopManagerUI] Trying to remove an item we don't have a key for: " + item.Definition.Type);
        }
    }
}
