using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public bool isHuman;
    private List<UnitController> units = new List<UnitController>();

    public delegate void ShipMoving(PlayerController player);
    public event ShipMoving shipDoneMoving;

    public void Update()
    {
        if (!ShipsMoving)
        {
            if(shipDoneMoving != null)
            {
                shipDoneMoving(this);
            }
        }
    }

    public void BeginTurn()
    {
        foreach (UnitController unit in units)
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

    public bool ShipsMoving
    {
        get
        {
            foreach (UnitController unit in units)
            {
                return unit.IsMoving ? true : false;
            }
            return false;
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
