using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Unit {

    public override void ActUpon(Player actingPlayer, Tile currentTile, Tile targetTile, out bool success) {
        if (targetTile.coords.x == currentTile.coords.x || targetTile.coords.z == currentTile.coords.z) {
            currentTile.asset = null;
            transform.localPosition = targetTile.coords + Vector3.up * transform.localScale.y / 2f;

            if (targetTile.asset != null) {
                Debug.Log(targetTile.asset is HeadQuarters);

                if (targetTile.asset.team != team && targetTile.asset is HeadQuarters) {                    
                    GameMaster.game.Win(team);
                }
                Destroy(targetTile.asset.gameObject);
            }
            targetTile.asset = this;

            success = true;
        }
        else {
            success = false;
        }
    }

    public override (int, int)[] GetMovementDirs() {
        return new (int, int)[] { (1, 0), (0, 1), (-1, 0), (0, -1) };
    }
}
