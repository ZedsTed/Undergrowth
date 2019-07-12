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

    /// <summary>
    /// Lock Toggle On means if the player selects outside of the toggle ui, we want to lock the deselected toggle on in the Update method.
    /// </summary>
    [SerializeField]
    protected bool lockToggleOn = false;

    /// <summary>
    /// Lock Toggle Off means if the player selects the same toggle twice, we want to lock the toggle off until they either click it, or another toggle again.
    /// </summary>
    [SerializeField]
    protected bool lockToggleOff = false;

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
        // If we're locking a toggle ON and the current deselected toggle isn't on already.
        if (lockToggleOn && !lockToggleOff && deselectedTool != null && !deselectedTool.toggle.isOn)
            deselectedTool.toggle.SetIsOnWithoutNotify(true);
        else if (!lockToggleOn && lockToggleOff && clickedTool.toggle.isOn)
        {   // If we're locking a toggle OFF and the current click toggle is currently on.
            clickedTool.toggle.isOn = false;
        }
    }

    protected void OnToggleChanged(Toggle toggle)
    {
        Debug.Log("OnToggleChanged");

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

    
    /// <summary>
    /// Called in the case where the player clicks outside of the tool UI.
    /// </summary>
    protected void OnNonToolClick()
    {
        Debug.Log("OnNonToolClick");
        lockToggleOn = true;
        OnToggleChanged(deselectedTool.toggle);
    }


    /// <summary>
    /// Called in the case where the player clicks the same tool twice.
    /// </summary>
    /// <param name="tool"></param>
    protected void OnToolMultipleClick(ToolToggle tool)
    {
        Debug.Log("OnToolMultipleClick");

        
        lockToggleOn = clickedTool.toggle.isOn;
        if (!clickedTool.toggle.isOn)
        {
            ConstructionEditor.Instance.SetConstructionMode(ConstructionEditor.ConstructionState.None);
        }
        //else
        //{
        //    ConstructionEditor.Instance.RevertToPreviousConstructionMode();
        //}
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
