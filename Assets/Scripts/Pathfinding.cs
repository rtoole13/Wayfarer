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

    public void StartFindPath(Node startNode, Node targetNode)
    {
        StartCoroutine(FindPath(startNode, targetNode));
    }
    IEnumerator FindPath(Node startNode, Node targetNode)
    {
        Node[] waypoints = new Node[0];
        bool pathSuccess = false;

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
                    int movementCostToNeighbor = currentNode.gCost + 1;
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

        Node[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        boardController.path = path;
        return waypoints;
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
