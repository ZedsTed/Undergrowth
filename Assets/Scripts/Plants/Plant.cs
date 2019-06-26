using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public enum LifeCycle
    {
        Seed,
        Germination,
        Seedling,
        Young,
        Mature,
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

    [Tooltip("How much soil is needed by the plant.")]
    public float SoilUsageNeed;

    /// <summary>
    /// How much sunlight this plant needs.
    /// </summary>
    public Sunlight SunlightNeed;

    /// <summary>
    /// 0 - 1 value for current water level.
    /// </summary>
    protected float SunlightLevel;

    /// <summary>
    /// How much moisture/water the plant needs.
    /// </summary>
    public Moisture MoistureNeed;

    /// <summary>
    /// 0 - 1 value for current water level.
    /// </summary>
    protected float WaterLevel;

    public float GrowthRate;
    public float CurrentGrowth;
    public float MaxGrowth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        // We work out how much we'll grow by this tick through
        // checking the energy output and multiplying by its set growth rate.
        CurrentGrowth = Mathf.Clamp((Photosynthesis()/100) * GrowthRate, 0f, MaxGrowth);
    }

    /// <summary>
    /// Goes through the photosynthesis process and returns the energy amount - a 0-1 value.
    /// </summary>
    /// <returns></returns>
    float Photosynthesis()
    {
        float energy = 0f;

        // we want to reflect how close to zero/equilibrium the sunlight and moisture levels are.


        float sunlightSatisfaction = CalculateSunlightSatisfaction();
        float moistureSatisfaction = CalculateMoistureSatisfaction();

        // If we're within a window of healthy satisfaction, then energy is higher.
        if (sunlightSatisfaction >= -0.05f && sunlightSatisfaction <= 0.05f)
            energy += 0.5f;
        else if (sunlightSatisfaction >= -0.15f && sunlightSatisfaction <= 0.15f)
            energy += 0.35f;
        else if (sunlightSatisfaction >= -0.25f && sunlightSatisfaction <= 0.25f)
            energy += 0.15f;
        else
            energy += 0f;


        // If we're within a window of healthy satisfaction, then things are good.
        if (moistureSatisfaction >= -0.05f && moistureSatisfaction <= 0.05f)
            energy += 0.5f;
        else if (moistureSatisfaction >= -0.15f && moistureSatisfaction <= 0.15f)
            energy += 0.35f;
        else if (moistureSatisfaction >= -0.25f && moistureSatisfaction <= 0.25f)
            energy += 0.15f;
        else
            energy += 0f;

        return energy;
    }

    /// <summary>
    /// Figures out how satisfied the plant is given its current sunlight amount and needs.
    /// </summary>
    /// <returns></returns>
    protected float CalculateSunlightSatisfaction()
    {
        float sunlightNeed = 0f;

        switch (SunlightNeed)
        {
            case Sunlight.Any:
                sunlightNeed = float.MinValue;
                break;
            case Sunlight.Full:
                sunlightNeed = 1f;
                break;
            case Sunlight.Partial:
                sunlightNeed = 0.5f;
                break;
            case Sunlight.Shade:
                sunlightNeed = 0.25f;
                break;
            default:
                break;
        }

        return SunlightLevel - sunlightNeed;        
    }


    /// <summary>
    /// Figures out how satisfied the plant is given its current water/moistures amount and needs.
    /// </summary>
    /// <returns></returns>
    protected float CalculateMoistureSatisfaction()
    {
        float moistureNeed = 0f;

        switch (MoistureNeed)
        {
            case Moisture.Any:
                moistureNeed = float.MinValue;
                break;
            case Moisture.MoistDraining:
                moistureNeed = 1f;
                break;
            case Moisture.PoorlyDrained:
                moistureNeed = 0.5f;
                break;
            case Moisture.WellDrained:
                moistureNeed = 0.25f;
                break;
            default:
                break;
        }

        return WaterLevel - moistureNeed;
    }
}
