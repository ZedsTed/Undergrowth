﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantDefinition.asset", menuName = "Plant Definition")]
public class PlantDefinition : ActorDefinition
{
    public Plant Actor => actor as Plant;

    /// <summary>
    /// How much sunlight this plant needs.
    /// </summary>
    [Header("Sunlight Needs")]
    [Tooltip("How much Sunlight this plant needs.")]
    [SerializeField]
    protected Sunlight sunlightNeed;
    public Sunlight SunlightNeed => sunlightNeed; 

    /// <summary>
    /// How much moisture/water the plant needs.
    /// </summary>
    [Header("Water Needs")]
    [Tooltip("How much Water this plant needs.")]
    [SerializeField]
    protected Moisture moistureNeed;
    public Moisture MoistureNeed => moistureNeed;

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
        [LifeCycle.Seedling] = 20,
        [LifeCycle.Young] = 40,
        [LifeCycle.Mature] = 80
    };


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
        Sporing, // Eventually we will have a base life cycle and then Flowering/Spore on top.
        Pollinated,
        Seeding
    }

    public enum Sunlight
    {
        Any,
        Full,
        Partial,
        Shade
    }

    public enum Soil
    {
        Any,
        Chalk,
        Clay,
        Loam,
        Sand
    }

    public enum Moisture
    {
        Any,
        MoistDraining,
        PoorlyDrained,
        WellDrained
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
