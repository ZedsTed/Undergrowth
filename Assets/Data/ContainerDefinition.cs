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
    [Tooltip("Width of the bed")]
    public float Width;
    [Tooltip("Length of the bed")]
    public float Length;
    [Tooltip("Depth of the bed")]
    public float Depth;
}
