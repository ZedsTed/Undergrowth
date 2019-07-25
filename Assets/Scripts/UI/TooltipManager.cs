using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipManager : Singleton<TooltipManager>, IPointerEnterHandler, IPointerExitHandler
{
    protected Tooltip tooltipInstance;
    public Tooltip ToolTipInstance => tooltipInstance;

    protected string tooltipPath = "Prefabs/UI/Tooltip";


    public Action<PointerEventData> onPointerEnter;
    public Action<PointerEventData> onPointerExit;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tooltipInstance != null)
        {
            tooltipInstance.transform.position = Input.mousePosition;
        }
    }

    protected void SpawnTooltip(string tooltipText, Vector2 position)
    {
        Debug.Log("Tooltip spawn at " + position);
        tooltipInstance = Instantiate(Load(), transform);

        tooltipInstance.transform.position = new Vector3(position.x, position.y, 0f);
        tooltipInstance.Text = tooltipText;
    }

    protected void DestroyTooltip()
    {
        if (tooltipInstance != null)
        {
            Debug.Log("destroy tooltip");
            Destroy(tooltipInstance.gameObject);
            tooltipInstance = null;
        }
    }

    protected Tooltip Load()
    {
        Debug.Log("loading tooltip");
        return (Resources.Load(tooltipPath) as GameObject).GetComponent<Tooltip>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("onpointerenter tooltip " + eventData.pointerCurrentRaycast.gameObject.name);
        TooltipData data = eventData.pointerCurrentRaycast.gameObject.GetComponent<TooltipData>();

        if (data)
        {
            Debug.Log("valid data");
            SpawnTooltip(data.text, eventData.position);
        }

        onPointerEnter?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyTooltip();

        onPointerExit?.Invoke(eventData);
    }
}
