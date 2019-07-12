using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Controls our toggle states and makes sure if we lose focus and we're in a tool mode, we keep focus on us.
/// </summary>
public class ToolToggle : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    public ToolsManager toolsManager;
    public Toggle toggle;

    protected void Start()
    {
        toolsManager = GetComponentInParent<ToolsManager>();
        toggle = GetComponent<Toggle>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        // If we're being selected, let the tools manager know.
        if (eventData.selectedObject == gameObject)
            toolsManager.SetSelectedTool(this);

        Debug.Log("select"+eventData.selectedObject.name);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject == gameObject)
            toolsManager.SetClickedTool(this);

        Debug.Log("click" + eventData.pointerCurrentRaycast.gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // If we're being deselected, let the tools manager know.
        if (eventData.selectedObject == gameObject)
            toolsManager.SetDeselectedTool(this);

        Debug.Log("deselect"+eventData.selectedObject.name);
    }
}
