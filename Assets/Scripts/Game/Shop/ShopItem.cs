using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class ShopItem
{
    protected ItemDefinition definition;
    public ItemDefinition Definition => definition;

    protected int quantity;
    public int Quantity => quantity;

    public ShopItem()
    { }

    public ShopItem(int amount)
    {
        quantity = amount;
    }

    public ShopItem(ItemDefinition def, int amount)
    {
        quantity = amount;
        definition = def;
    }

    public bool AddQuantity(int amount)
    {
        // TODO: Make this take off however much it can and then return the surplus.
        if ((quantity + amount) > ShopManager.Instance.MaxQuantityPerItem)
        {
            Debug.LogWarning(definition.DescriptiveName + " tried to add " + amount + " which brought it to over max value.");
            return false;
        }

        quantity += amount;

        return true;
    }

    /// <summary>
    /// Removes a given amount from the item's quantity and returns the remainder, if any.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public int RemoveQuantity(int amount)
    {
        if ((quantity - amount) < 0)
        {
            // We have less than is wanted to be removed, set our quantity to 0,
            // then return the remainder which is the mount we wanted to remove minus
            // the amount we have.
            quantity = 0;

            return amount - quantity;
        }

        quantity -= amount;

        return quantity;
    }

    StringBuilder sb;
    public override string ToString()
    {
        if (sb == null)
            sb = new StringBuilder();

        sb.Clear();
        sb.AppendLine("<b>" + definition.DescriptiveName + "</b>");
        sb.AppendLine(definition.Description);
        sb.AppendLine();
        sb.AppendLine("Stock: " + Quantity);

        return sb.ToString();
    }
}
