using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Unit
{
    public override void ActUpon(Player actingPlayer, Tile currentTile, Tile targetTile, out bool success) {
        int dX = Mathf.RoundToInt(targetTile.coords.x - currentTile.coords.x);
        int dY = Mathf.RoundToInt(targetTile.coords.z - currentTile.coords.z);

        if (Mathf.Abs(dX) == Mathf.Abs(dY)) {
            currentTile.asset = null;
            transform.localPosition = targetTile.coords + Vector3.up * transform.localScale.y / 2f;

            if (targetTile.asset != null) { Destroy(targetTile.asset.gameObject); }
            targetTile.asset = this;

            success = true;
        }
        else {
            success = false;
        }
    }

    public override (int, int)[] GetMovementDirs() {
        return new (int, int)[] { (1, 1), (1, -1), (-1, 1), (-1, -1) };
    }
}
