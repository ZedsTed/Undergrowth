using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
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

    public float GrowthRate;
    public float CurrentGrowth;
    public float MaxGrowth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
