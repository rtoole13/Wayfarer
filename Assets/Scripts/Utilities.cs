using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities {


    public static Node NodeFromMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hitInfo;
        
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask("HexMap")))
        {
            GameObject hitObject = hitInfo.collider.transform.parent.gameObject;
            return hitObject.GetComponent<HexManager>().node;
        }
        return null;
    }

    public static Vector2 GridFromCubeCoords(Vector3 cubeCoords)
    {
        int row = Mathf.RoundToInt(cubeCoords.y);
        int col = Mathf.RoundToInt(cubeCoords.x + (row - row % 1) / 2);
        return new Vector2(col, row);
    }

}
