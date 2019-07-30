using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LandscapingDefinition.asset", menuName = "Landscaping Definition")]
public class LandscapingDefinition : ScriptableObject
{
    [Header("Actor")]
    [Tooltip("Our visual representation of the landscaping definition.")]
    [SerializeField]
    protected Landscaping actor;
    public Landscaping Actor => actor;

    [Tooltip("Name of our landscaping.")]
    [SerializeField]
    protected string landscapingName;
    public string LandscapingName => landscapingName;
}
