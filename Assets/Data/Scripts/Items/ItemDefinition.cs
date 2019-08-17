using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDefinition.asset", menuName = "Data/Item Definition")]
public class ItemDefinition : ScriptableObject
{
    [Tooltip("A good old descriptive name of the item.")]
    [SerializeField]
    protected string descriptiveName;
    public virtual string DescriptiveName => descriptiveName;

    [Tooltip("The type of item.")]
    [SerializeField]
    protected ItemType type;
    public ItemType Type => type;

    [Tooltip("Cost of the item, either the value from selling or cost of buying.")]
    [SerializeField]
    protected float cost;
    public virtual float Cost => cost;


    [Tooltip("Icon used to display the item.")]
    [SerializeField]
    protected Sprite icon;
    public Sprite Icon => icon;


    public enum ItemType
    {
        RootVegetableHarvest,
        Plant01Fruit,
        ArtichokeHarvest,
        CabbageHarvest,
        Fertiliser,
        Mulch
    }
}
