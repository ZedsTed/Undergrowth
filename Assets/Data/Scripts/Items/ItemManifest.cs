using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemManifest.asset", menuName = "Data/Item Manifest")]
public class ItemManifest : ScriptableObject
{
    public List<ItemDefinition> itemDefinitions;

    public ItemDefinition GetItemDefinition(string name)
    {
        for (int i = itemDefinitions.Count; i-- > 0;)
        {
            if (itemDefinitions[i].DescriptiveName == name)
                return itemDefinitions[i];
        }

        return null;
    }

    public ItemDefinition GetItemDefinition(int index)
    {
        if (index >= 0 && index < itemDefinitions.Count)
            return itemDefinitions[index];

        return null;
    }

    public ItemDefinition GetItemDefinition(ItemDefinition.ItemType itemType)
    {
        for (int i = itemDefinitions.Count; i-- > 0;)
        {
            if (itemDefinitions[i].Type == itemType)
                return itemDefinitions[i];
        }

        return null;
    }
}
