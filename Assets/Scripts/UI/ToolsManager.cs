using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ToolsManager : MonoBehaviour
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

    protected GameObject panel;

    /// <summary>
    /// Lock Toggle On means if the player selects outside of the toggle ui, we want to lock the deselected toggle on in the Update method.
    /// </summary>
    [SerializeField]
    protected bool lockToggleOn = false;


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
        if (lockToggleOn && selectedTool != null && !selectedTool.toggle.isOn)
        {
            selectedTool.toggle.SetIsOnWithoutNotify(true);
        }

        if (panel != null && selectedTool != null)
        {
            if (!selectedTool.toggle.isOn)
                DespawnSelectableList();
        }
    }

    protected void OnToggleChanged(Toggle toggle)
    {
        Debug.Log("OnToggleChanged");

        switch (toggle.name)
        {
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
    }

    public void SetDeselectedTool(ToolToggle tool)
    {
        deselectedTool = tool;

        if (selectedTool == deselectedTool)
            OnNonToolClick();
    }

    public void OnSelectableListClicked()
    {
        Debug.Log("OnSelectableListClicked");
        lockToggleOn = true;
    }

    /// <summary>
    /// Spawns our selectable list for selecting the placement objects.
    /// </summary>
    /// <param name="tool"></param>
    protected void SpawnSelectableList(Toggle toggle)
    {
        // If we already have a panel spawned, return
        if (panel != null && panel.transform.parent == toggle.transform)
            return;
        else
            DespawnSelectableList();


        if (panel == null)
            panel = Instantiate(Resources.Load("Prefabs/UI/HorizontalItemList") as GameObject, toggle.transform);
    }

    public void DespawnSelectableList()
    {
        DestroyImmediate(panel);
        panel = null;
    }


    /// <summary>
    /// Called in the case where the player clicks outside of the tool UI.
    /// </summary>
    protected void OnNonToolClick()
    {
        Debug.Log("OnNonToolClick");
        lockToggleOn = true;
        OnToggleChanged(selectedTool.toggle);
    }

    /// <summary>
    /// Called in the case where the player clicks the same tool twice.
    /// </summary>
    /// <param name="tool"></param>
    protected void OnToolMultipleClick(ToolToggle tool)
    {
        Debug.Log("OnToolMultipleClick");

        if (selectedTool == clickedTool)
            lockToggleOn = false;        

        if (!selectedTool.toggle.isOn)
        {
            DespawnSelectableList();
            ConstructionEditor.Instance.SetConstructionMode(ConstructionEditor.ConstructionState.None);
        }
        
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
