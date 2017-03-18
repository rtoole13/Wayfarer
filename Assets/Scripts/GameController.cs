using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance = null;

    public BoardController boardScript;

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
        
        StateController.getInstance();
    }

    
    public void Start()
    {
        InitializeGame();
        //Call game start.. Ideally wouldn't occur until after all  menus have been dealt with, but for now..
        StateController.BeginGame();
    }

    void Update()
    {
        //End Player turn
        if (Input.GetKeyDown("e"))
        {
            StateController.NextTurn();
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
    void EnemyAction()
    {

        Debug.Log("Enemy actions occur.. skipping");
        //Move to player turn as there is no enemy action current. Change this later to firest execute enemy actions
        StateController.NextTurn();
    }
    void InitializeGame()
    {
        boardScript.InitializeMap();
    }
    public void GameOver()
    {
        enabled = false;
    }
}
