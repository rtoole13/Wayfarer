using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardController : MonoBehaviour {

    GameObject hexMap;
    public GameObject hexPrefab;
    TerrainGenerator terrainGen;

    public int width;
    public int height;
    
    public GameObject shipPrefab;
    public Vector2 gridSpawnLoc;
    public int spawnDirX;
    public int spawnDirY;


    UnitController playerShip;
    GameObject[,] mapMesh;
    public Node[,] grid;

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
    void CreateHexMesh()
    {
        // Initialize map Mesh, intialize values of offset from grid integer coordinates to world coordinates
        float xEvenOffset = Mathf.Sqrt(3) / 2;
        float yEvenOffset = 3 / 4f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mapMesh[x, y] = CreateHex(x, y, xEvenOffset, yEvenOffset);
            }
        }
    }
    GameObject CreateHex(int x, int y, float worldXOffset, float worldYOffset)
    {
        //from grid coords to world
        float xPos = x * worldXOffset;
        float yPos = y * worldYOffset;
        if (y % 2 == 1)
        {
            //on even rows, x offset is an additional half of x offset
            xPos += worldXOffset / 2f;
        }
        Vector3 worldPos = new Vector3(xPos, 0, yPos);
        GameObject newHex = (GameObject)Instantiate(hexPrefab, worldPos, Quaternion.identity);

        //rename hex
        newHex.name = "Hex_" + x.ToString().PadLeft(2, '0') + "_" + y.ToString().PadLeft(2, '0');
        newHex.transform.SetParent(hexMap.transform);
        return newHex;
    }

    PlayerController CreateShip()
    {
        Node shipNode = grid[Mathf.RoundToInt(gridSpawnLoc.x), Mathf.RoundToInt(gridSpawnLoc.y)];
        GameObject newShip = (GameObject)Instantiate(shipPrefab, shipNode.worldPosition, Quaternion.identity);

        PlayerController shipController = newShip.GetComponent<PlayerController>();

        shipController.Initialize(shipNode, spawnDirX, spawnDirY);

        return shipController;
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

        // Get procedural terrain generator and initialize
        terrainGen = GetComponentInChildren<TerrainGenerator>();

        // Create hexMesh
        CreateHexMesh();

        //initialize map terrain
        terrainGen.Initialize(width, height);

        // Call on the terrain generator, update hex colors and grid values
        GenerateNewMap();

        // Add player
        playerShip = CreateShip();
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
