using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ContainerManifest.asset", menuName = "Container Manifest")]
public class ContainerManifest : ScriptableObject
{
    public List<ContainerDefinition> containerDefinitions;
    
    public ContainerDefinition GetContainerDefinition(string name)
    {
        for (int i = containerDefinitions.Count; i-- > 0;)
        {
            if (containerDefinitions[i].ContainerName == name)
                return containerDefinitions[i];
        }

        return null;
    }
}
