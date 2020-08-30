using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the grid highlight visuals.
/// TODO: Allow changing of colour.
/// </summary>
public class GridHighlight : SingletonDontCreate<GridHighlight>
{
    protected bool isVisible = false;

    [SerializeField]
    protected MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetPosition(Vector3 position)
    {
        position.y += 0.025f;
        transform.position = position;
    }

    public void SetVisibility(bool visible)
    {
        if (isVisible != visible)
        {
            isVisible = visible;

            meshRenderer.enabled = visible;
        }
    }
}
