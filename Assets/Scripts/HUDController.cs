using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour {

    private static bool endTurn = false;
    public static bool EndTurn
    {
        get
        {
            if(endTurn)
            {
                endTurn = false;
                return true;
            }
            return false;
        }
    }

	public void EndTurnClicked()
    {
        endTurn = true;
    }

}
