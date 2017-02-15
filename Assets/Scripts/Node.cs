﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node{

    public bool walkable;
    public Vector3 worldPosition;

    public int gridX, gridY;

    public int gCost;
    public int hCost;

    public GameObject hexRef;
    public Node parent;

    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY, GameObject _hexRef)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
        hexRef = _hexRef;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
