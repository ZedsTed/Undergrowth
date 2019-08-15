using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class ConstructionEditor : SingletonDontCreate<ConstructionEditor>
{
    // TODO: Pop all of the below into a big ol' game class.

    [SerializeField]
    protected PropManifest propManifest;
    public PropManifest PropManifest => propManifest;

    [SerializeField]
    protected ContainerManifest containerManifest;
    public ContainerManifest ContainerManifest => containerManifest;

    [SerializeField]
    protected LandscapingManifest landscapingManifest;
    public LandscapingManifest LandscapingManifest => landscapingManifest;

    [SerializeField]
    protected PlantManifest plantManifest;
    public PlantManifest PlantManifest => plantManifest;

    [SerializeField]
    protected ItemManifest itemManifest;
    public ItemManifest ItemManifest => itemManifest;


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


    public List<Prop> Props { get; protected set; } = new List<Prop>();
    public List<Container> Containers { get; protected set; } = new List<Container>();
    public List<Landscaping> Landscapings { get; protected set; } = new List<Landscaping>();
    public List<Plant> Plants { get; protected set; } = new List<Plant>();


    public Action<ConstructionState> onConstructionModeChanged;
    public Action<List<GameObject>> onRaycastHit;
    public Action<GameObject> onRaycastHitUI;

    protected Actor pickedActor;
    public Actor PickedObject { get { return pickedActor; } protected set { pickedActor = value; } }

    protected Vector3 inputPosition;
    protected Vector3 previousInputPosition;
    protected Vector3 editorGridWorldPosition;
    protected Vector3 hitCellPosition;

    [SerializeField]
    protected float appliedRotationY = 0f;

    [SerializeField]
    protected float previousActorRotation;

    [SerializeField]
    protected float actorRotation;

    public float lerpFactor1;
    public float lerpFactor2;

    protected void Start()
    {
        inputPosition = new Vector3(0f, 0f, 0f);
        previousInputPosition = new Vector3(0f, 0f, 0f);


    }



    protected void Update()
    {
        WorldCellPositionDebug.text = EditorGrid.GetCellCenterWorld(EditorGrid.WorldToCell(editorGridWorldPosition)).ToString();
        WorldPositionDebug.text = editorGridWorldPosition.ToString();

        if (!IsPointerOverUI())
        {
            if (mode == ConstructionState.Placing)
            {
                if (pickedActor != null)
                {

                    UpdatePosition();

                    UpdateRotation();



                    if (IsValidPosition())
                    {
                        OnValidPosition();

                        if (Input.GetMouseButtonDown(0))
                        {
                            bool multiple = false;

                            if (Input.GetKey(KeyCode.LeftControl))
                                multiple = true;

                            OnActorPlaced(hitCellPosition, multiple);
                        }
                    }
                    else
                    {
                        OnInvalidPosition();
                    }
                }
                else
                {
                    GridHighlight.Instance.SetVisibility(false);
                }
            }
            else if (mode == ConstructionState.Watering)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    for (int i = raycastResults.Count; i-- > 0;)
                    {
                        Actor actor = raycastResults[i].gameObject.GetComponentInParent<Actor>();
                        if (actor == null)
                            continue;

                        if (!(actor is Landscaping))
                            continue;

                        // TODO: Change to some sort of variable quantity.
                        (actor as Landscaping).OnWatered(1f);
                    }
                }
            }
            else if (mode == ConstructionState.Removing)
            {
                if (Input.GetMouseButton(0))
                {
                    for (int i = 0, iC = raycastResults.Count; i < iC; ++i)
                    {
                        if (raycastResults[i].gameObject.GetComponentInParent<Actor>())
                        {
                            Destroy(raycastResults[i].gameObject.transform.parent.gameObject);
                        }
                    }
                }
            }
        }
        else
        {

        }

    }

    protected void UpdatePosition()
    {
        //Debug.Log(pointerEventData.pointerCurrentRaycast.worldPosition);
        hitCellPosition = EditorGrid.GetCellCenterWorld(EditorGrid.WorldToCell(editorGridWorldPosition));

        hitCellPosition.y = 0f;

        GridHighlight.Instance.SetVisibility(true);
        GridHighlight.Instance.SetPosition(hitCellPosition);

        previousInputPosition = Vector3.Lerp(previousInputPosition, (hitCellPosition - inputPosition) * 0.6f, 33f * Time.unscaledDeltaTime);
        inputPosition += previousInputPosition;
        inputPosition.y = 0f;

        pickedActor.transform.position = hitCellPosition;
        pickedActor.Mesh.transform.position = inputPosition;
    }

    protected void UpdateRotation()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            appliedRotationY += -90f;

        if (Input.GetKeyDown(KeyCode.X))
            appliedRotationY += 90f;


        previousActorRotation = Mathf.Lerp(previousActorRotation, (appliedRotationY - actorRotation) * 0.6f, 33f * Time.unscaledDeltaTime);

        actorRotation += previousActorRotation;


        pickedActor.transform.rotation = Quaternion.Euler(0f, actorRotation, 0f);
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

        if (pickedActor != null)
        {
            Debug.Log("deselect");
            Destroy(pickedActor.gameObject);
            pickedActor = null;

            previousActorRotation = 0f;
            actorRotation = 0f;
            appliedRotationY = 0f;

            //Highlighter.Instance.RemoveHighlight(pickedActor.gameObject);

            return;
        }
        else if (!selected)
        {   // At this point we've done everything we want to with a deselection.
            return;
        }

        switch (ToolsManager.SelectedTool.name)
        {
            case "Prop":
                SpawnProp(item.id);
                break;
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
       
    protected Container parentContainer;
    protected Landscaping parentLandscaping;
    protected void OnValidPosition()
    {
        pickedActor.OnValidPosition();

        if (pickedActor is Landscaping)
        {
            parentContainer = (pickedActor as Landscaping).GetComponentInParent<Container>();

            // Grab parent rotation
            Quaternion rot = parentContainer.transform.rotation;

            // Grab parent size definition
            Vector3 size = parentContainer.Definition.ContainerSoilSize;

            // Work out where we need to be in this container, 
            // what our offset from our snap position and out 
            // world position it is so we can correctly position ourselves.

            Vector3 snapToPos = parentContainer.SnapPoint.position;

            pickedActor.transform.position = snapToPos;
            pickedActor.Mesh.transform.position = snapToPos;
            hitCellPosition = snapToPos; // We now need to update our 'selected' cell so that when we place this item, it'll be place there.
            pickedActor.transform.rotation = rot; // We don't want to lerp this, it'll look too clunky.

            pickedActor.SetScale(size);
        }
        else if (pickedActor is Plant)
        {
            parentLandscaping = (pickedActor as Plant).GetComponentInParent<Landscaping>();

            Vector3 snapToPos = parentLandscaping.SnapPoint.position;
            hitCellPosition = snapToPos;


            pickedActor.transform.position = snapToPos;
        }
    }

    protected void OnInvalidPosition()
    {
        pickedActor.OnInvalidPosition();

        if (pickedActor is Landscaping)
        {
            // Revert it back.
            Vector3 size = ContainerManifest.GetContainerDefinition("Raised Bed").ContainerSoilSize;
            
            pickedActor.SetScale(size);
        }
    }

    protected void OnActorPlaced(Vector3 cellPosition, bool multiple = false)
    {
        pickedActor.transform.position = cellPosition;

        pickedActor.Mesh.transform.position = cellPosition;
        pickedActor.SetLayerForHighlight(false);
        Accounts.Instance.BuyItem(pickedActor.Definition.Cost); // TODO: Need to add in a solution for if the player can't afford to buy
        pickedActor.OnPlaced();
        pickedActor.Picked = false;


        if (multiple)
            SpawnDuplicateActor(pickedActor);
        else
        {
            pickedActor = null;

            previousActorRotation = 0f;
            actorRotation = 0f;
            appliedRotationY = 0f;
        }
    }

    #region Actor Spawning

    protected void SpawnDuplicateActor(Actor currentActor)
    {
        if (currentActor is Prop)
            SpawnProp(currentActor.Definition.DescriptiveName);
        else if (currentActor is Container)
            SpawnContainer(currentActor.Definition.DescriptiveName);
        else if (currentActor is Landscaping)
            SpawnLandscaping(currentActor.Definition.DescriptiveName);
        else if (currentActor is Plant)
            SpawnPlant(currentActor.Definition.DescriptiveName);
        else
            Debug.LogError("[SpawnDuplicateActor] You are trying to spawn a duplicate actor of unknown type.");

        pickedActor.SetLayerForHighlight(true);
    }

    protected void SpawnProp(string name)
    {
        PropDefinition pDef = PropManifest.GetPropDefinition(name);

        Prop p = Instantiate(pDef.Actor, inputPosition,
            Quaternion.identity, transform);

        Props.Add(p);

        p.Definition = pDef;

        pickedActor = p;
        pickedActor.transform.position = inputPosition;

    }

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
                // Containers are never valid if they're colliding with something
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
                    {
                        return false;
                    }
                    else if (sceneColliders[i].CompareTag("Container"))
                    {
                        if (pickedActor.CompareTag("EditorGrid"))
                        {
                            pickedActor.transform.parent = sceneColliders[i].transform.parent;
                            return true;
                        }   //If our container isn't this scene collider and the current input position is in its bounds, we want to switch to it.
                        else if (pickedActor.transform.parent != sceneColliders[i].transform.parent &&
                            sceneColliders[i].bounds.Contains(inputPosition))
                        {
                            pickedActor.transform.parent = sceneColliders[i].transform.parent;
                            return true;
                        }
                        else if (pickedActor.transform.parent == sceneColliders[i].transform.parent &&
                            sceneColliders[i].bounds.Contains(inputPosition))
                        {   // at this point, we've checked if we've changed and we haven't so we're the same one.
                            pickedActor.transform.parent = sceneColliders[i].transform.parent;
                            return true;
                        }
                    }
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
                    {
                        return false;
                    }
                    else if (sceneColliders[i].CompareTag("Landscaping"))
                    {
                        pickedActor.transform.parent = sceneColliders[i].transform.parent.gameObject.transform;
                        return true;
                    }
                }
            }
        }
        else
        {
            // Only containers or props can be placed by themselves for the moment.
            // TODO: eventually this will be landscaping of certain types as well.
            if (pickedActor is Container || pickedActor is Prop)
                return true;
        }


        return false;
    }

    [SerializeField]
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
        float colliderShrink = 0.8f; // We want to shrink our colliders just to make sure we have a proper intersect.

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

        sceneColliders.Sort((x, y) =>
        Vector3.Distance(x.gameObject.transform.position, pickedActor.gameObject.transform.position)
        .CompareTo
        (Vector3.Distance(y.gameObject.transform.position, pickedActor.gameObject.transform.position)));



        return sceneColliders.Count > 0 ? true : false;
    }

    protected List<RaycastResult> raycastResults = new List<RaycastResult>();
    protected PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
    protected RaycastResult pointerRaycastResult;
    protected List<GameObject> hitColliders = new List<GameObject>();
    /// <summary>
    /// Checks whether the current pointer position is over a gameobject whose root transform is a Canvas.
    /// </summary>
    /// <returns>True if pointer is over UI, false if anything else.</returns>
    protected bool IsPointerOverUI()
    {
        raycastResults.Clear();
        hitColliders.Clear();

        pointerEventData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        raycastResults.Sort((x, y) => x.distance.CompareTo(y.distance));

        // Loop through from the start of the collection, 
        // Now that we've done a sort, it's usually the case that 
        // the object we need is the closest to the camera.
        for (int i = 0, iC = raycastResults.Count; i < iC; ++i)
        {
            pointerRaycastResult = raycastResults[i];

            if (pointerRaycastResult.gameObject.transform.root.name.Contains("Canvas"))
            {
                hitColliders.Add(pointerRaycastResult.gameObject);
                onRaycastHit?.Invoke(hitColliders);
                return true; // We're over UI, as it's the first thing we've hit we can add, invoke and just return to let the editor know we're over UI and don't want to do anything else.
            }
            else if (pointerRaycastResult.gameObject.CompareTag("EditorGrid"))
            {
                editorGridWorldPosition = pointerRaycastResult.worldPosition;
            }

            if (pickedActor == null && (pointerRaycastResult.gameObject.layer == LayerMask.NameToLayer("EditorColliders") || pointerRaycastResult.gameObject.CompareTag("EditorGrid")))
            {
                // Let's tell everyone we've hit something of note.

                hitColliders.Add(pointerRaycastResult.gameObject);
            }
        }


        if (hitColliders.Count > 0)
            onRaycastHit?.Invoke(hitColliders);

        return false;
    }

}
