using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ContainerManifest.asset", menuName = "Data/Container Manifest")]
public class ContainerManifest : ScriptableObject
{
    public List<ContainerDefinition> containerDefinitions;

    public ContainerDefinition GetContainerDefinition(string name)
    {
        for (int i = containerDefinitions.Count; i-- > 0;)
        {
            if (containerDefinitions[i].DescriptiveName == name)
                return containerDefinitions[i];
        }

        Debug.LogError("[GetContainerDefinition] Unable to find a Container with name " + name + " in ContainerManifest");

        return null;
    }

    public ContainerDefinition GetContainerDefinition(int index)
    {
        if (index >= 0 && index < containerDefinitions.Count)
            return containerDefinitions[index];

        Debug.LogError("[GetContainerDefinition] Unable to find a Container at index " + index + " in ContainerManifest");

        return null;
    }
}
