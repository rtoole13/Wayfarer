using UnityEngine;
using System;
using System.Collections;

public class TerrainGenerator : MonoBehaviour {

    Map map;
    int width;
    int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0,100)]
    public int randomFillPercentage;

    int[,] localGrid;

    void Start()
    {
        
        
    }
    public void Initialize()
    {
        // Get map component. Set local w/h to that of map
        map = GetComponentInChildren<Map>();
        width = map.width;
        height = map.height;
    }
    public int[,] GenerateMap()
    {
        localGrid = new int[width, height];
        RandomFillMap();
        return localGrid;
    }

    void RandomFillMap()
    {
        
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                localGrid[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercentage) ? 1 : 0;
            }
        }

    }
}
