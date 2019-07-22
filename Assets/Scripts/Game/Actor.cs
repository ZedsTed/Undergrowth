using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public bool Picked { get; set; }

    public virtual bool CanBeSelected()
    { return false; }

    public virtual bool CanBePlaced()
    { return false; }

    public virtual bool CanBeDeleted()
    { return false; }

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
