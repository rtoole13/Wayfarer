using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance = null;

    public BoardController boardScript;

    private PlayerController[] players;
    public int playerHealth = 100;

    // Update is called once per frame
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        boardScript = GetComponent<BoardController>();
        Utilities.getInstance(boardScript);
        StateController.getInstance();
    }

    
    public void Start()
    {
        InitializeGame();
        //Call game start.. Ideally wouldn't occur until after all  menus have been dealt with, but for now..
        StateController.BeginGame();
        HumanAction();
    }

    void Update()
    {
        //End Player turn
        if (Input.GetKeyDown("e"))
        {
            if (!EndHumanTurn())
            {
                return;
            }
            StateController.NextTurn(); //Rotate to enemy turn
            EnemyAction();
        }

        if (Input.GetKeyDown("g"))
        {
            if(StateController.GetState() != States.End)
            {
                StateController.EndGame();
            }
            else
            {
                boardScript.ResetBoard();
                StateController.BeginGame();
            }
        }
    }

    private void EndEnemyTurn(PlayerController player)
    {
        player.shipDoneMoving -= EndEnemyTurn;
        player.EndTurn();
        StateController.NextTurn(); //Rotate to human turn
        HumanAction();
    }

    private bool EndHumanTurn()
    {
        foreach (PlayerController player in players)
        {
            if (player.IsHuman)
            {
                if (player.ShipsMoving)
                {
                    return false;
                }
                player.EndTurn();
                return true;
            }
        }
        //Only occurs if AI exists WITHOUT a human existing
        return true;
    }

    private void HumanAction()
    {
        foreach(PlayerController player in players)
        {
            if (player.IsHuman)
            {
                player.BeginTurn();
            }
        }
    }

    private void EnemyAction()
    {
        foreach (PlayerController player in players)
        {
            if (!player.IsHuman)
            {
                player.shipDoneMoving += EndEnemyTurn;
                player.BeginTurn();
            }
        }
    }
    void InitializeGame()
    {
        players = GetComponentsInChildren<PlayerController>();
        
        boardScript.InitializeMap();
        boardScript.InitializePlayerUnits(players);

    }
    public void GameOver()
    {
        enabled = false;
    }
}
