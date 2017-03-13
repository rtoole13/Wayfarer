using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitController : MonoBehaviour {

    public float speed = .1f;
    protected int xDir = 0;
    protected int yDir = 1;
    protected Node nodeLocation;

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

    protected void OnPathFound(Node[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
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
