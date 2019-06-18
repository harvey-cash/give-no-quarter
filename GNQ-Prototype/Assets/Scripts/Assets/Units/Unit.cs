using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : Asset
{
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

    public override void GrabHighlightColors(Player player) {
        normalColor = player.normalUnitColor;
        highlightColor = player.highlightUnitColor;
        selectColor = player.selectUnitColor;
    }
}
