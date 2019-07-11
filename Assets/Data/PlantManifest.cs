using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantManifest.asset", menuName = "Plant Manifest")]
public class PlantManifest : ScriptableObject
{
    public List<PlantDefinition> plantDefinitions;

    public PlantDefinition GetPlantDefinition(string name)
    {
        for (int i = plantDefinitions.Count; i--> 0;)
        {
            if (plantDefinitions[i].PlantName == name)
                return plantDefinitions[i];
        }

        return null;
    }
}
