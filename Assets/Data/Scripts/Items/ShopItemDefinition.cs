using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ShopItemDefinition.asset", menuName = "Data/Shop Item Definition")]
public class ShopItemDefinition : ScriptableObject
{
    [Tooltip("The item that is stocked.")]
    [SerializeField]
    [InlineEditor]
    protected ItemDefinition item;
    public ItemDefinition Item => item;

    [Tooltip("Quantity of item that start off as bases stock.")]
    [SerializeField]
    [BoxGroup("Shop Data")]
    protected int quantity;
    public int Quantity => quantity;
 
}
