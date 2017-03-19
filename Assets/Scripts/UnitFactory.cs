using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoBehaviour{

    
    private GameObject hexMap;
    private GameObject[,] mapMesh;
    public GameObject hexPrefab;
    public GameObject shipPrefab;

    public void Initialize(GameObject hexMap, GameObject[,] mapMesh)
    {
        this.hexMap = hexMap;
        this.mapMesh = mapMesh;
    }

    public HumanUnitController CreateShip(Node shipNode,int spawnDirX, int spawnDirY)
    {
        GameObject newShip = (GameObject)Instantiate(shipPrefab, shipNode.worldPosition, Quaternion.identity);

        HumanUnitController shipController = newShip.GetComponent<HumanUnitController>();
        shipController.Initialize(shipNode, spawnDirX, spawnDirY);

        return shipController;
    }

    public GameObject CreateHex(int x, int y, float worldXOffset, float worldYOffset)
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

    public GameObject[,] CreateHexMesh(int boardWith, int boardHeight)
    {
        // Initialize map Mesh, intialize values of offset from grid integer coordinates to world coordinates
        float xEvenOffset = Mathf.Sqrt(3) / 2;
        float yEvenOffset = 3 / 4f;

        for (int x = 0; x < boardWith; x++)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                mapMesh[x, y] = CreateHex(x, y, xEvenOffset, yEvenOffset);
            }
        }
        return mapMesh;
    }
}
