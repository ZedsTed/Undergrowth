using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantManifest.asset", menuName = "Data/Plant Manifest")]
public class PlantManifest : ScriptableObject
{
    public List<PlantDefinition> plantDefinitions;

    public PlantDefinition GetPlantDefinition(string name)
    {
        for (int i = plantDefinitions.Count; i-- > 0;)
        {
            if (plantDefinitions[i].DescriptiveName == name)
                return plantDefinitions[i];
        }

        return null;
    }

    public PlantDefinition GetPlantDefinition(int index)
    {
        if (index >= 0 && index < plantDefinitions.Count)
            return plantDefinitions[index];

        return null;
    }
}
