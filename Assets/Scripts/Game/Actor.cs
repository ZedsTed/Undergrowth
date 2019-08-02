using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    protected ActorDefinition definition;
    public virtual ActorDefinition Definition { get { return definition; } set {definition = value; } }

    [Header("Highlighting")]
    [SerializeField]
    protected HighlightPlus.HighlightEffect highlight;
    public HighlightPlus.HighlightEffect Highlight => highlight;

    public Color validPlace;
    public Color invalidPlace;

    [Header("Mesh")]
    [SerializeField]
    protected GameObject mesh;
    public GameObject Mesh => mesh;

    public bool Picked { get; set; }

    public virtual bool CanBeSelected()
    { return false; }

    public virtual bool CanBePlaced()
    { return false; }

    public virtual bool CanBeDeleted()
    { return false; }

    public virtual void OnPlaced()
    {
        highlight.highlighted = false;
    }

    public virtual void OnValidPosition()
    {
        if (highlight.glowHQColor != validPlace)
            highlight.glowHQColor = validPlace;
    }

    public virtual void OnInvalidPosition()
    {
        if (highlight.glowHQColor != invalidPlace)
            highlight.glowHQColor = invalidPlace;
    }
   
    public virtual void OnSelected()
    {
        highlight.highlighted = true;
        highlight.glowHQColor = validPlace;
    }
    
    public virtual void OnDeselected()
    {
        highlight.highlighted = false;
    }

    /// <summary>
    /// Sets the layer for the gameobject and all its children to be illuminated.
    /// </summary>
    /// <param name="highlight">True to highlight it, false to revert to default layer.</param>
    public virtual void SetLayerForHighlight(bool highlight)
    {
        for (int i = gameObject.transform.childCount; i-- > 0;)
        {
            if (gameObject.transform.GetChild(i).gameObject.layer == LayerMask.NameToLayer("EditorColliders"))
                continue;                

            if (highlight)
                gameObject.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("PostProcessOutline");
            else
                gameObject.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
