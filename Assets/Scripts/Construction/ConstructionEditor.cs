using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ConstructionEditor : MonoBehaviour
{

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
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                cube.transform.position = EditorGrid.GetCellCenterWorld(EditorGrid.WorldToCell(clickHit.point));
            }
        }
    }
}
