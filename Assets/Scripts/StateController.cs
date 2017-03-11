using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController
{
    private static volatile StateController instance = new StateController();
    private static States currentState;
    private static int roundNum;

    private StateController() { }

    public static StateController getInstance()
    {
        instance = new StateController();
        instance.SetCurrentState(States.Initial);
        return instance;
    }

    public static void BeginGame()
    {
        if (instance.GetCurrentState() == States.Initial || instance.GetCurrentState() == States.End)
        {
            instance.SetCurrentState(States.PlayerTurn);
            instance.ResetRoundNum();
            return;
        }
    }

    public static void NextTurn()
    {
        if(instance.GetCurrentState() == States.EnemyTurn)
        {
            instance.SetCurrentState(States.PlayerTurn);
            instance.IncrementRound();
            return;
        }

        if (instance.GetCurrentState() == States.PlayerTurn)
        {
            instance.SetCurrentState(States.EnemyTurn);
            return;
        }
    }

    internal static void EndGame()
    {
        instance.SetCurrentState(States.End);
        //TODO Actually display an end game screen
        Debug.Log("--Displaying end game screen-- Press G to start game again");
        return;
    }

    internal static States GetState()
    {
        return instance.GetCurrentState();
    }

    private void SetCurrentState(States state)
    {
        currentState = state;
        Debug.Log("Current state is now: " + currentState);
    }

    private States GetCurrentState()
    {
        return currentState;
    }

    private void ResetRoundNum()
    {
        roundNum = 1;
        Debug.Log("Current round is: " + roundNum);
    }
    private void IncrementRound()
    {
        roundNum++;
        Debug.Log("Current round is: " + roundNum);
    }
}

enum States{
    Initial,
    PlayerTurn,
    EnemyTurn,
    End
};
