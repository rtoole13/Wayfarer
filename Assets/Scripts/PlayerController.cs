using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float restartLevelDelay = 1f;
    public float speed = .1f;
    private Node nodeLocation;

    private BoardController boardController;
    private int health;

    Node[] path;
    int targetIndex;

    // Use this for initialization
    void Start () {
        health = GameController.instance.playerHealth;
	}
	
    private void OnDisable()
    {
        GameController.instance.playerHealth = health;
    }
	// Update is called once per frame
	void Update () {
        if (!GameController.instance.playerTurn) return; 

        if (Input.GetMouseButtonDown(0))
        {
            PathRequestManager.RequestPath(nodeLocation, boardController.NodeFromMousePosition(), OnPathFound);
        }
	}
    
    public void OnPathFound(Node[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Node currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint.worldPosition)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    targetIndex = 0;
                    path = new Node[0];
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            nodeLocation = currentWaypoint;
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.worldPosition, speed);
            yield return null;
        }
    }
    public void Initialize(BoardController _boardController, Node node)
    {
        boardController = _boardController;
        
        nodeLocation = node;
    }
    
    private void Restart()
    {
        // reload map
    }

    public void TakeDamage(int loss)
    {
        health -= loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (health <= 0)
        {
            GameController.instance.GameOver();
        }
    }

    public Node GetNodeLocation()
    {
        return nodeLocation;
    }
}
