using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    protected ActorDefinition definition;
    public virtual ActorDefinition Definition { get { return definition; } set { definition = value; } }

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

    Vector3 previousLerpedSize;
    public void SetScale(Vector3 size)
    {
        Transform t;

        for (int i = transform.childCount; i-- > 0;)
        {
            t = transform.GetChild(i);

            BoxCollider c = t.gameObject.GetComponent<BoxCollider>();

            if (c != null)
            {
                // Because the collider size will be specified as a specific value, whereas the rest of the actor - especially the mesh
                // will have transform.localScale of 1 to start, we only want to figure out how much we should scale the collider by.
                // e.g. if we had a mesh that was scale.y of 1, then it needed to be made a scale.y of 1.5, that's a 1.5x.
                // if our collider was size.y of 0.35, the yMultiplier of that would be 1.5 and we'd get a (0.35 * 1.5) size.y value as
                // our new collider vertical size.
                Vector3 colSize = size;
                float yMultiplier = colSize.y;
                colSize.y = yMultiplier * c.size.y;
                c.size = colSize;

                continue;
            }

            previousLerpedSize = Vector3.Lerp(previousLerpedSize, (size - t.localScale) * 0.7f, 30f *
                       Time.unscaledDeltaTime);

            t.localScale += previousLerpedSize;
        }
    }
}
