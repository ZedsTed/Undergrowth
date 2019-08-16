using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LandscapingDefinition.asset", menuName = "Data/Landscaping Definition")]
public class LandscapingDefinition : ActorDefinition
{
    public Landscaping Actor => actor as Landscaping;    

    [SerializeField]
    public AnimationCurve drainageProfile;
}
