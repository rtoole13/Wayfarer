using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardController : MonoBehaviour {

    GameObject hexMap;
    
    TerrainGenerator terrainGen;

    public int width;
    public int height;
    
    UnitFactory factory;
    UnitController playerShip;
    GameObject[,] mapMesh;
    public Node[,] grid;

    public List<Node> path;

    // Use this for initialization
    void Awake()
    {
        factory = GetComponent<UnitFactory>();
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

        factory.Initialize(hexMap, mapMesh);

        // Get procedural terrain generator and initialize
        terrainGen = GetComponentInChildren<TerrainGenerator>();

        // Create hexMesh
        mapMesh = factory.CreateHexMesh(width,height);
        
        //initialize map terrain
        terrainGen.Initialize(width, height);

        // Call on the terrain generator, update hex colors and grid values
        GenerateNewMap();

        
    }

    public void InitializePlayerUnits(PlayerController[] players)
    {
        foreach(PlayerController player in players)
        {
            if (player.IsHuman)
            {
                foreach (Token token in player.GetTokens())
                {
                    // Add player
                    Node playerSpawn = grid[Mathf.RoundToInt(token.gridSpawnLoc.x), Mathf.RoundToInt(token.gridSpawnLoc.y)];
                    playerShip = factory.CreateShip(playerSpawn, Mathf.RoundToInt(token.spawnDir.x), Mathf.RoundToInt(token.spawnDir.y));
                    player.AddNewUnit(playerShip);
                }
            }
            else
            {
                foreach (Token token in player.GetTokens())
                {
                    // Add player
                    Node playerSpawn = grid[Mathf.RoundToInt(token.gridSpawnLoc.x), Mathf.RoundToInt(token.gridSpawnLoc.y)];
                    var playerShip = factory.CreateAIShip(playerSpawn, Mathf.RoundToInt(token.spawnDir.x), Mathf.RoundToInt(token.spawnDir.y));
                    player.AddNewUnit(playerShip);
                }
            }
        }
        
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
        if (playerShip != null && playerShip.Walkables != null)
        {
            foreach (Vector2 walkable in playerShip.Walkables)
            {
                Gizmos.color = Color.green;
                Vector3 worldPos = grid[Mathf.RoundToInt(walkable.x), Mathf.RoundToInt(walkable.y)].worldPosition;
                Gizmos.DrawSphere(worldPos, .25f);
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
