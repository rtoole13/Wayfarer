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

        for(int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++)
        {
            for(int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++)
        {
                //FIX ME: Currently not finding exact hex neighbors. acting like a square grid.
                if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
                {
                    if (neighborX != gridX || neighborY != gridY)
                    {
                        wallCount += localGrid[neighborX, neighborY];
                    }
                }   
                else
                {
                    //encourages growth at map edges. Could just as well do without, or have conditions
                    //to encourage growth at particular edges, rather than all
                    wallCount++;
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
                    //Setting all map borders to 1, not likely to be the case for an ocean map
                    localGrid[x, y] = 1;
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
                
                if (neighborWallTiles > 4)
                {
                    localGrid[x, y] = 1;
                }
                else if (neighborWallTiles < 4) 
                {
                    localGrid[x, y] = 0;
                }
            }
        }
    }
}
