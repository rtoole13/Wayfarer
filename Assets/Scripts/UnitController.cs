using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitController : MonoBehaviour {

    public float speed = .1f;
    protected int xDir = 0;
    protected int yDir = 1;
    protected Node nodeLocation;


    private int initialMovePoints = 1;
    protected int currentMovePoints = 0;
    private int maxMovePoints = 3;

    private int hexesMoved = 0;
    private int prevHexesMoved = 0;

    private bool canRotate = true;
    private List<Vector2> walkables;

    public int health;

    Node[] path;
    int targetIndex;

    // Use this for initialization
    void Start () {
        
	}

    // Update is called once per frame
    void Update () {
		
	}

    public void Initialize(Node node, int _dirX, int _dirY)
    {
        nodeLocation = node;
        xDir = _dirX;
        yDir = _dirY;
        transform.eulerAngles = new Vector3(0, RotationFromDirection(xDir, yDir), 0);
    }

    internal void BeginTurn()
    {
        
        InitializeMovepoints();
        Debug.Log("Starting movepoints " + currentMovePoints);
        FindWalkableSet();
    }

    private void FindWalkableSet()
    {
        List<Vector3> potentialList = new List<Vector3>();
        List<Vector3> trimmedList = new List<Vector3>();
        walkables = new List<Vector2>();
        var currentx = nodeLocation.cubeX;
        var currenty = nodeLocation.cubeY;
        var currentz = nodeLocation.cubeZ;
        
        if(currentMovePoints == 1)
        {
            walkables.Add(Utilities.GridFromCubeCoords(new Vector3(currentx+xDir, currenty+yDir, -xDir-yDir)));
            return;
        }

        for(int x = -currentMovePoints; x <= currentMovePoints; x++)
        {
            for(int y = -currentMovePoints; y <= currentMovePoints; y++)
            {
                for (int z = -currentMovePoints; z <= currentMovePoints; z++)
                {
                    if(x == 0 && y == 0 && z == 0)
                    {
                        continue;
                    }
                    if(x+y+z == 0)
                    {
                        potentialList.Add(new Vector3(x, y, z));
                    }
                }
            }
        }

        foreach (Vector3 potential in potentialList)
        {
            //Northeast
            if (xDir == 0 && yDir == 1)
            {
                if(potential.y >= 0 && potential.z <= 0)
                {
                    trimmedList.Add(potential);
                }
            }
            //Dueeast
            if (xDir == 1 && yDir == 0)
            {
                if (potential.x >= 0 && potential.z <= 0)
                {
                    trimmedList.Add(potential);
                }
            }
            //Southeast
            if (xDir == 1 && yDir == -1)
            {
                if (potential.x >= 0 && potential.y <= 0)
                {
                    trimmedList.Add(potential);
                }
            }
            //Southwest
            if (xDir == 0 && yDir == -1)
            {
                if (potential.y <= 0 && potential.z >= 0)
                {
                    trimmedList.Add(potential);
                }
            }
            //Duewest
            if (xDir == -1 && yDir == 0)
            {
                if (potential.x <= 0 && potential.z >= 0)
                {
                    trimmedList.Add(potential);
                }
            }
            //Northwest
            if (xDir == -1 && yDir == 1)
            {
                if (potential.y >= 0 && potential.x <= 0)
                {
                    trimmedList.Add(potential);
                }
            }
        }

        foreach (Vector3 trimmed in trimmedList)
        {
            walkables.Add(Utilities.GridFromCubeCoords(new Vector3(currentx + trimmed.x, currenty + trimmed.y, currentz + trimmed.z)));
        }

    }

    internal void EndTurn()
    {
        walkables = new List<Vector2>();
    }

    private void InitializeMovepoints()
    {
        canRotate = true;
        currentMovePoints = hexesMoved + initialMovePoints;
        if (currentMovePoints > maxMovePoints)
        {
            currentMovePoints = maxMovePoints;
        }
        hexesMoved = 0;
    }

    protected void OnPathFound(Node[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            if (currentMovePoints < path.Length - 1)
            {
                return;
            }
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
            hexesMoved += path.Length-1;   
            currentMovePoints -= path.Length - 1;
            if (prevHexesMoved == 0)
            {
                canRotate = false;
            }
        }
    }

    protected IEnumerator FollowPath()
    {
        Node currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint.worldPosition)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    targetIndex = 0;
                    path = new Node[0];
                    yield break;
                }
                currentWaypoint = path[targetIndex];
                int xNewDir = currentWaypoint.cubeX - nodeLocation.cubeX;
                int yNewDir = currentWaypoint.cubeY - nodeLocation.cubeY;
                if (xNewDir != xDir || yNewDir != yDir)
                {
                    canRotate = false;
                    xDir = xNewDir;
                    yDir = yNewDir;
                    transform.eulerAngles = new Vector3(0, RotationFromDirection(xDir, yDir), 0);
                }
                nodeLocation = currentWaypoint;
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.worldPosition, speed);
            yield return null;
        }
    }

    protected virtual int RotationFromDirection(int dirX, int dirY)
    {
        int rotAngle = 0; //default at NE
        if (dirX == 0 && dirY == 1)
        {
            //NE
            rotAngle = 0;
        }
        else if (dirX == 1 && dirY == 0)
        {
            //E
            rotAngle = 60;
        }
        else if (dirX == 1 && dirY == -1)
        {
            //SE
            rotAngle = 120;
        }
        else if (dirX == 0 && dirY == -1)
        {
            //SW
            rotAngle = 180;
        }
        else if (dirX == -1 && dirY == 0)
        {
            //W
            rotAngle = 240;
        }
        else if (dirX == -1 && dirY == 1)
        {
            //NW
            rotAngle = 300;
        }
        else
        {
            throw new System.Exception("Inappropriately formated direction vector. x and y coordinates must be -1, 0, or 1.");
        }
        return rotAngle;
    }

    public List<Vector2> Walkables
    {
        get
        {
            return walkables;
        }
    }

    protected virtual void TakeDamage(int loss)
    {
        health -= loss;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public Node GetNodeLocation()
    {
        return nodeLocation;
    }
}
