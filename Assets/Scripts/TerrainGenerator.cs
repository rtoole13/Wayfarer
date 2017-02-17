using UnityEngine;
using System;
using System.Collections;

public class TerrainGenerator : MonoBehaviour {

    int width;
    int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0,100)]
    public int randomFillPercentage;

    public bool eastCoast;
    public bool westCoast;
    public bool northCoast;
    public bool southCoast;
    
    public int smoothing;
    int[,] localGrid;

    public void Initialize(int _width, int _height)
    {
        //Set local w/h to that of map
        width = _width;
        height = _height;
    }
    public int[,] GenerateMap()
    {
        localGrid = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < smoothing; i++)
        {
            SmoothMap();
        }
        return localGrid;
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;

        for(int x = -1; x <= 1; x++)
        {
            for(int y =  -1; y <= 1; y++)
        {
                if (x == 0 && y == 0)
                {
                    continue;
                }               
                if (x == -1 && y != 0 && gridY % 2 == 1)
                {
                    continue;
                }
                else if (x == 1 && y != 0 && gridY % 2 == 0)
                {
                    continue;
                }
                int checkX = gridX + x;
                int checkY = gridY + y;

                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    wallCount += localGrid[checkX, checkY];
                }   
                else
                {
                    //encourages growth at map edges. Could just as well do without, or have conditions
                    //to encourage growth at particular edges, rather than all
                    
                    //removing altogether results in a neat borderless, island archipelago-like map
                    if (eastCoast || westCoast || northCoast || southCoast)
                    {
                        wallCount ++;
                    }
                }
        }
    }
        return wallCount;
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
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    if (x == width - 1 && eastCoast)
                    {
                        localGrid[x, y] = 1;
                        continue;
                    }

                    if (x == 0 && westCoast)
                    {
                        localGrid[x, y] = 1;
                        continue;
                    }
                    if (y == height -1 && northCoast)
                    {
                        localGrid[x, y] = 1;
                        continue;
                    }
                    if (y == 0 && southCoast)
                    {
                        localGrid[x, y] = 1;
                        continue;
                    }
                }
                else
                {
                    localGrid[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercentage) ? 1 : 0;
                }
            }
        }

    }
    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighborWallTiles = GetSurroundingWallCount(x, y);
                
                if (neighborWallTiles > 3)
                {
                    localGrid[x, y] = 1;
                }
                else if (neighborWallTiles < 3) 
                {
                    localGrid[x, y] = 0;
                }
            }
        }
    }
}
