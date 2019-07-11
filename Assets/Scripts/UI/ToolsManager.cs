using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolsManager : MonoBehaviour
{
    public Toggle ContainerToggle;
    public Toggle LandscapingToggle;
    public Toggle PlantToggle;
    public Toggle WaterToggle;
    public Toggle RemoveToggle;


    protected void Start()
    {
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
    
    protected void OnToggleChanged(Toggle toggle)
    {
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

}
