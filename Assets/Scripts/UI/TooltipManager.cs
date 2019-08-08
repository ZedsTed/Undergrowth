using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipManager : SingletonDontCreate<TooltipManager>, IPointerEnterHandler, IPointerExitHandler
{
    protected string tooltipPath = "Prefabs/UI/Tooltip";

    public Action<PointerEventData> onPointerEnter;
    public Action<PointerEventData> onPointerExit;


    public Dictionary<GameObject, Tooltip> tooltips = new Dictionary<GameObject, Tooltip>();


    // Start is called before the first frame update
    void Start()
    {
        ConstructionEditor.Instance.onRaycastHit += OnRaycastHit;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (tooltips.Count > 0)
        {
            transform.position = Input.mousePosition;
        }
    }




    protected Tooltip Load()
    {
        return (Resources.Load(tooltipPath) as GameObject).GetComponent<Tooltip>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("[Tooltip] Entered: " + eventData.pointerCurrentRaycast.gameObject.name);
        if (tooltips.ContainsKey(eventData.pointerCurrentRaycast.gameObject))
            return;

        TooltipData data = eventData.pointerCurrentRaycast.gameObject.GetComponent<TooltipData>();
        // we want to get actor component.
        // then test the type of the stored actor
        // then spawn the tooltip with the actor data.

        if (data)
        {// TODO: Have SpawnTooltip add the tooltip and gameobject to the dictionary instead.
            tooltips.Add(eventData.pointerCurrentRaycast.gameObject,
                SpawnTooltip(eventData.pointerCurrentRaycast.gameObject, data.text, eventData.position));
        }

        onPointerEnter?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject == null)
            return;

        //Debug.Log("[Tooltip] Exited: " + eventData.pointerCurrentRaycast.gameObject.name);

        tooltips.Clear();

        for (int i = transform.childCount; i-- > 0;)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        onPointerExit?.Invoke(eventData);
    }

    protected GameObject hit;
    protected TooltipData data;
    protected void OnRaycastHit(List<GameObject> gameObjects)
    {
        for (int i = gameObjects.Count; i-- > 0;)
        {
            hit = gameObjects[i];

            if (hit == null)
            {
                Debug.LogError("[Tooltip::OnRaycastHit] Encountered a null gameobject, probably shouldn't happen unless you're trying to tooltip a gameObject that's being destroyed.");
                continue;
            }

            if (tooltips.TryGetValue(hit, out Tooltip t))
            {
                t.SetTooltipAsValid(); // Still a valid tooltip to keep.
                UpdateTooltip(hit, t); // We already have the tooltip, so let's make sure it's updated.
                continue;
            }

            data = hit.GetComponent<TooltipData>();

            if (data)
            {
                tooltips.Add(hit, SpawnTooltip(hit, data.text, Input.mousePosition));
                continue;
            }
            else if (!hit.transform.root.name.Contains("Canvas")) // if we're not looking at tooltip data, let's make sure it's not a UI element
            {
                OnRaycastEditorObject(hit);
            }
        }

        ClearInvalidTooltips();
    }

    //protected void OnRaycastHitUI(GameObject hitGameObject)
    //{
    //    Debug.Log("[Tooltip] RaycastHit: " + hitGameObject.name);
    //    if (tooltips.ContainsKey(hitGameObject))
    //        return;

    //    TooltipData data = hitGameObject.GetComponent<TooltipData>();
    //    // we want to get actor component.
    //    // then test the type of the stored actor
    //    // then spawn the tooltip with the actor data.

    //    if (data)
    //    {// TODO: Have SpawnTooltip add the tooltip and gameobject to the dictionary instead.
    //        tooltips.Add(hitGameObject,
    //            SpawnTooltip(hitGameObject, data.text, Input.mousePosition));
    //    }
    //    else
    //    {
    //        Debug.Log("[Tooltip] No data on " + hitGameObject.name);
    //    }
    //}

    protected List<GameObject> toRemove = new List<GameObject>();
    protected void OnRaycastEditorObject(GameObject hit)
    {
        // If we don't have it in our collection.
        if (!tooltips.TryGetValue(hit, out Tooltip t))
        {
            t = ParseHitGameObjectForType(hit);

            if (t != null)
                tooltips.Add(hit, t);
        }
    }

    /// <summary>
    /// Clears up any tooltips that are marked as invalid.
    /// Adds them to a remove list and then destroys them.
    /// </summary>
    protected void ClearInvalidTooltips()
    {
        toRemove.Clear();
                
        // Check if we have any in our collection that aren't valid.
        foreach (GameObject gameObject in tooltips.Keys)
        {
            if (!tooltips[gameObject].Valid)
            {
                toRemove.Add(gameObject);
            }
        }

        if (toRemove.Count > 0)
        {
            for (int i = toRemove.Count; i-- > 0;)
            {
                Destroy(tooltips[toRemove[i]].gameObject);
                tooltips.Remove(toRemove[i]);
            }
        }
    }

    protected void UpdateTooltip(GameObject hit, Tooltip t)
    {
        Actor actor = hit.GetComponentInParent<Actor>();

        if (actor == null)
        {
            return;
        }
        else if (actor is Container)
        {
            t.Text = GetTextForContainer(actor as Container);
        }
        else if (actor is Landscaping)
        {
            t.Text = actor.ToString();
        }
        else if (actor is Plant)
        {
            t.Text = actor.ToString();
        }
    }

    protected Tooltip SpawnTooltip(GameObject gameObject, string tooltipText, Vector2 position)
    {
        Tooltip tooltip = Instantiate(Load(), transform);

        tooltip.transform.position = new Vector3(position.x, position.y, 0f);
        tooltip.Text = tooltipText;

        //tooltips.Add(gameObject, tooltip);

        return tooltip;
    }

    protected void DestroyTooltip(GameObject gameObject)
    {
        Destroy(gameObject);
        gameObject = null;
    }

    protected Tooltip ParseHitGameObjectForType(GameObject gameObject)
    {
        Actor actor = gameObject.GetComponentInParent<Actor>();

        if (actor == null)
        {
            //Debug.Log("[Tooltip] Actor doesn't exist on this raycast object.");
            return null;
        }
        else if (actor is Container)
        {
            return SpawnTooltipForContainer(actor as Container);
        }
        else if (actor is Landscaping)
        {
            return SpawnTooltipForLandscaping(actor as Landscaping);
        }
        else if (actor is Plant)
        {
            return SpawnTooltipForPlant(actor as Plant);
        }

        return null;
    }


    protected Tooltip SpawnTooltipForContainer(Container container)
    {
        return SpawnTooltip(container.gameObject, GetTextForContainer(container), Input.mousePosition);
    }

    protected Tooltip SpawnTooltipForLandscaping(Landscaping landscaping)
    {
        return SpawnTooltip(landscaping.gameObject, landscaping.ToString(), Input.mousePosition);
    }


    protected Tooltip SpawnTooltipForPlant(Plant plant)
    {
        return SpawnTooltip(plant.gameObject, plant.ToString(), Input.mousePosition);
    }

    protected StringBuilder sb = new StringBuilder();

    protected string GetTextForContainer(Container container)
    {
        return container.Definition.DescriptiveName;
    }

}
