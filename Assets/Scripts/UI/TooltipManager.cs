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
        ConstructionEditor.Instance.onRaycastHitEditorCollider += OnRaycastEditorObject;
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

        tooltips.Clear();

        for(int i = transform.childCount; i--> 0;)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        //if (tooltips.TryGetValue(eventData.pointerCurrentRaycast.gameObject, out Tooltip t))
        //{
        //    Destroy(t.gameObject);
        //    tooltips.Remove(eventData.pointerCurrentRaycast.gameObject);
        //}

        onPointerExit?.Invoke(eventData);
    }

    protected List<GameObject> toRemove = new List<GameObject>();
    protected void OnRaycastEditorObject(List<GameObject> gameObjects)
    {
        for (int i = gameObjects.Count; i-- > 0;)
        {
            GameObject gameObject = gameObjects[i];
            
            // If we don't have it in our collection.
            if (!tooltips.ContainsKey(gameObject))
            {
                Tooltip t = ParseHitGameObjectForType(gameObject);

                if (t != null)
                    tooltips.Add(gameObject, t);

            }
        }

        toRemove.Clear();
        // Check if we have any in our collection that aren't in the latest raycast.
        foreach (GameObject gameObject in tooltips.Keys)
        {
            //Debug.Log(gameObject.transform.parent.gameObject.name);
            if (!gameObjects.Contains(gameObject))
            {
                toRemove.Add(gameObject);
                // Destroy the corresponding tooltip here.
                
            }
        }

        if (toRemove.Count > 0)
        {
            for(int i = toRemove.Count; i--> 0;)
            {
                Destroy(tooltips[toRemove[i]].gameObject);
                tooltips.Remove(toRemove[i]);
            }
        }


        // TODO: Spawn a tooltip for each time we're called. Maintain a hashset of each tooltip/gameobject we're spawned for and then remove them if they're not on that hashset/list.
        // If the given object is the same as the stored raycast object, then no point in spawning.
        //if (raycastObject == gameObject)
        //    return;



    }

    //protected void SpawnNewTooltipInstance(string tooltipText, Vector2 position)
    //{
    //    DestroyTooltip();

    //    tooltipInstance = Instantiate(Load(), transform);

    //    tooltipInstance.transform.position = new Vector3(position.x, position.y, 0f);
    //    tooltipInstance.Text = tooltipText;
    //}

    protected Tooltip SpawnTooltip(GameObject gameObject, string tooltipText, Vector2 position)
    {
        Tooltip tooltip = Instantiate(Load(), transform);

        tooltip.transform.position = new Vector3(position.x, position.y, 0f);
        tooltip.Text = tooltipText;

        return tooltip;
    }

    //protected void DestroyTooltip()
    //{
    //    if (tooltipInstance != null)
    //    {
    //        DestroyTooltip(tooltipInstance.gameObject);
    //    }
    //}

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
        return SpawnTooltip(container.gameObject, container.Definition.ContainerName, Input.mousePosition);
    }

    protected Tooltip SpawnTooltipForLandscaping(Landscaping landscaping)
    {
        return SpawnTooltip(landscaping.gameObject, landscaping.Definition.LandscapingName, Input.mousePosition);
    }

    protected StringBuilder sb = new StringBuilder();
    protected Tooltip SpawnTooltipForPlant(Plant plant)
    {
        sb.Clear();
        sb.AppendLine(plant.Definition.PlantName);
        sb.AppendLine(plant.CurrentLifeCycle.ToString());
        sb.AppendLine("Growth: " + plant.CurrentGrowth + "/" + plant.Definition.MaxGrowth);
        sb.AppendLine("Growth Rate: " + plant.CurrentGrowthRate);

        return SpawnTooltip(plant.gameObject, sb.ToString(), Input.mousePosition);
    }


    protected Container IsContainer(GameObject gameObject)
    {
        // Check if we have one as our parent (expected).
        Container c = gameObject.GetComponentInParent<Container>();
        if (c)
            return c;

        // Check if we have one as our child (not expected, but can happen).
        c = gameObject.GetComponentInChildren<Container>();
        if (c)
            return c;

        return null;
    }

    protected Landscaping IsLandscaping(GameObject gameObject)
    {
        // Check if we have one as our parent (expected).
        Landscaping l = gameObject.GetComponentInParent<Landscaping>();
        if (l)
            return l;

        // Check if we have one as our child (not expected, but can happen).
        l = gameObject.GetComponentInChildren<Landscaping>();
        if (l)
            return l;

        return null;
    }

    protected Plant IsPlant(GameObject gameObject)
    {
        // Check if we have one as our parent (expected).
        Plant p = gameObject.GetComponentInParent<Plant>();
        if (p)
            return p;

        // Check if we have one as our child (not expected, but can happen).
        p = gameObject.GetComponentInChildren<Plant>();
        if (p)
            return p;

        return null;
    }
}
