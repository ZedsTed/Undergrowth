using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ShopManagerUI : WindowUI
{
    [SerializeField]
    protected RectTransform content;

    [SerializeField]
    protected TextMeshProUGUI emptyText;

    [SerializeField]
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
    protected Dictionary<ItemDefinition.ItemType, ShopItemUI> items = new Dictionary<ItemDefinition.ItemType, ShopItemUI>();

    protected void Start()
    {
        transform.localScale = hideScale;

        ShopManager.Instance.onShopItemAdded += OnItemAdded;
        ShopManager.Instance.onShopItemUpdated += OnItemUpdated;
        ShopManager.Instance.onShopItemRemoved += OnItemRemoved;

        ShopManager.Instance.FirstTimeSetup();
    }

    protected void OnItemAdded(ShopItem item)
    {
        ShopItemUI itemUI = Instantiate((Resources.Load("Prefabs/UI/ShopTile") as GameObject), content).GetComponent<ShopItemUI>();

        //Debug.Log("Trying to add item");

        itemUI.onClicked += OnItemClicked;

        itemUI.SetShopItem(item);
        itemUI.SetStoredIcon(item.Definition.Icon);
        itemUI.SetStoredQuantity(item.Quantity);

        items.Add(item.Definition.Type, itemUI);

        if (items.Count > 0 && emptyText.enabled)
            emptyText.enabled = false;

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
            itemUI.onClicked -= OnItemClicked;
            Destroy(itemUI.gameObject);
            items.Remove(item.Definition.Type);

            if (items.Count == 0 && !emptyText.enabled)
                emptyText.enabled = true;
        }
        else
        {
            Debug.LogError("[ShopManagerUI] Trying to remove an item we don't have a key for: " + item.Definition.Type);
        }
    }


    protected void OnItemClicked(ShopItemUI shopItemUI, PointerEventData data)
    {
        int remaining = 0;
        ShopItem item = shopItemUI.ShopItem;

        ShopManager.Instance.RemoveShopItem(item.Definition.Type, 1, ref remaining);
        StorageManager.Instance.AddStorageItem(item.Definition.Type, 1);
        Accounts.Instance.BuyItem(item.Definition.Cost);
        //shopItemUI.SetStoredQuantity(item.Quantity);
        Debug.Log(item.Definition.DescriptiveName);
    }
}
