using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitController : UnitController {

    System.Random rnjesus = new System.Random();

    public override void BeginTurn()
    {
        isMoving = true;
        base.BeginTurn();
        Vector2 targetIndex = walkables[rnjesus.Next(0, walkables.Count - 1)];
        Node targetNode = Utilities.NodeFromGridIndex(targetIndex);
        if (targetNode == null || targetNode.occupied)
        {
            return;
        }
        PathRequestManager.RequestPath(nodeLocation, targetNode, xDir, yDir, OnPathFound);
    }
}
