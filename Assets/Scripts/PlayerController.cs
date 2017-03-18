using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController :MonoBehaviour{

    public bool isHuman;

    public bool IsHuman
    {
        get
        {
            return isHuman;
        }
    }

    public void AddNewShip(HumanUnitController ship)
    {

    }

    internal Token[] GetTokens()
    {
        return GetComponents<Token>();
    }
}
