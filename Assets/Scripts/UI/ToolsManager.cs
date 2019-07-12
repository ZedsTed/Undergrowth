using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolsManager : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    public ToggleGroup ToolsGroup;

    public Toggle ContainerToggle;
    public Toggle LandscapingToggle;
    public Toggle PlantToggle;
    public Toggle WaterToggle;
    public Toggle RemoveToggle;


    protected void Start()
    {
        eventSystem = EventSystem.current;

        ContainerToggle.onValueChanged.AddListener(delegate
        {
            OnToggleChanged(ContainerToggle);
        });

        LandscapingToggle.onValueChanged.AddListener(delegate
        {
            OnToggleChanged(LandscapingToggle);
        });

        PlantToggle.onValueChanged.AddListener(delegate
        {
            OnToggleChanged(PlantToggle);
        });

        WaterToggle.onValueChanged.AddListener(delegate
        {
            OnToggleChanged(WaterToggle);
        });

        RemoveToggle.onValueChanged.AddListener(delegate
        {
            OnToggleChanged(RemoveToggle);
        });
    }

    protected void Update()
    {
        
    }

    EventSystem eventSystem;
    GameObject selection;
    protected void OnToggleChanged(Toggle toggle)
    {
        if (eventSystem.currentSelectedGameObject != null && eventSystem.currentSelectedGameObject != selection)
            selection = eventSystem.currentSelectedGameObject;
        else if (selection != null && eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(selection);

        switch (toggle.name)
        {
            case "Container":
                ConstructionEditor.Instance.SetConstructionMode(ConstructionEditor.ConstructionState.Containering);
                break;
            case "Landscaping":
                ConstructionEditor.Instance.SetConstructionMode(ConstructionEditor.ConstructionState.Landscaping);
                break;
            case "Plant":
                ConstructionEditor.Instance.SetConstructionMode(ConstructionEditor.ConstructionState.Planting);
                break;
            case "Water":
                ConstructionEditor.Instance.SetConstructionMode(ConstructionEditor.ConstructionState.Watering);
                break;
            case "Remove":
                ConstructionEditor.Instance.SetConstructionMode(ConstructionEditor.ConstructionState.Removing);
                break;
            default:
                ConstructionEditor.Instance.SetConstructionMode(ConstructionEditor.ConstructionState.None);
                break;
        }

    }

    public void OnSelect(BaseEventData eventData)
    {

    }

    public void OnDeselect(BaseEventData eventData)
    {
       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventSystem.currentSelectedGameObject != null && eventSystem.currentSelectedGameObject != selection)
            selection = eventSystem.currentSelectedGameObject;
        else if (IsToolToggle(eventData.selectedObject) && eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(eventData.selectedObject);
    }

    /// <summary>
    /// Checks whether the gameobject passed is one of the tool toggle objects.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    protected bool IsToolToggle(GameObject gameObject)
    {
        switch (gameObject.name)
        {
            case "Container":
                return true;
            case "Landscaping":
                return true;
            case "Plant":
                return true;
            case "Water":
                return true;
            case "Remove":
                return true;
            default:
                return false;
        }
    }
}
