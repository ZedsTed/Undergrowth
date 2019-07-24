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

    public List<Container> Containers { get; protected set; } = new List<Container>();
    public List<Landscaping> Landscapings { get; protected set; } = new List<Landscaping>();
    public List<Plant> Plants { get; protected set; } = new List<Plant>();


    public PostEffect postEffect;

    public Action<ConstructionState> onConstructionModeChanged;

    protected Actor pickedActor;
    public Actor PickedObject { get { return pickedActor; } protected set { pickedActor = value; } }

    protected Vector3 inputPosition;
    protected Vector3 previousInputPosition;
    protected Vector3 editorGridWorldPosition;

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
            if (pickedActor != null)
            {
                if (!IsPointerOverUI())
                {
                    //Debug.Log(pointerEventData.pointerCurrentRaycast.worldPosition);
                    Vector3 hitCellPosition = EditorGrid.GetCellCenterWorld(EditorGrid.WorldToCell(editorGridWorldPosition));

                    previousInputPosition = Vector3.Lerp(previousInputPosition, (hitCellPosition - inputPosition) * 0.6f, 33f * Time.deltaTime);
                    inputPosition += previousInputPosition;
                    inputPosition.y = 0f;

                    pickedActor.transform.position = inputPosition;

                    if (IsValidPosition())
                    {
                        postEffect.SetColor(Color.green);
                        if (Input.GetMouseButtonDown(0))
                        {
                            hitCellPosition.y = 0f;
                            pickedActor.transform.position = hitCellPosition;
                            pickedActor.SetLayerForHighlight(false);
                            pickedActor.Picked = false;
                            pickedActor = null;
                        }
                    }
                    else
                    {
                        postEffect.SetColor(Color.red);
                    }
                }
            }
        }
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

        if (pickedActor != null && !selected)
        {
            Debug.Log("deselect");
            Destroy(pickedActor.gameObject);
            pickedActor = null;

            return;
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

        if (selected && pickedActor != null && pickedActor.gameObject.layer != LayerMask.NameToLayer("PostProcessOutline"))
            pickedActor.SetLayerForHighlight(true);
    }

    #region Actor Spawning

    protected void SpawnContainer(string name)
    {
        ContainerDefinition cDef = ContainerManifest.GetContainerDefinition(name);

        Container c = Instantiate(cDef.Actor, inputPosition,
            Quaternion.identity, transform);

        Containers.Add(c);

        c.Definition = cDef;

        pickedActor = c;
        pickedActor.transform.position = inputPosition;

    }

    protected void SpawnLandscaping(string name)
    {
        LandscapingDefinition lDef = LandscapingManifest.GetLandscapingDefinition(name);

        Landscaping l = Instantiate(lDef.Actor, inputPosition,
            Quaternion.identity, transform);

        Landscapings.Add(l);

        l.Definition = lDef;
        l.Picked = true;

        pickedActor = l;
        pickedActor.transform.position = inputPosition;
    }

    protected void SpawnPlant(string name)
    {
        PlantDefinition pDef = PlantManifest.GetPlantDefinition(name);

        Plant p = Instantiate(pDef.Actor, inputPosition,
            Quaternion.identity, transform);

        Plants.Add(p);

        p.Definition = pDef;
        p.Picked = true;

        pickedActor = p;
        pickedActor.transform.position = inputPosition;
        pickedActor.transform.localScale = new Vector3(1f, 1f, 1f);
    }


    #endregion


    protected bool IsValidPosition()
    {
        if (IsCollidingWithWorld())
        {
            if (pickedActor is Container)
            {
                return false;
            }
            else if (pickedActor is Landscaping)
            {
                // Scan through scene colliders to check if any of them belong to containers.
                // If we have any landscaping or plant colliders we're not valid.
                // If we have any container ones, and we haven't already returned, then we have a valid placement.
                for (int i = sceneColliders.Count; i-- > 0;)
                {
                    if (sceneColliders[i].CompareTag("Landscaping") || sceneColliders[i].CompareTag("Plant"))
                        return false;
                    else if (sceneColliders[i].CompareTag("Container"))
                        return true;
                }
            }
            else if (pickedActor is Plant)
            {
                // Scan through scene colliders to check if any of them belong to landscaping.
                // If we have any landscaping or plant colliders we're not valid.
                // If we have any container ones, and we haven't already returned, then we have a valid placement.
                for (int i = sceneColliders.Count; i-- > 0;)
                {
                    if (sceneColliders[i].CompareTag("Plant"))
                        return false;
                    else if (sceneColliders[i].CompareTag("Landscaping"))
                        return true;
                }
            }
        }
        else
        {
            // Only containers can be placed by themselves for the moment.
            // TODO: eventually this will be landscaping of certain types as well.
            if (pickedActor is Container)
                return true;
        }


        return false;
    }

    protected List<Collider> sceneColliders = new List<Collider>();
    protected List<Collider> actorColliders = new List<Collider>();
    int colliderLayer;
    int colliderLayerMask;
    protected bool IsCollidingWithWorld()
    {
        // Setup
        sceneColliders.Clear();
        actorColliders.Clear();

        colliderLayer = LayerMask.NameToLayer("EditorColliders");
        colliderLayerMask = 1 << colliderLayer;
        int properMask = LayerMask.GetMask("EditorColliders");

        // Gather colliders
        actorColliders.AddRange(pickedActor.GetComponentsInChildren<Collider>(true));

        Collider c;
        float colliderShrink = 0.975f; // We want to shrink our colliders just to make sure we have a proper intersect.

        // Sort them and then union to see what we intersect with.
        for (int i = actorColliders.Count; i-- > 0;)
        {
            c = actorColliders[i];

            if (c.gameObject.layer != colliderLayer)
            {
                actorColliders.RemoveAt(i); // maybe we don't even need to remove, just continue.
                continue; // continue as we don't want to test this one.
            }

            Matrix4x4 localToWorld = c.transform.localToWorldMatrix;

            if (c is BoxCollider)
            {
                BoxCollider bc = c as BoxCollider;

                sceneColliders.AddRange(Physics.OverlapBox(
                    localToWorld.MultiplyPoint(bc.center),
                    bc.size * 0.5f * colliderShrink,
                    bc.transform.rotation,
                    colliderLayerMask,
                    QueryTriggerInteraction.Collide));
            }
            else if (c is SphereCollider)
            {
                SphereCollider sc = c as SphereCollider;

                sceneColliders.AddRange(Physics.OverlapSphere(
                    localToWorld.MultiplyPoint(sc.center),
                    sc.radius * colliderShrink,
                    colliderLayerMask,
                    QueryTriggerInteraction.Collide));
            }
            else if (c is CapsuleCollider)
            {
                CapsuleCollider cc = c as CapsuleCollider;

                float halfHeight = cc.height * 0.5f * colliderShrink;

                Vector3 pointA = new Vector3(0f, halfHeight - cc.radius, 0f);

                Vector3 pointB = new Vector3(0f, -halfHeight + cc.radius, 0f);

                sceneColliders.AddRange(Physics.OverlapCapsule(
                    localToWorld.MultiplyPoint(cc.center + pointA),
                    localToWorld.MultiplyPoint(cc.center + pointB),
                    cc.radius * colliderShrink,
                    colliderLayerMask,
                    QueryTriggerInteraction.Collide));
            }
        }

        // Remove any of our own colliders from the picked actor.
        for (int i = sceneColliders.Count; i-- > 0;)
        {
            if (actorColliders.Contains(sceneColliders[i]))
                sceneColliders.RemoveAt(i);
        }


        return sceneColliders.Count > 0 ? true : false;
    }

    protected List<RaycastResult> raycastResults = new List<RaycastResult>();
    protected PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
    protected RaycastResult pointerRaycastResult;
    
    /// <summary>
    /// Checks whether the current pointer position is over a gameobject whose root transform is a Canvas.
    /// </summary>
    /// <returns>True if pointer is over UI, false if anything else.</returns>
    protected bool IsPointerOverUI()
    {
        raycastResults.Clear();        

        pointerEventData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        for (int i = raycastResults.Count; i-- > 0;)
        {
            pointerRaycastResult = raycastResults[i];

            if (pointerRaycastResult.gameObject.transform.root.name.Contains("Canvas"))
            {
                return true;
            }
            else if (pointerRaycastResult.gameObject.CompareTag("EditorGrid"))
            {
                editorGridWorldPosition = pointerRaycastResult.worldPosition;
            }
              
        }

        return false;
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
