using UnityEngine;
using System.Collections;

public class HexManager : MonoBehaviour {

    [HideInInspector]
    public int gridX, gridY;

    [HideInInspector]
    public Vector3 worldPosition;

    [HideInInspector]
    public bool walkable = false;
	
    public void Initialize(int _gridX, int _gridY, Vector3 _worldPosition)
    {
        gridX = _gridX;
        gridY = _gridY;
        worldPosition = _worldPosition;
    }
}
