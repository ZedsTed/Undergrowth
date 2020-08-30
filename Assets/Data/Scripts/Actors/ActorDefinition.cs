using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ActorDefinition : SerializedScriptableObject
{
    [BoxGroup("Actor Info")]
    [Tooltip("Our visual representation of the actor definition.")]
    [SerializeField]
    protected Actor actor;

    [BoxGroup("Actor Info")]
    [Tooltip("A good old descriptive name of the actor.")]
    [SerializeField]
    protected string descriptiveName;
    public virtual string DescriptiveName => descriptiveName;

    [BoxGroup("Actor Info")]
    [Tooltip("Cost of the actor.")]
    [SerializeField]
    protected float cost;
    public virtual float Cost => cost;

}
