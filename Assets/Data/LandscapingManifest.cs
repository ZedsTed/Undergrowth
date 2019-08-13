using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LandscapingManifest.asset", menuName = "Data/Landscaping Manifest")]
public class LandscapingManifest : ScriptableObject
{
    public List<LandscapingDefinition> landscapingDefinitions;

    public LandscapingDefinition GetLandscapingDefinition(string name)
    {
        for (int i = landscapingDefinitions.Count; i-- > 0;)
        {
            if (landscapingDefinitions[i].DescriptiveName == name)
                return landscapingDefinitions[i];
        }

        return null;
    }

    public LandscapingDefinition GetLandscapingDefinition(int index)
    {
        if (index >= 0 && index < landscapingDefinitions.Count)
            return landscapingDefinitions[index];

        return null;
    }
}
