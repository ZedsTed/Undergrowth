using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class ConstructionEditor : Singleton<ConstructionEditor>
{
    [SerializeField]
    protected ContainerManifest containerManifest;
    public ContainerManifest ContainerManifest => containerManifest;

    [SerializeField]
    protected LandscapingManifest landscapingManifest;
    public LandscapingManifest LandscapingManifest => landscapingManifest;

    [SerializeField]
    protected PlantManifest plantManifest;
    public PlantManifest PlantManifest => plantManifest;




    public Grid EditorGrid;

    public ToolsManager ToolsManager;

    public TextMeshProUGUI WorldPositionDebug;
    public TextMeshProUGUI CellPositionDebug;
    public TextMeshProUGUI WorldCellPositionDebug;

    public enum ConstructionState
    {
        None,
        Placing, // Handles Containers, Landscaping and Planting.
        Watering,
        Removing
    }

    [SerializeField]
    protected ConstructionState previousMode;

    [SerializeField]
    protected ConstructionState mode;
    public ConstructionState Mode { get { return mode; } protected set { mode = value; } }

    public Action<ConstructionState> onConstructionModeChanged;

    protected GameObject pickedObject;
    public GameObject PickedObject { get { return pickedObject; } protected set { pickedObject = value; } }

    protected Vector3 inputPosition;
    protected Vector3 previousInputPosition;

    public float lerpFactor1;
    public float lerpFactor2;

    protected void Start()
    {
        inputPosition = new Vector3(0f, 0f, 0f);
        previousInputPosition = new Vector3(0f, 0f, 0f);
    }


    protected void Update()
    {
        if (mode == ConstructionState.Placing)
        {
            if (pickedObject != null)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    Vector3 hitCellPosition = EditorGrid.GetCellCenterWorld(EditorGrid.WorldToCell(hit.point));
                   
                    previousInputPosition = Vector3.Lerp(previousInputPosition, (hitCellPosition - inputPosition) * 0.6f, 33f * Time.deltaTime);
                    inputPosition += previousInputPosition;
                    inputPosition.y = 0f;
                
                    pickedObject.transform.position = inputPosition;

                    if (Input.GetMouseButtonDown(0))
                    {
                        hitCellPosition.y = 0f;
                        pickedObject.transform.position = hitCellPosition;
                        pickedObject = null;
                    }
                }
            }
        }

        //if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        //{
        //    Vector3 pos = hit.point;

        //    Vector3Int cellPos = EditorGrid.WorldToCell(pos);

        //    Vector3 worldCellPos = EditorGrid.GetCellCenterWorld(cellPos);

        //    var posEnd = pos;
        //    posEnd.y = posEnd.y + 1f;

        //    var cellPosEnd = cellPos;
        //    cellPosEnd.y = cellPosEnd.y + 1;

        //    Debug.DrawLine(pos, posEnd, Color.blue);
        //    Debug.DrawLine(cellPos, cellPosEnd, Color.red);

        //    WorldPositionDebug.text = "World Position: " + pos.ToString();
        //    CellPositionDebug.text = "Cell Position: " + cellPos.ToString();
        //    WorldCellPositionDebug.text = "Center Position: " + worldCellPos.ToString();
        //    //Debug.Log("pos: " + pos);
        //    //Debug.Log("cellPos: " + cellPos);
        //}
        //if (Input.GetMouseButtonDown(0))
        //{
        //    //if (EventSystem.current.IsPointerOverGameObject())
        //    //    return;

        //    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit clickHit))
        //    {
        //        Vector3 hitCellPosition = EditorGrid.GetCellCenterWorld(EditorGrid.WorldToCell(clickHit.point));

        //        if (IsClickOnRaisedBed(clickHit, out Container clickedBed))
        //        {
        //            hitCellPosition.y = 0f + clickedBed.Definition.Depth;

        //            PlantDefinition pDef = PlantManifest.GetPlantDefinition("Plant01");

        //            Plant p = Instantiate(pDef.Actor, hitCellPosition, Quaternion.identity, clickedBed.transform);
        //            p.PlantBed = clickedBed;
        //            p.Definition = pDef;
        //        }
        //        else
        //        {
        //            ContainerDefinition cDef = ContainerManifest.GetContainerDefinition("RaisedBed");
        //            hitCellPosition.y = 0f;

        //            Container c = Instantiate(cDef.Actor, hitCellPosition,
        //                Quaternion.identity, transform);

        //            c.Definition = cDef;
        //        }
        //    }
        //}
    }

    public void SetConstructionMode(ConstructionState state)
    {
        if (mode != state)
        {
            previousMode = mode;
            mode = state;
        }
    }

    public void RevertToPreviousConstructionMode()
    {
        mode = previousMode;
    }


    protected void OnConstructionModeChanged()
    {
        switch (mode)
        {
            case ConstructionState.None:
                break;
            case ConstructionState.Placing:
                break;
            case ConstructionState.Watering:
                break;
            case ConstructionState.Removing:
                break;
            default:
                break;
        }
    }

    public void OnItemSelected(SelectableListItem item, bool selected)
    {
        Debug.Log("CEselected " + item.id + " " + selected);

        if (pickedObject != null)
        {
            Destroy(pickedObject);
            pickedObject = null;
        }

        switch (ToolsManager.SelectedTool.name)
        {
            case "Container":
                SpawnContainer(item.id);
                break;
            case "Landscaping":
                SpawnLandscaping(item.id);
                break;
            case "Plant":
                SpawnPlant(item.id);
                break;
            default:
                break;
        }
    }

    protected void SpawnContainer(string name)
    {
        ContainerDefinition cDef = ContainerManifest.GetContainerDefinition(name);

        Container c = Instantiate(cDef.Actor, inputPosition,
            Quaternion.identity, transform);

        c.Definition = cDef;

        pickedObject = c.gameObject;
        pickedObject.transform.position = inputPosition;
        
    }

    protected void SpawnLandscaping(string name)
    {
        //ContainerDefinition cDef = ContainerManifest.GetContainerDefinition(name);

        //Container c = Instantiate(cDef.Actor, inputPosition,
        //    Quaternion.identity, transform);

        //c.Definition = cDef;

        //pickedObject = c.gameObject;
        //pickedObject.transform.position = inputPosition;
    }

    protected void SpawnPlant(string name)
    {
        PlantDefinition pDef = PlantManifest.GetPlantDefinition(name);

        Plant p = Instantiate(pDef.Actor, inputPosition,
            Quaternion.identity, transform);

        p.Definition = pDef;
        p.Picked = true;

        pickedObject = p.gameObject;
        pickedObject.transform.position = inputPosition;
        pickedObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }


    protected bool IsClickOnRaisedBed(RaycastHit clickhit, out Container bed)
    {
        // Check if it's in our parents (should be)
        bed = clickhit.transform.gameObject.GetComponentInParent<Container>();
        if (bed != null)
        {
            return true;
        }

        // Check if it's in our children (bad if it is, not a good prefab setup.
        bed = clickhit.transform.gameObject.GetComponentInChildren<Container>();
        if (bed != null)
        {
            Debug.LogWarning(bed.Definition.ContainerName + " has a bad collider setup for its actor/prefab.");
            return true;
        }

        return false;
    }
}
