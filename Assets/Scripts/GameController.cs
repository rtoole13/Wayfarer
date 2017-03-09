using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance = null;

    public BoardController boardScript;

    [HideInInspector]
    public bool playerTurn = true;

    public int playerHealth = 100;
	// Update is called once per frame
	void Awake ()
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
        InitializeGame();
	}
    void Update()
    {
        if (playerTurn)
        {
            return;
        }
        if (Input.GetKeyDown("e"))
        {
            
            EnemyAction();

            //Hacky ass beginning of player turn. Calling on board to tell player to begin turn
            boardScript.BeginPlayerTurn();
        }
        
    }
    void EnemyAction()
    {
        //Insert enemy activity in here
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
