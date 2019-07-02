﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    protected Container plantBed;
    public Container PlantBed { get { return plantBed; } set { plantBed = value; } }

    [SerializeField]
    protected PlantDefinition.LifeCycle currentLifeCycle;
    public PlantDefinition.LifeCycle CurrentLifeCycle
    {
        get { return currentLifeCycle; }
        set { currentLifeCycle = value; }
    }

    
    [Tooltip("How much soil is needed by the plant.")]
    public float SoilUsageNeed;

    protected PlantDefinition definition;
    public PlantDefinition Definition { get { return definition; } set { definition = value; } }

    [SerializeField]
    protected float sunlightLevel;

    /// <summary>
    /// 0 - 1 value for current water level.
    /// </summary>
    [SerializeField]
    protected float waterLevel;

    [Tooltip("The default optimal growth rate that this plant should have.")]
    public float GrowthRate;
    [Tooltip("The current scaled growth rate that this plant does have.")]
    public float CurrentGrowthRate;
    [Tooltip("The current absolute growth of this plant.")]
    public float CurrentGrowth;
    [Tooltip("The maximum growth that this plant should have.")]
    public float MaxGrowth;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected void Update()
    {
        float normalizedGrowth = GetGrowthPercentage() / 100;

        transform.localScale = new Vector3(normalizedGrowth, normalizedGrowth, normalizedGrowth);
    }
    protected void FixedUpdate()
    {
        CurrentGrowthRate = (Photosynthesis() / 100) * GrowthRate;

        // We work out how much we'll grow by this tick through
        // checking the energy output and multiplying by its set growth rate.
        CurrentGrowth += CurrentGrowthRate;
        CurrentGrowth = Mathf.Clamp(CurrentGrowth, 0f, MaxGrowth);

        UpdateCurrentLifeCycle();
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
                if (GetGrowthPercentage() > definition.GrowthThresholds[PlantDefinition.LifeCycle.Seed])
                    CurrentLifeCycle = PlantDefinition.LifeCycle.Germination;
                break;
            case PlantDefinition.LifeCycle.Germination:
                if (GetGrowthPercentage() > definition.GrowthThresholds[PlantDefinition.LifeCycle.Germination])
                    CurrentLifeCycle = PlantDefinition.LifeCycle.Seedling;
                break;
            case PlantDefinition.LifeCycle.Seedling:
                if (GetGrowthPercentage() > definition.GrowthThresholds[PlantDefinition.LifeCycle.Seedling])
                    CurrentLifeCycle = PlantDefinition.LifeCycle.Young;
                break;
            case PlantDefinition.LifeCycle.Young:
                if (GetGrowthPercentage() > definition.GrowthThresholds[PlantDefinition.LifeCycle.Young])
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
        if (MaxGrowth == 0f)
            return 0f;

        return (CurrentGrowth / MaxGrowth) * 100;
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

        switch (Definition.SunlightNeed)
        {
            case PlantDefinition.Sunlight.Any:
                sunlightNeed = float.MinValue;
                break;
            case PlantDefinition.Sunlight.Full:
                sunlightNeed = 1f;
                break;
            case PlantDefinition.Sunlight.Partial:
                sunlightNeed = 0.5f;
                break;
            case PlantDefinition.Sunlight.Shade:
                sunlightNeed = 0.25f;
                break;
            default:
                break;
        }

        return sunlightLevel - sunlightNeed;        
    }


    /// <summary>
    /// Figures out how satisfied the plant is given its current water/moistures amount and needs.
    /// </summary>
    /// <returns></returns>
    protected float CalculateMoistureSatisfaction()
    {
        float moistureNeed = 0f;

        switch (definition.MoistureNeed)
        {
            case PlantDefinition.Moisture.Any:
                moistureNeed = float.MinValue;
                break;
            case PlantDefinition.Moisture.MoistDraining:
                moistureNeed = 1f;
                break;
            case PlantDefinition.Moisture.PoorlyDrained:
                moistureNeed = 0.5f;
                break;
            case PlantDefinition.Moisture.WellDrained:
                moistureNeed = 0.25f;
                break;
            default:
                break;
        }

        return waterLevel - moistureNeed;
    }
}