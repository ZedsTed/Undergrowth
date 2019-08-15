using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Plant : Actor
{
    [SerializeField]
    protected MeshRenderer fruit;    

    protected Landscaping landscaping;
    public Landscaping Landscaping { get { return landscaping; } set { landscaping = value; } }

    [SerializeField]
    protected PlantDefinition.LifeCycle currentLifeCycle;
    public PlantDefinition.LifeCycle CurrentLifeCycle
    {
        get { return currentLifeCycle; }
        set { currentLifeCycle = value; }
    }

    protected bool readyToHarvest = false;
    public bool ReadyToHarvest => readyToHarvest;

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

    protected float storedEnergy;
    public float StoredEnergy => storedEnergy;
   

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void OnPlaced()
    {
        base.OnPlaced();

        landscaping = GetComponentInParent<Landscaping>();
    }

    protected void Update()
    {
        //return; // DEBUG
        if (Picked)
            return;

        Photosynthesis();

        Grow();

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
        // If we have no stored energy
        if (storedEnergy <= 0f)
            return;

        // Test if we have enough to grow, if not just give us the last bit.
        float growthEnergy = storedEnergy > storedEnergy - (Definition.GrowthRate * GameConstants.Instance.GrowthMultiplier) ? (Definition.GrowthRate * GameConstants.Instance.GrowthMultiplier) : storedEnergy;

        // Add our growth energy into growth rate.
        CurrentGrowthRate = growthEnergy;

        // Deduct the energy used from our stored energy.
        storedEnergy = Mathf.Clamp01(storedEnergy - growthEnergy);

        // We work out how much we'll grow by this tick through
        // checking the energy output and multiplying by its set growth rate.
        CurrentGrowth += CurrentGrowthRate;
        CurrentGrowth = Mathf.Clamp(CurrentGrowth, 0f, Definition.MaxGrowth);
    }

    protected void Harvest(WorldNotification notification)
    {
        Debug.Log("Harvest");
        if (fruit != null)
        {
            fruit.enabled = false;
        }

        StorageManager.Instance.AddStorageItem(Definition.Harvest, 1);
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
                if (GetGrowthPercentage() > Definition.GrowthThresholds[PlantDefinition.LifeCycle.Germination])
                    CurrentLifeCycle = PlantDefinition.LifeCycle.Germination;
                break;
            case PlantDefinition.LifeCycle.Germination:
                if (GetGrowthPercentage() > Definition.GrowthThresholds[PlantDefinition.LifeCycle.Seedling])
                    CurrentLifeCycle = PlantDefinition.LifeCycle.Seedling;
                break;
            case PlantDefinition.LifeCycle.Seedling:
                if (GetGrowthPercentage() > Definition.GrowthThresholds[PlantDefinition.LifeCycle.Young])
                    CurrentLifeCycle = PlantDefinition.LifeCycle.Young;
                break;
            case PlantDefinition.LifeCycle.Young:
                if (GetGrowthPercentage() > Definition.GrowthThresholds[PlantDefinition.LifeCycle.Mature])
                {
                    CurrentLifeCycle = PlantDefinition.LifeCycle.Mature;
                    OnLifeCycleMature();
                }
                break;
        }
    }

    protected void OnLifeCycleMature()
    {
        readyToHarvest = true;
        WorldNotification notification = NotificationManager.Instance.AddNotification(gameObject);

        notification.onPressComplete += Harvest;

        if (fruit != null)
        {
            fruit.enabled = true;            
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
    /// Goes through the photosynthesis process and adds the energy amount generated this frame to the stored energy.
    /// </summary>
    protected void Photosynthesis()
    {
        energy = 0f;

        // we want to reflect how close to zero/equilibrium the sunlight and moisture levels are.


        sunlightSatisfaction = CalculateSunlightSatisfaction();
        moistureSatisfaction = CalculateMoistureSatisfaction();

        energy = EnvironmentData.Instance.
            EnvManifest.energyDefinition.Satisfaction.Evaluate(sunlightSatisfaction + moistureSatisfaction);

        storedEnergy = Mathf.Clamp01(storedEnergy + (energy * GameConstants.Instance.EnergyProductionMultiplier * Time.deltaTime));
    }

    /// <summary>
    /// Figures out how satisfied the plant is given its current sunlight amount and needs.
    /// </summary>
    /// <returns></returns>
    protected float CalculateSunlightSatisfaction()
    {
        return EnvironmentData.Instance.EnvManifest.EvaluateSunlightSatisfaction(Definition.SunlightNeed);
    }


    /// <summary>
    /// Figures out how satisfied the plant is given its current water/moistures amount and needs.
    /// </summary>
    /// <returns></returns>
    protected float CalculateMoistureSatisfaction()
    {
        return EnvironmentData.Instance.EnvManifest.EvaluateWaterSatisfaction(Definition.MoistureNeed, WaterLevel);
    }

    StringBuilder sb;
    public override string ToString()
    {
        if (sb == null)
            sb = new StringBuilder();

        sb.Clear();
        sb.AppendLine(Definition.DescriptiveName);
        sb.AppendLine(currentLifeCycle.ToString());
        sb.AppendLine("Sunlight Satisfaction: " + sunlightSatisfaction.ToString("P2"));
        sb.AppendLine("Water Satisfaction: " + moistureSatisfaction.ToString("P2"));
        sb.AppendLine("Energy: " + energy.ToString("P2"));
        sb.AppendLine("Stored Energy: " + storedEnergy.ToString("P2"));
        sb.AppendLine("Growth: " + (CurrentGrowth / Definition.MaxGrowth).ToString("P2"));
        sb.AppendLine("Growth Rate: " + (CurrentGrowthRate / ((Time.deltaTime / 60) / 60)).ToString("F2") + " per hour"); // TODO: Make me per in-game minute or something.

        return sb.ToString();
    }
}
