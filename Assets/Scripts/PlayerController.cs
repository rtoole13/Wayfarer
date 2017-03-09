﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float restartLevelDelay = 1f;
    public float speed = .1f;

    private Node nodeLocation;
    private int xDir = 0;
    private int yDir = 1;
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
            PathRequestManager.RequestPath(nodeLocation, Utilities.NodeFromMousePosition(), OnPathFound);
        }
    }
    
    public void OnPathFound(Node[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");

            //Will normally be set after movement and other turn actions are complete
            EndTurn();
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
    public void BeginTurn()
    {
        //calculate stuff 
        GameController.instance.playerTurn = true;
    }
    void EndTurn()
    {
        //calculate stuff as needed at end of turn
        GameController.instance.playerTurn = false;
    }
    public void Initialize(Node node, int _dirX, int _dirY)
    {
        nodeLocation = node;
        transform.Rotate(Vector3.up, RotationFromDirection(_dirX, _dirY));

        xDir = _dirX;
        yDir = _dirY;
    }
    
    private void Restart()
    {
        // reload map
    }

    private int RotationFromDirection(int dirX, int dirY)
    {
        int rotAngle = 0; //default at NE
        if (dirX == 0 && dirY == 1)
        {
            //NE
            rotAngle = 0;
        }
        else if(dirX == 1 && dirY == 0)
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
            throw new Exception("Inappropriately formated direction vector. x and y coordinates must be -1, 0, or 1.");
        }

        return rotAngle;
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