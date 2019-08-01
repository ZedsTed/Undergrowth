using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Plant : Actor
{
    protected Landscaping landscaping;
    public Landscaping Landscaping { get { return landscaping; } set { landscaping = value; } }

    [SerializeField]
    protected PlantDefinition.LifeCycle currentLifeCycle;
    public PlantDefinition.LifeCycle CurrentLifeCycle
    {
        get { return currentLifeCycle; }
        set { currentLifeCycle = value; }
    }

    
    [Tooltip("How much soil is needed by the plant.")]
    public float SoilUsageNeed;
    
    public new PlantDefinition Definition
    { get { return definition as PlantDefinition; } set { definition = value; } }



    /// <summary>
    /// Eventually we'll need to raycast from the sun to the plant to check the actual sunlight level, but this will do for now.
    /// </summary>
    [SerializeField]
    protected float SunlightLevel => EnvironmentData.Instance.SunIntensity;

    protected float sunlightSatisfaction;
    protected float moistureSatisfaction;
    protected float energy;

    /// <summary>
    /// 0 - 1 value for current water level in the container.
    /// </summary>    
    public float WaterLevel { get { return landscaping.Water; } }

    [Tooltip("The current scaled growth rate that this plant does have.")]
    public float CurrentGrowthRate;
    [Tooltip("The current absolute growth of this plant.")]
    public float CurrentGrowth;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    public override void OnPlaced()
    {
        landscaping = GetComponentInParent<Landscaping>();        
    }

    protected void Update()
    {
        //return; // DEBUG
        if (Picked)
            return;

        CurrentGrowthRate = (Photosynthesis()) * Definition.GrowthRate * GameConstants.Instance.GrowthMultiplier * Time.deltaTime;

        // We work out how much we'll grow by this tick through
        // checking the energy output and multiplying by its set growth rate.
        CurrentGrowth += CurrentGrowthRate;
        CurrentGrowth = Mathf.Clamp(CurrentGrowth, 0f, Definition.MaxGrowth);

        UpdateCurrentLifeCycle();

        float normalizedGrowth = GetGrowthPercentage() / 100;

        mesh.transform.localScale = new Vector3(normalizedGrowth, normalizedGrowth, normalizedGrowth);
    }
    protected void FixedUpdate()
    {

    }

    public void Sow()
    {
        currentLifeCycle = PlantDefinition.LifeCycle.Seed;
    }

    protected void Grow()
    {

    }

    /// <summary>
    /// Looks at the current growth amount, the growth percentage threshold to 'level up' a life cycle, and returns the new/current life cycle.
    /// </summary>
    /// <returns></returns>
    protected void UpdateCurrentLifeCycle()
    {
        switch (currentLifeCycle)
        {
            case PlantDefinition.LifeCycle.Seed:
                if (GetGrowthPercentage() > Definition.GrowthThresholds[PlantDefinition.LifeCycle.Seed])
                    CurrentLifeCycle = PlantDefinition.LifeCycle.Germination;
                break;
            case PlantDefinition.LifeCycle.Germination:
                if (GetGrowthPercentage() > Definition.GrowthThresholds[PlantDefinition.LifeCycle.Germination])
                    CurrentLifeCycle = PlantDefinition.LifeCycle.Seedling;
                break;
            case PlantDefinition.LifeCycle.Seedling:
                if (GetGrowthPercentage() > Definition.GrowthThresholds[PlantDefinition.LifeCycle.Seedling])
                    CurrentLifeCycle = PlantDefinition.LifeCycle.Young;
                break;
            case PlantDefinition.LifeCycle.Young:
                if (GetGrowthPercentage() > Definition.GrowthThresholds[PlantDefinition.LifeCycle.Young])
                    CurrentLifeCycle = PlantDefinition.LifeCycle.Mature;
                break;           
        }
    }

    /// <summary>
    /// Gets the percentage representing the current growth / max growth.
    /// </summary>
    /// <returns>0 - 100 value representing actual percentage.</returns>
    protected float GetGrowthPercentage()
    {
        if (Definition.MaxGrowth == 0f)
            return 0f;

        return (CurrentGrowth / Definition.MaxGrowth) * 100;
    }

    /// <summary>
    /// Goes through the photosynthesis process and returns the energy amount - a 0-1 value.
    /// </summary>
    /// <returns></returns>
    float Photosynthesis()
    {
        energy = 0f;

        // we want to reflect how close to zero/equilibrium the sunlight and moisture levels are.


        sunlightSatisfaction = CalculateSunlightSatisfaction();
        moistureSatisfaction = CalculateMoistureSatisfaction();

        energy = EnvironmentData.Instance.
            EnvManifest.energyDefinition.Satisfaction.Evaluate(sunlightSatisfaction + moistureSatisfaction);

        //// If we're within a window of healthy satisfaction, then energy is higher.
        //if (sunlightSatisfaction >= -0.05f && sunlightSatisfaction <= 0.05f)
        //    energy += 0.5f;
        //else if (sunlightSatisfaction >= -0.15f && sunlightSatisfaction <= 0.15f)
        //    energy += 0.35f;
        //else if (sunlightSatisfaction >= -0.25f && sunlightSatisfaction <= 0.25f)
        //    energy += 0.15f;
        //else
        //    energy += 0f;


        //// If we're within a window of healthy satisfaction, then things are good.
        //if (moistureSatisfaction >= -0.05f && moistureSatisfaction <= 0.05f)
        //    energy += 0.5f;
        //else if (moistureSatisfaction >= -0.15f && moistureSatisfaction <= 0.15f)
        //    energy += 0.35f;
        //else if (moistureSatisfaction >= -0.25f && moistureSatisfaction <= 0.25f)
        //    energy += 0.15f;
        //else
        //    energy += 0f;

        return energy;
    }

    /// <summary>
    /// Figures out how satisfied the plant is given its current sunlight amount and needs.
    /// </summary>
    /// <returns></returns>
    protected float CalculateSunlightSatisfaction()
    {
        //float sunlightNeed = 0f;

        return EnvironmentData.Instance.EnvManifest.EvaluateSunlightSatisfaction(Definition.SunlightNeed);

        //switch (Definition.SunlightNeed)
        //{
        //    case SunlightManifest.Sunlight.Any:
        //        //EnvironmentData.SunManifest.Needs;
        //        break;
        //    case SunlightManifest.Sunlight.Full:
        //        sunlightNeed = 1f;
        //        break;
        //    case SunlightManifest.Sunlight.Partial:
        //        sunlightNeed = 0.5f;
        //        break;
        //    case SunlightManifest.Sunlight.Shade:
        //        sunlightNeed = 0.25f;
        //        break;
        //    default:
        //        break;
        //}

        //return SunlightLevel - sunlightNeed;        
    }


    /// <summary>
    /// Figures out how satisfied the plant is given its current water/moistures amount and needs.
    /// </summary>
    /// <returns></returns>
    protected float CalculateMoistureSatisfaction()
    {
        return EnvironmentData.Instance.EnvManifest.EvaluateWaterSatisfaction(Definition.MoistureNeed, WaterLevel);

        //float moistureNeed = 0f;

        //switch (Definition.MoistureNeed)
        //{
        //    case PlantDefinition.Moisture.Any:
        //        moistureNeed = float.MinValue;
        //        break;
        //    case PlantDefinition.Moisture.MoistDraining:
        //        moistureNeed = 1f;
        //        break;
        //    case PlantDefinition.Moisture.PoorlyDrained:
        //        moistureNeed = 0.5f;
        //        break;
        //    case PlantDefinition.Moisture.WellDrained:
        //        moistureNeed = 0.25f;
        //        break;
        //    default:
        //        break;
        //}

        //return WaterLevel - moistureNeed;
    }

    StringBuilder sb;
    public override string ToString()
    {
        if (sb == null)
            sb = new StringBuilder();

        sb.Clear();
        sb.AppendLine(Definition.DescriptiveName);
        sb.AppendLine(currentLifeCycle.ToString());
        sb.AppendLine("Sunlight Satisfaction: " + sunlightSatisfaction);
        sb.AppendLine("Water Satisfaction: " + moistureSatisfaction);
        sb.AppendLine("Energy: " + energy);
        sb.AppendLine("Growth: " + (CurrentGrowth / Definition.MaxGrowth).ToString("P1"));
        sb.AppendLine("Growth Rate: " + CurrentGrowthRate); // TODO: Make me per in-game minute or something.

        return sb.ToString();
    }
}
