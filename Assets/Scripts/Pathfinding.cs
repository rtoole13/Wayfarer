using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour {

    PathRequestManager requestManager;
    BoardController boardController;

    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        boardController = GetComponent<BoardController>();
    }

    public void StartFindPath(Node startNode, Node targetNode, int xDir, int yDir)
    {
        StartCoroutine(FindPath(startNode, targetNode, xDir, yDir));
    }
    IEnumerator FindPath(Node startNode, Node targetNode, int _xDir, int _yDir)
    {
        Node[] waypoints = new Node[0];
        bool pathSuccess = false;
        int xDir = _xDir;
        int yDir = _yDir;

        if (startNode.walkable && targetNode.walkable)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        /*
                        Vector2 newDir = GetDirection(currentNode, openSet[i]);
                        xDir = Mathf.RoundToInt(newDir.x);
                        yDir = Mathf.RoundToInt(newDir.y);
                        */
                        currentNode = openSet[i];
                    }
                }
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbor in boardController.GetNeighbors(currentNode))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    //All neighbors 1 away
                    Vector2 neighborDir = new Vector2();
                    neighborDir = GetDirection(currentNode, neighbor);
                    
                    int movementCostToNeighbor = currentNode.gCost;
                    if (xDir == Mathf.RoundToInt(neighborDir.x) && yDir == Mathf.RoundToInt(neighborDir.y))
                    {
                        movementCostToNeighbor += 1;
                        Debug.Log("old dirX" + xDir + "old dirY" + yDir);
                        Debug.Log(neighborDir);

                    }
                    else
                    {
                        movementCostToNeighbor += 3;
                    }

                    if (movementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = movementCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);

                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Node[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        
        //Bypassing path simplification
        Node[] waypoints = path.ToArray();
        Array.Reverse(waypoints);
        boardController.path = path;
        return waypoints;
    }

    Vector2 GetDirection(Node currentNode, Node targetNode)
    {
        Vector2 directionNew = new Vector2(targetNode.cubeX - currentNode.cubeX, targetNode.cubeY - currentNode.cubeY);
        return directionNew;
    }

    Node[] SimplifyPath(List<Node> path)
    {
        List<Node> waypoints = new List<Node>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].cubeX - path[i].cubeX, path[i - 1].cubeY - path[i].cubeY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i - 1]);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }
    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeB.cubeX - nodeA.cubeX);
        int distY = Mathf.Abs(nodeB.cubeY - nodeA.cubeY);
        int distZ = Mathf.Abs(nodeB.cubeZ - nodeA.cubeZ);

        return Mathf.Max(distX, distY, distZ);
    }
    
}
