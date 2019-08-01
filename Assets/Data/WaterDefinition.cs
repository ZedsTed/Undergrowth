using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaterDefinition.asset", menuName = "Water Definition")]
public class WaterDefinition : ScriptableObject
{
    [Tooltip("Moisture/water needs..")]
    [SerializeField]
    protected Moisture need;
    public Moisture Need => need;

    [Tooltip("The satisfaction curve for water being 0 - 1.")]
    [SerializeField]
    protected AnimationCurve satisfaction;
    public AnimationCurve Satisfaction => satisfaction;

    public enum Moisture
    {
        Any,
        MoistDraining,
        PoorlyDrained,
        WellDrained
    }

}
