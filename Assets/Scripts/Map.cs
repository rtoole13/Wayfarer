using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

    public GameObject hexPrefab;
    TerrainGenerator terrainGen;

    public int width;
    public int height;

    GameObject[,] mapMesh;
    public Node[,] grid;

    public List<Node> path;
    // Use this for initialization
    void Start ()
    {
        // Initialize hexMesh and grid arrays
        mapMesh = new GameObject[width, height];
        grid = new Node[width, height];

        // Create hexMesh
        CreateHexMesh(); 
        
        // Get procedural terrain generator and initialize
        terrainGen = GetComponentInChildren<TerrainGenerator>();
        terrainGen.Initialize(width, height);

        //Call on the terrain generator, update hex colors and grid values
        GenerateNewMap();

        
    }
	
    void Update()
    {
        //Cycle through generating of maps on click
        if (Input.GetMouseButtonDown(0))
        {
            GenerateNewMap();
            //Debug.Log("(" + grid[3, 3].cubeX + "," + grid[3, 3].cubeY + "," + grid[3, 3].cubeZ + ")");
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
                mapMesh[x, y] = createHex(x, y, xEvenOffset, yEvenOffset);
            }
        }
    }
    GameObject createHex(int x, int y, float worldXOffset, float worldYOffset)
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
        newHex.name = "Hex_" + x.ToString().PadLeft(2,'0') + "_" + y.ToString().PadLeft(2, '0');
        newHex.transform.SetParent(this.transform);
        return newHex;
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
                grid[x, y] = new Node(walkable, mapMesh[x, y].transform.position, x, y, mapMesh[x,y]);
                mapMesh[x, y].GetComponentInChildren<HexManager>().node = grid[x, y];
            }
        }
    }
    
    public Node NodeFromMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject hitObject = hitInfo.collider.transform.parent.gameObject;
            return hitObject.GetComponent<HexManager>().node;
        }
        return null;
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
        if (path!= null)
        {
            foreach (Node n in path)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(n.worldPosition, .25f);
            }
        }
    }
}
