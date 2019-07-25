using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipManager : SingletonDontCreate<TooltipManager>, IPointerEnterHandler, IPointerExitHandler
{
    protected Tooltip tooltipInstance;
    public Tooltip ToolTipInstance => tooltipInstance;

    protected string tooltipPath = "Prefabs/UI/Tooltip";


    public Action<PointerEventData> onPointerEnter;
    public Action<PointerEventData> onPointerExit;



    // Start is called before the first frame update
    void Start()
    {
        ConstructionEditor.Instance.onRaycastHitEditorCollider += OnRaycastEditorObject;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (tooltipInstance != null)
        {
            tooltipInstance.transform.position = Input.mousePosition;
        }
    }

 
   

    protected Tooltip Load()
    {        
        return (Resources.Load(tooltipPath) as GameObject).GetComponent<Tooltip>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {        
        TooltipData data = eventData.pointerCurrentRaycast.gameObject.GetComponent<TooltipData>();
        // we want to get actor component.
        // then test the type of the stored actor
        // then spawn the tooltip with the actor data.

        if (data)
        {            
            SpawnTooltip(data.text, eventData.position);
        }    

        onPointerEnter?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyTooltip();

        onPointerExit?.Invoke(eventData);
    }

    protected GameObject raycastObject;
    protected void OnRaycastEditorObject(GameObject gameObject)
    {
        // If the given object is the same as the stored raycast object, then no point in spawning.
        if (raycastObject == gameObject)
            return;

        raycastObject = gameObject;
        Actor actor = raycastObject.GetComponent<Actor>();

        if (actor == null)
        {
            Debug.Log("[Tooltip] Actor doesn't exist on this raycast object.");
            return;
        }
        else if (actor is Container)
        {
            SpawnTooltipForContainer(actor as Container);
        }
        else if (actor is Landscaping)
        {
            SpawnTooltipForLandscaping(actor as Landscaping);
        }
        else if (actor is Plant)
        {
            SpawnTooltipForPlant(actor as Plant);
        }
    }

    protected void SpawnTooltip(string tooltipText, Vector2 position)
    {
        DestroyTooltip();

        tooltipInstance = Instantiate(Load(), transform);

        tooltipInstance.transform.position = new Vector3(position.x, position.y, 0f);
        tooltipInstance.Text = tooltipText;
    }

    protected void DestroyTooltip()
    {
        if (tooltipInstance != null)
        {
            Destroy(tooltipInstance.gameObject);
            tooltipInstance = null;
            raycastObject = null;
        }
    }


    protected void SpawnTooltipForContainer(Container container)
    {
        SpawnTooltip(container.Definition.ContainerName, Input.mousePosition);
    }

    protected void SpawnTooltipForLandscaping(Landscaping landscaping)
    {
        SpawnTooltip(landscaping.Definition.LandscapingName, Input.mousePosition);
    }

    protected StringBuilder sb = new StringBuilder();
    protected void SpawnTooltipForPlant(Plant plant)
    {
        sb.Clear();
        sb.AppendLine(plant.Definition.PlantName);
        sb.AppendLine(plant.CurrentLifeCycle.ToString());
        sb.AppendLine("Growth: " + plant.CurrentGrowth + "/" + plant.Definition.MaxGrowth);
        sb.AppendLine("Growth Rate: " + plant.CurrentGrowthRate);

        SpawnTooltip(sb.ToString(), Input.mousePosition);
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
