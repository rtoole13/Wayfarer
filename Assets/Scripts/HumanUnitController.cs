using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanUnitController : UnitController {

    public float restartLevelDelay = 1f;


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
        //Currently player turn NOT in progress
        if (StateController.GetState() != States.PlayerTurn)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            PathRequestManager.RequestPath(nodeLocation, Utilities.NodeFromMousePosition(), OnPathFound);
        }
    }

    protected override void TakeDamage(int loss)
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

   
}
