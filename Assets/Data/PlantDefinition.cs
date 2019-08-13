using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantDefinition.asset", menuName = "Data/Plant Definition")]
public class PlantDefinition : ActorDefinition
{
    public Plant Actor => actor as Plant;

    /// <summary>
    /// How much sunlight this plant needs.
    /// </summary>
    [Header("Sunlight Needs")]
    [Tooltip("How much Sunlight this plant needs.")]
    [SerializeField]
    protected SunlightDefinition.Sunlight sunlightNeed;
    public SunlightDefinition.Sunlight SunlightNeed => sunlightNeed; 

    /// <summary>
    /// How much moisture/water the plant needs.
    /// </summary>
    [Header("Water Needs")]
    [Tooltip("How much Water this plant needs.")]
    [SerializeField]
    protected WaterDefinition.Moisture moistureNeed;
    public WaterDefinition.Moisture MoistureNeed => moistureNeed;

    /// <summary>
    /// How fast this plant grows in optimal conditions for it.
    /// </summary>
    [Header("Optimal Growth Rate")]
    [Tooltip("Optimal growth rate for the plant in ideal conditions.")]
    [SerializeField]
    protected float growthRate;
    public float GrowthRate => growthRate;

    /// <summary>
    /// The maximum growth value for this plant, perhaps in meters, we'll figure it out later.
    /// </summary>
    [Header("Maximum Growth")]
    [Tooltip("Float value for maximum growth this plant can reach.")]
    [SerializeField]
    protected float maxGrowth;
    public float MaxGrowth => maxGrowth;

    [Header("Growth Thresholds")]
    [Tooltip("Percentage of growth for each life cycle stage to be achieved.")]
    [SerializeField]
    public Dictionary<LifeCycle, int> GrowthThresholds = new Dictionary<LifeCycle, int>
    {
        [LifeCycle.Seed] = 0,
        [LifeCycle.Germination] = 4,
        [LifeCycle.Seedling] = 8,
        [LifeCycle.Young] = 20,
        [LifeCycle.Mature] = 80
    };


    // Probably more complex than needed atm.
    //[Header("Reproduction Thresholds")]
    //[Tooltip("Percentage of reproduction growth it takes to reach each threshold.")]
    //public Dictionary<ReproductionCycle, int> ReproductionThresholds = new Dictionary<ReproductionCycle, int>
    //{
    //    [ReproductionCycle.Flowering] = 0, // At this point the flower is growing or grown.
    //    [ReproductionCycle.Pollinated] = 25, // At this point the flower is pollinated (has been around long enough)
    //    [ReproductionCycle.Seeding] = 80 // At this point it's basically fruiting
    //};

    #region
    public enum LifeCycle
    {
        Seed,
        Germination,
        Seedling,
        Young,
        Mature
    }

    public enum ReproductionCycle
    {
        Flowering, // Plant will either Flower or Spore
        // Sporing, // Eventually we will have a base life cycle and then Flowering/Spore on top.
        Pollinated,
        Seeding
    }

    public enum Soil
    {
        Any,
        Chalk,
        Clay,
        Loam,
        Sand
    }

    public enum Ph
    {
        Any,
        Acid,
        Alkaline,
        Neutral
    }

    public enum Exposure
    {
        Any,
        Exposed,
        Sheltered
    }

    public enum Hardiness
    {
        Any,
        H1A,    // Under glass all year (15C)
        H1B,    // Can be grown outside in the summer (10C - 15C)
        H1C,    // Can be grown outside in the summer
        H2,     // Tolerant of low temperatures, but not surviving being (1 to 5)
        H3,     // Hardy in coastal and relatively mild parts of the UK (-5 to 1)
        H4,     // Hardy through most of the UK (-10 to -5)
        H5,     // Hardy in most places throughout the UK even in severe (-15 to -10)
        H6,     // Hardy in all of UK and northern Europe (-20 to -15)
        H7      // Hardy in the severest European continental climates (-20)
    }

    public enum Foliage
    {
        Evergreen,
        SemiEvergreen,
        Deciduous
    }
    #endregion
}
