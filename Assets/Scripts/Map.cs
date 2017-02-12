﻿using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

    public GameObject hexPrefab;
    TerrainGenerator terrainGen;
    public int width;
    public int height;

    float xEvenOffset;
    float yEvenOffset;

    GameObject[,] hexGrid;
    int[,] pathGrid;

    // Use this for initialization
    void Start ()
    {
        // Initialize path and hex grids
        hexGrid = new GameObject[width, height];
        pathGrid = new int[width, height];

        // Get procedural terrain generator
        terrainGen = GetComponentInChildren<TerrainGenerator>();
        terrainGen.Initialize();

        // Initialize values of offset from grid integer coordinates to world coordinates
        xEvenOffset = Mathf.Sqrt(3) / 2;
        yEvenOffset = 3/4f;

	    for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                
                hexGrid[x,y] = createHex(x, y, xEvenOffset, yEvenOffset);
            }
        }
        
        // Get the terrain grid map
        pathGrid = terrainGen.GenerateMap();

        // Color terrain and label hex unwalkable
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (pathGrid[x,y] == 1)
                {
                    hexGrid[x, y].GetComponent<HexManager>().walkable = false;
                    hexGrid[x, y].GetComponentInChildren<SpriteRenderer>().color = Color.gray;
                }
            }
        }
    }
	
    GameObject createHex(int x, int y, float worldXOffset, float worldYOffset)
    {
        //from grid coords to world
        float xPos = x * xEvenOffset;
        float yPos = y * yEvenOffset;
        if (y % 2 == 1)
        {
            //on even rows, x offset is an additional half of x offset
            xPos += xEvenOffset / 2f;
        }
        Vector3 worldPos = new Vector3(xPos, 0, yPos);
        GameObject newHex = (GameObject)Instantiate(hexPrefab, worldPos, Quaternion.identity);

        //pass in  grid coordinates and world coordinates (latter not necessary I suppose considering transform)
        newHex.GetComponent<HexManager>().Initialize(x, y, worldPos);

        //rename hex
        newHex.name = "Hex_" + x.ToString().PadLeft(2,'0') + "_" + y.ToString().PadLeft(2, '0');
        newHex.transform.SetParent(this.transform);
        return newHex;
    }
}
