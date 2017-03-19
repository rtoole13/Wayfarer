using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController :MonoBehaviour{

    public bool isHuman;
    private List<UnitController> units = new List<UnitController>();

    public void BeginTurn()
    {
        //Loop over all units and call unit.beginTurn();
        foreach(UnitController unit in units)
        {
            unit.BeginTurn();
        }
    }

    public void EndTurn()
    {
        foreach (UnitController unit in units)
        {
            unit.EndTurn();
        }
    }

    public bool IsHuman
    {
        get
        {
            return isHuman;
        }
    }

    internal Token[] GetTokens()
    {
        return GetComponents<Token>();
    }

    internal void AddNewUnit(UnitController playerShip)
    {
        units.Add(playerShip);
    }
}
