using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Unit
{
    public override void ActUpon(Player actingPlayer, Tile currentTile, Tile targetTile, out bool success) {
        if (targetTile.asset == null) {
            currentTile.asset = null;
            transform.localPosition = targetTile.coords;
            targetTile.asset = this;
        }
        else {
            // Attack enemy?
        }

        success = true;
    }
}
