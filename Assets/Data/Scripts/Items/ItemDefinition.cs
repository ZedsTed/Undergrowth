using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ItemDefinition.asset", menuName = "Data/Item Definition")]
public class ItemDefinition : ScriptableObject
{
    [BoxGroup("Basic Info")]
    [LabelWidth(120)]
    [Tooltip("A good old descriptive name of the item.")]
    [SerializeField]
    protected string descriptiveName;
    public virtual string DescriptiveName => descriptiveName;

    [Tooltip("A good old descriptive name of the item.")]
    [SerializeField]
    [LabelWidth(100)]
    [TextArea]
    [BoxGroup("Basic Info")]
    protected string description;
    public virtual string Description => description;

    [Tooltip("Cost of the item, either the value from selling or cost of buying.")]
    [SerializeField]
    [LabelWidth(40)]
    [BoxGroup("Basic Info")]
    protected float cost;
    public virtual float Cost => cost;

    [BoxGroup("Basic Info/Actor Definition")]
    [SerializeField]
    [HideLabel]
    protected ActorDefinition actorDefinition;
    public ActorDefinition ActorDefinition => actorDefinition;


    [BoxGroup("Basic Info/Type")]
    [Tooltip("The type of item.")]
    [SerializeField]
    [EnumToggleButtons]
    [HideLabel]
    protected ItemType type;
    public ItemType Type => type;

    [BoxGroup("Basic Info/Type")]
    [Tooltip("The sub-type of item.")]
    [SerializeField]
    [EnumToggleButtons]
    [HideLabel]
    [Space]
    protected SubItemType subType;
    public SubItemType SubType => subType;


    [BoxGroup("Basic Info/Icon")]
    [Tooltip("Icon used to display the item.")]
    [SerializeField]
    [PreviewField(75)]
    [HideLabel]
    protected Sprite icon;
    public Sprite Icon => icon;

   
  

    public enum ItemType
    {
        RootVegetableHarvest,
        Plant01Fruit,
        ArtichokeHarvest,
        CabbageHarvest,
        Fertiliser,
        Mulch,
        RootVegetableSeed,
        Plant01Seed,
        ArtichokeSeed,
        CabbageSeed
    }

    public enum SubItemType
    {
        None,
        Seed,
        Harvest
    }
}
