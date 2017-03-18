using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour {

    public TokenType type;
    public Vector2 gridSpawnLoc;
    public Vector2 spawnDir;



    public enum TokenType{
        Ship
    }
}
