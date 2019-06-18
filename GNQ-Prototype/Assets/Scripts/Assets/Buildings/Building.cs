using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Asset {
    public override void ClickSomeTile(Player actingPlayer, Tile currentTile, Tile targetTile, out bool nowDeselect) {
        nowDeselect = true;
    }

    public override void GrabHighlightColors(Player player) {
        normalColor = player.normalUnitColor;
        highlightColor = player.highlightUnitColor;
        selectColor = player.selectUnitColor;
    }
}
