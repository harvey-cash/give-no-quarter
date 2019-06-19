using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : Asset
{
    private int moveRange = 4;
    public override void OnSelect(District district, Tile tile) {
        List<Tile> validTargetTiles = new List<Tile>();
        (int, int) tileCoords = (Mathf.RoundToInt(tile.coords.x), Mathf.RoundToInt(tile.coords.z));

        (int, int)[] directions = GetMovementDirs();
        for (int i = 0; i < directions.Length; i++) {
            (int, int) dir = directions[i];

            // Step in each direction, until edge of district / some asset is encountered
            for (int j = 1; j <= moveRange; j++) {
                (int, int) coords = (tileCoords.Item1 + j * dir.Item1, tileCoords.Item2 + j * dir.Item2);

                // Does tile exist at coords?
                if (district.tiles.TryGetValue((coords.Item1, coords.Item2), out Tile targetTile)) {
                    // Something there, break the loop
                    if (targetTile.asset != null) {
                        if (targetTile.asset.team == team) { validTargetTiles.Add(targetTile); }
                        break;
                    }
                    else {
                        validTargetTiles.Add(targetTile);
                        continue;
                    }
                }
                else {
                    break;
                }
            }
        }

        for (int i = 0; i < validTargetTiles.Count; i++) {
            //validTargetTiles[i].Highlight(HighlightEnum.HIGHLIGHT);
        }
    }

    // The full range of motion of the current unit
    public abstract (int, int)[] GetMovementDirs();

    public override void HoverOverSomeTile(Player actingPlayer, Tile currentTile, Tile targetTile) { return; }

    // Contextually choose what to do
    public override void ClickSomeTile(Player actingPlayer, Tile currentTile, Tile targetTile, out bool nowDeselect) {
        if (currentTile != targetTile) {
            ActUpon(actingPlayer, currentTile, targetTile, out nowDeselect);
        }
        else {
            nowDeselect = true;
        }
    }

    public abstract void ActUpon(Player actingPlayer, Tile currentTile, Tile targetTile, out bool success);
}
