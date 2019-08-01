using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentManifest.asset", menuName = "Environment Manifest")]
public class EnvironmentManifest : ScriptableObject
{
    #region Sunlight Satisfaction
    [Tooltip("All eligible sunlight needs.")]
    [SerializeField]
    protected List<SunlightDefinition> sunlightDefinitions;
    public List<SunlightDefinition> SunlightDefinitions => sunlightDefinitions;

    public SunlightDefinition GetSunlightDefinition(string name)
    {
        for (int i = sunlightDefinitions.Count; i-- > 0;)
        {
            if (sunlightDefinitions[i].DescriptiveName == name)
                return sunlightDefinitions[i];
        }

        Debug.LogError("[SunlightManifest] Unable to find sunlight definition: " + name);
        return null;
    }

    public SunlightDefinition GetSunlightDefinition(SunlightDefinition.Sunlight sun)
    {
        for (int i = sunlightDefinitions.Count; i-- > 0;)
        {
            if (sunlightDefinitions[i].Need == sun)
                return sunlightDefinitions[i];
        }

        Debug.LogError("[SunlightManifest] Unable to find sunlight definition: " + sun.ToString());
        return null;
    }

    public AnimationCurve GetSunlightSatisfactionCurve(SunlightDefinition.Sunlight sun)
    {
        for (int i = sunlightDefinitions.Count; i-- > 0;)
        {
            if (sunlightDefinitions[i].Need == sun)
                return sunlightDefinitions[i].Satisfaction;
        }

        Debug.LogError("[SunlightManifest] Unable to find sunlight definition: " + sun.ToString());
        return null;
    }

    /// <summary>
    /// Evaluates the sunlight satisfaction of a sunlight definition for the given sunlight type.
    /// </summary>
    public float EvaluateSunlightSatisfaction(SunlightDefinition.Sunlight sun)
    {
        for (int i = sunlightDefinitions.Count; i-- > 0;)
        {
            if (sunlightDefinitions[i].Need == sun)
                return sunlightDefinitions[i].Satisfaction.Evaluate(EnvironmentData.Instance.SunIntensity);
        }

        Debug.LogError("[SunlightManifest] Unable to find sunlight definition: " + sun.ToString());
        return 0f;
    }

    #endregion

    #region Water Satisfaction
    [Tooltip("All eligible water needs.")]
    [SerializeField]
    protected List<WaterDefinition> waterDefinitions;
    public List<WaterDefinition> WaterDefinitions => waterDefinitions;


    public WaterDefinition GetWaterDefinition(WaterDefinition.Moisture water)
    {
        for (int i = waterDefinitions.Count; i-- > 0;)
        {
            if (waterDefinitions[i].Need == water)
                return waterDefinitions[i];
        }

        Debug.LogError("[WaterManifest] Unable to find water definition: " + water.ToString());
        return null;
    }

    public AnimationCurve GetWaterSatisfactionCurve(WaterDefinition.Moisture water)
    {
        for (int i = waterDefinitions.Count; i-- > 0;)
        {
            if (waterDefinitions[i].Need == water)
                return waterDefinitions[i].Satisfaction;
        }

        Debug.LogError("[WaterManifest] Unable to find water definition: " + water.ToString());
        return null;
    }

    /// <summary>
    /// Evaluates the water satisfaction of a water definition for the given water type.
    /// </summary>
    public float EvaluateWaterSatisfaction(WaterDefinition.Moisture water, float waterLevel)
    {
        for (int i = waterDefinitions.Count; i-- > 0;)
        {
            if (waterDefinitions[i].Need == water)
                return waterDefinitions[i].Satisfaction.Evaluate(waterLevel);
        }

        Debug.LogError("[WaterManifest] Unable to find water definition: " + water.ToString());
        return 0f;
    }

    #endregion

    public EnergyDefinition energyDefinition;
}
