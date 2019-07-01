using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ConstructionEditor : MonoBehaviour
{
    [SerializeField]
    protected PlantManifest plantManifest;
    public PlantManifest PlantManifest => plantManifest;

    [SerializeField]
    protected ContainerManifest containerManifest;
    public ContainerManifest ContainerManifest => containerManifest;


    public Grid EditorGrid;

    public TextMeshProUGUI WorldPositionDebug;
    public TextMeshProUGUI CellPositionDebug;
    public TextMeshProUGUI WorldCellPositionDebug;

    void Start()
    {

    }


    void Update()
    {

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            Vector3 pos = hit.point;

            Vector3Int cellPos = EditorGrid.WorldToCell(pos);

            Vector3 worldCellPos = EditorGrid.GetCellCenterWorld(cellPos);

            var posEnd = pos;
            posEnd.y = posEnd.y + 1f;

            var cellPosEnd = cellPos;
            cellPosEnd.y = cellPosEnd.y + 1;

            Debug.DrawLine(pos, posEnd, Color.blue);
            Debug.DrawLine(cellPos, cellPosEnd, Color.red);

            WorldPositionDebug.text = "World Position: " + pos.ToString();
            CellPositionDebug.text = "Cell Position: " + cellPos.ToString();
            WorldCellPositionDebug.text = "Center Position: " + worldCellPos.ToString();
            //Debug.Log("pos: " + pos);
            //Debug.Log("cellPos: " + cellPos);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit clickHit))
            {
                Vector3 hitCellPosition = EditorGrid.GetCellCenterWorld(EditorGrid.WorldToCell(clickHit.point));

                if (IsClickOnRaisedBed(clickHit, out Container clickedBed))
                {
                    hitCellPosition.y = 0f + clickedBed.Definition.Depth;

                    PlantDefinition pDef = PlantManifest.GetPlantDefinition("Plant01");

                    Plant p = Instantiate(pDef.Actor, hitCellPosition, Quaternion.identity, clickedBed.transform);
                    p.PlantBed = clickedBed;
                    p.Definition = pDef;
                }
                else
                {
                    ContainerDefinition cDef = ContainerManifest.GetContainerDefinition("RaisedBed");
                    hitCellPosition.y = 0f;
                    
                    Container c = Instantiate(cDef.Actor, hitCellPosition, 
                        Quaternion.identity, transform);

                    c.Definition = cDef;
                }
            }
        }
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
