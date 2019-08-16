using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SunlightDefinition.asset", menuName = "Data/Sunlight Definition")]
public class SunlightDefinition : ScriptableObject
{    
    [Tooltip("A good old descriptive name of the actor.")]
    [SerializeField]
    protected string descriptiveName;
    public virtual string DescriptiveName => descriptiveName;

    [Tooltip("A good old descriptive name of the actor.")]
    [SerializeField]
    protected Sunlight need;
    public Sunlight Need => need;

    [Tooltip("The satisfaction curve for sunlight being 0 - 1.")]
    [SerializeField]
    protected AnimationCurve satisfaction;
    public AnimationCurve Satisfaction => satisfaction;

    public enum Sunlight
    {
        Any,
        Full,
        Partial,
        Shade
    }
}
