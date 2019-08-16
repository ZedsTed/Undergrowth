using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorDefinition : ScriptableObject
{
    [Header("Actor")]
    [Tooltip("Our visual representation of the actor definition.")]
    [SerializeField]
    protected Actor actor;

    [Tooltip("A good old descriptive name of the actor.")]
    [SerializeField]
    protected string descriptiveName;
    public virtual string DescriptiveName => descriptiveName;

    [Tooltip("Cost of the actor.")]
    [SerializeField]
    protected float cost;
    public virtual float Cost => cost;

}
