using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Asset {
    public override void ClickSomeTile(Player actingPlayer, Tile currentTile, Tile targetTile, out bool nowDeselect) {
        nowDeselect = true;
    }

    public override void HoverOverSomeTile(Player actingPlayer, Tile currentTile, Tile targetTile) {
        return;
    }

    public override void OnSelect(District district, Tile tile) {
        return;
    }
}
