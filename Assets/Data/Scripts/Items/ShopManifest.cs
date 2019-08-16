using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopManifest.asset", menuName = "Data/Shop Manifest")]
public class ShopManifest : ScriptableObject
{
    public List<KeyValuePair<ItemDefinition, int>> itemDefinitions;

    public ItemDefinition GetItemDefinition(string name)
    {
        for (int i = itemDefinitions.Count; i-- > 0;)
        {            
            if (itemDefinitions[i].Key.DescriptiveName == name)
                return itemDefinitions[i].Key;
        }

        Debug.LogError("[ShopManifest] Unable to find item of name: " + name);

        return null;
    }

    public ItemDefinition GetItemDefinition(int index)
    {
        if (index >= 0 && index < itemDefinitions.Count)
            return itemDefinitions[index].Key;

        Debug.LogError("[ShopManifest] Unable to find item of index: " + index);

        return null;
    }

    public ItemDefinition GetItemDefinition(ItemDefinition.ItemType itemType)
    {
        for (int i = itemDefinitions.Count; i-- > 0;)
        {
            if (itemDefinitions[i].Key.Type == itemType)
                return itemDefinitions[i].Key;
        }

        Debug.LogError("[ShopManifest] Unable to find item of type: " + itemType);

        return null;
    }

    public int GetItemDefinitionQuantity(ItemDefinition.ItemType itemType)
    {
        for (int i = itemDefinitions.Count; i-- > 0;)
        {
            if (itemDefinitions[i].Key.Type == itemType)
                return itemDefinitions[i].Value;
        }

        Debug.LogError("[ShopManifest] Unable to find item of type: " + itemType);

        return 0;
    }

    public KeyValuePair<ItemDefinition, int> GetItemDefinitionAndQuantity(ItemDefinition.ItemType itemType)
    {
        for (int i = itemDefinitions.Count; i-- > 0;)
        {
            if (itemDefinitions[i].Key.Type == itemType)
                return itemDefinitions[i];
        }

        Debug.LogError("[ShopManifest] Unable to find item of type: " + itemType);

        return default;
    }
}
