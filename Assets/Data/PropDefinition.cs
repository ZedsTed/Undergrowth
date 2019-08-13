using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PropDefinition.asset", menuName = "Data/Prop Definition")]
public class PropDefinition : ActorDefinition
{
    public Prop Actor => actor as Prop;
}
