using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the data for the weather, sun, etc.
/// </summary>
public class EnvironmentData : SingletonDontCreate<EnvironmentData>
{
    [SerializeField]
    protected SunCycle Sun;
    
    public float SunIntensity { get { return Sun.sun.intensity; } }
}
