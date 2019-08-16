using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnergyDefinition.asset", menuName = "Data/Energy Definition")]
public class EnergyDefinition : ScriptableObject
{
    [Tooltip("The energy creation according to water and sun satisfaction.")]
    [SerializeField]
    protected AnimationCurve satisfaction;
    public AnimationCurve Satisfaction => satisfaction;
}
