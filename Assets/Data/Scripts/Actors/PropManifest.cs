using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PropManifest.asset", menuName = "Data/Prop Manifest")]
public class PropManifest : ScriptableObject
{
    public List<PropDefinition> propDefinitions;

    public PropDefinition GetPropDefinition(string name)
    {
        for (int i = propDefinitions.Count; i-- > 0;)
        {
            if (propDefinitions[i].DescriptiveName == name)
                return propDefinitions[i];
        }

        return null;
    }

    public PropDefinition GetPropDefinition(int index)
    {
        if (index >= 0 && index < propDefinitions.Count)
            return propDefinitions[index];

        return null;
    }
}
