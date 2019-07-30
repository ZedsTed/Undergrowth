using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ContainerDefinition.asset", menuName = "Container Definition")]
public class ContainerDefinition : ScriptableObject
{
    [Header("Actor")]
    [Tooltip("Our visual representation of the container definition.")]
    [SerializeField]
    protected Container actor;
    public Container Actor => actor;

    [Tooltip("Name of our container.")]
    [SerializeField]
    protected string containerName;
    public string ContainerName => containerName;



    [Header("Size")]
    [Tooltip("Size of the container - external.")]
    [SerializeField]
    protected Vector3 containerSize;
    public Vector3 ContainerSize => containerSize;
    

    [Tooltip("The size of the soil in the container - internal.")]
    [SerializeField]
    protected Vector3 containerSoilSize;
    public Vector3 ContainerSoilSize => containerSoilSize;

    [Tooltip("The offset of the soil in the container - internal.")]
    [SerializeField]
    protected Vector3 containerSoilOffset;
    public Vector3 ContainerSoilOffset => containerSoilOffset;
}
