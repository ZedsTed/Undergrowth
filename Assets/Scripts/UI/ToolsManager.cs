using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ToolsManager : SingletonDontCreate<ToolsManager>
{
    public ToggleGroup ToolsGroup;

    /// <summary>
    /// Clicked tool corresponds to the tool/toggle the player has most recently click on.
    /// We track it so we can tell if they're clicking an already selected tool/toggle.
    /// </summary>
    [SerializeField]
    protected ToolToggle clickedTool;

    /// <summary>
    /// Selected tool is the most recently selected toggle.
    /// We track it to test against clickedTool for >1 clicks on the same toggle.
    /// </summary>
    [SerializeField]
    protected ToolToggle selectedTool;
    public ToolToggle SelectedTool => selectedTool;

    /// <summary>
    /// Deselected tool is the most recently deselected toggle.
    /// In cases where the player selects another toggle, this is the previously selected one. 
    /// Additionally, it can be the same as selectedTool in cases where the player clicks away from the toggleGroup/toolsUI.
    /// </summary>
    [SerializeField]
    protected ToolToggle deselectedTool;

    public Toggle ContainerToggle;
    public Toggle LandscapingToggle;
    public Toggle PlantToggle;
    public Toggle WaterToggle;
    public Toggle RemoveToggle;



    protected SelectableList panel;

    /// <summary>
    /// Lock Toggle On means if the player selects outside of the toggle ui, we want to lock the deselected toggle on in the Update method.
    /// </summary>
    [SerializeField]
    protected bool lockToggleOn = false;


    protected void Start()
    {
       
    }

    protected void Update()
    {
        if (lockToggleOn && selectedTool != null && !selectedTool.toggle.isOn)
        {
            selectedTool.toggle.SetIsOnWithoutNotify(true);
        }

        if (panel != null && selectedTool != null)
        {
            if (!selectedTool.toggle.isOn)
                DespawnSelectableList();

            if (!ToolsGroup.AnyTogglesOn())
                DespawnSelectableList();
        }
    }
 

    protected void SetContructionEditorMode(Toggle toggle)
    {
        switch (toggle.name)
        {
            case "Prop":
            case "Container":
            case "Landscaping":
            case "Plant":
                ConstructionEditor.Instance.SetConstructionMode(ConstructionEditor.ConstructionState.Placing);                
                SpawnSelectableList(toggle);
                break;
            case "Water":
                ConstructionEditor.Instance.SetConstructionMode(ConstructionEditor.ConstructionState.Watering);
                DespawnSelectableList();
                break;
            case "Remove":
                ConstructionEditor.Instance.SetConstructionMode(ConstructionEditor.ConstructionState.Removing);
                DespawnSelectableList();
                break;
            default:
                ConstructionEditor.Instance.SetConstructionMode(ConstructionEditor.ConstructionState.None);
                DespawnSelectableList();
                break;
        }
    }

    public void SetClickedTool(ToolToggle tool)
    {
        if (clickedTool == tool)
        {
            OnToolMultipleClick(tool);
        }

        clickedTool = tool;
    }

    public void SetSelectedTool(ToolToggle tool)
    {
        selectedTool = tool;

        if (selectedTool != deselectedTool)
        {
            lockToggleOn = false;
        }

        SetContructionEditorMode(tool.toggle);
    }

    public void SetDeselectedTool(ToolToggle tool)
    {
        deselectedTool = tool;

        if (selectedTool == deselectedTool)
            OnNonToolClick();
    }

    public void OnSelectableListClicked()
    {
        //Debug.Log("OnSelectableListClicked");
        lockToggleOn = true;
    }

    /// <summary>
    /// Spawns our selectable list for selecting the placement objects.
    /// </summary>
    /// <param name="tool"></param>
    protected void SpawnSelectableList(Toggle toggle)
    {
        // If we already have a panel spawned and it's parented to the selected toggle, return
        if (panel != null && panel.transform.parent == toggle.transform)
            return;
        else if (panel != null) // otherwise, destroy it and we'll spawn our new one.
            DespawnSelectableList();


        if (panel == null)
        {
            panel = Instantiate(Resources.Load("Prefabs/UI/HorizontalList") as GameObject, toggle.transform).GetComponent<SelectableList>();

            switch (selectedTool.name)
            {
                case "Prop":
                    SetupPropSelectableList();
                    break;
                case "Container":
                    SetupContainerSelectableList();
                    break;
                case "Landscaping":
                    SetupLandscapingSelectableList();
                    break;
                case "Plant":
                    SetupPlantSelectableList();
                    break;
                default:
                    Debug.LogWarning("[ToolsManager] Spawned a selectable list for a tool that doesn't need one.");
                    break;
            }
        }
    }

    protected void SetupPropSelectableList()
    {
        if (panel == null)
            return;

        panel.onItemSelected += ConstructionEditor.Instance.OnItemSelected;

        int containerDefCount = ConstructionEditor.Instance.PropManifest.propDefinitions.Count;

        SelectableListItem item = (Resources.Load("Prefabs/UI/HorizontalListItem") as GameObject).GetComponent<SelectableListItem>();

        for (int i = 0, iC = containerDefCount; i < iC; ++i)
        {
            item.id = ConstructionEditor.Instance.PropManifest.GetPropDefinition(i).DescriptiveName;
            panel.AddPrefab(item);
        }
    }

    protected void SetupContainerSelectableList()
    {
        if (panel == null)
            return;

        panel.onItemSelected += ConstructionEditor.Instance.OnItemSelected;

        int containerDefCount = ConstructionEditor.Instance.ContainerManifest.containerDefinitions.Count;

        SelectableListItem item = (Resources.Load("Prefabs/UI/HorizontalListItem") as GameObject).GetComponent<SelectableListItem>();

        for (int i = 0, iC = containerDefCount; i < iC; ++i)
        {
            item.id = ConstructionEditor.Instance.ContainerManifest.GetContainerDefinition(i).DescriptiveName;
            panel.AddPrefab(item);
        }
    }

    protected void SetupLandscapingSelectableList()
    {
        if (panel == null)
            return;

        panel.onItemSelected += ConstructionEditor.Instance.OnItemSelected;

        int landscapingDefCount = ConstructionEditor.Instance.LandscapingManifest.landscapingDefinitions.Count;

        SelectableListItem item = (Resources.Load("Prefabs/UI/HorizontalListItem") as GameObject).GetComponent<SelectableListItem>();

        for (int i = 0, iC = landscapingDefCount; i < iC; ++i)
        {
            item.id = ConstructionEditor.Instance.LandscapingManifest.GetLandscapingDefinition(i).DescriptiveName;
            panel.AddPrefab(item);
        }
    }

    protected void SetupPlantSelectableList()
    {
        if (panel == null)
            return;

        int plantDefCount = ConstructionEditor.Instance.PlantManifest.plantDefinitions.Count;

        panel.onItemSelected += ConstructionEditor.Instance.OnItemSelected;

        SelectableListItem item = (Resources.Load("Prefabs/UI/HorizontalListItem") as GameObject).GetComponent<SelectableListItem>();

        for (int i = 0, iC = plantDefCount; i < iC; ++i)
        {
            item.id = ConstructionEditor.Instance.PlantManifest.GetPlantDefinition(i).DescriptiveName;
            panel.AddPrefab(item);
        }       
    }

    public void DespawnSelectableList()
    {
        if (panel == null)
            return; 

        Debug.Log("Destroying list");
        panel.ClearSelection(); // clear our selection first, allows us to trigger things.
        DestroyImmediate(panel.gameObject);
        panel = null;
    }


    /// <summary>
    /// Called in the case where the player clicks outside of the tool UI.
    /// </summary>
    protected void OnNonToolClick()
    {
       // Debug.Log("OnNonToolClick");
        lockToggleOn = true;
        SetContructionEditorMode(selectedTool.toggle);
    }

    /// <summary>
    /// Called in the case where the player clicks the same tool twice.
    /// </summary>
    /// <param name="tool"></param>
    protected void OnToolMultipleClick(ToolToggle tool)
    {
        if (selectedTool == null)
            return;

        //Debug.Log("OnToolMultipleClick");

        if (selectedTool == clickedTool)
            lockToggleOn = false;

        if (!selectedTool.toggle.isOn)
        {
            DespawnSelectableList();
            ConstructionEditor.Instance.SetConstructionMode(ConstructionEditor.ConstructionState.None);
        }

    }
}
