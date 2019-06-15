using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionEditor : MonoBehaviour
{

    public Grid EditorGrid;


    void Start()
    {
        
    }

   
    void Update()
    {

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            Vector3 pos = hit.point;

            Vector3Int cellPos = EditorGrid.WorldToCell(pos);

            var posEnd = pos;
            posEnd.y = posEnd.y + 1f;

            var cellPosEnd = cellPos;
            cellPosEnd.y = cellPosEnd.y + 1;

            Debug.DrawLine(pos, posEnd * 1.5f, Color.blue);
            Debug.DrawLine(cellPos, cellPosEnd * 2, Color.red);

            Debug.Log("pos: " + pos);
            Debug.Log("cellPos: " + cellPos);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit clickHit))
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                cube.transform.position = EditorGrid.WorldToCell(clickHit.point);
            }
        }
    }
}
