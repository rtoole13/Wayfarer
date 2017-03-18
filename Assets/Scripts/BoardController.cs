using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardController : MonoBehaviour {

    GameObject hexMap;
    
    TerrainGenerator terrainGen;

    public int width;
    public int height;
    
    public Vector2 gridSpawnLoc;
    public int spawnDirX;
    public int spawnDirY;

    UnitFactory factory = new UnitFactory();
    UnitController playerShip;
    GameObject[,] mapMesh;
    public Node[,] grid;

    public GameObject shipPrefab;
    public GameObject hexPrefab;

    public List<Node> path;

    // Use this for initialization
    void Start()
    {

    }
	// Update is called once per frame
	void Update ()
    {
        // Cycle through generating of maps on space
        if (Input.GetKeyDown("space"))
        {
            GenerateNewMap();
        }
    }
    
    void GenerateNewMap()
    {
        // Get the terrain grid map
        int[,] terrainGrid = new int[width, height];
        terrainGrid = terrainGen.GenerateMap();

        // Color terrain and label hex unwalkable
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool walkable;

                MeshRenderer mr = mapMesh[x, y].GetComponentInChildren<MeshRenderer>();
                if (terrainGrid[x, y] == 1)
                {
                    walkable = false;
                    mr.material.color = Color.gray;
                }
                else
                {
                    walkable = true;
                    mr.material.color = Color.white;
                }
                grid[x, y] = new Node(walkable, mapMesh[x, y].transform.position, x, y, mapMesh[x, y]);
                mapMesh[x, y].GetComponentInChildren<HexManager>().node = grid[x, y];
            }
        }
    }
    public void InitializeMap()
    {
        hexMap = new GameObject();
        hexMap.name = "hexMap";
        // Initialize hexMesh and grid arrays
        mapMesh = new GameObject[width, height];
        grid = new Node[width, height];
        factory.Initialize(hexMap, mapMesh,shipPrefab,hexPrefab);

        // Get procedural terrain generator and initialize
        terrainGen = GetComponentInChildren<TerrainGenerator>();

        // Create hexMesh
        mapMesh = factory.CreateHexMesh(width,height);
        
        //initialize map terrain
        terrainGen.Initialize(width, height);

        // Call on the terrain generator, update hex colors and grid values
        GenerateNewMap();

        // Add player
        Node playerSpawn = grid[Mathf.RoundToInt(gridSpawnLoc.x), Mathf.RoundToInt(gridSpawnLoc.y)];
        playerShip = factory.CreateShip(playerSpawn,spawnDirX,spawnDirY);
    }
    
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                if (node.gridY % 2 == 1)
                {
                    if (x == -1 && y != 0)
                    {
                        continue;
                    }
                }
                else
                {
                    if (x == 1 && y != 0)
                    {
                        continue;
                    }
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbors;
    }
    void OnDrawGizmos()
    {
        if (path != null)
        {
            foreach (Node n in path)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(n.worldPosition, .25f);
            }
        }
    }

    public void ResetBoard()
    {
        CleanBoard();
        InitializeMap();
    }

    public void CleanBoard()
    {
        Destroy(hexMap);
        playerShip.Destroy();
    }

    public Node ShipNode()
    {
        return playerShip.GetNodeLocation();
    }
}
