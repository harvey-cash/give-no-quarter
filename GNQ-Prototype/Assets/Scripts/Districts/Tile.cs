using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public District district { private set; get; }
    public PlayerEnum team { private set; get; }
    public Vector3 coords { private set; get; }
    public Unit unit;

    private Color normalTileColor, highlightTileColor, selectTileColor;

    public void Initialise(District district, Vector3 coords, Player player) {
        this.district = district;
        this.coords = coords;

        if (player != null) {
            team = player.team;
            normalTileColor = player.normalTileColor;
            highlightTileColor = player.highlightTileColor;
            selectTileColor = player.selectTileColor;
        }
        else {
            normalTileColor = GameMaster.game.normalNoMans;
            highlightTileColor = GameMaster.game.highlightNoMans;
            selectTileColor = GameMaster.game.selectNoMans;
        }

        Highlight(HighlightEnum.NORMAL);
    }

    private void OnMouseEnter() {
        GameMaster.game.activePlayer.MouseEnterTile(this);
    }
    private void OnMouseExit() {
        GameMaster.game.activePlayer.MouseExitTile(this);
    }
    public void OnMouseDown() {
        GameMaster.game.activePlayer.ClickTile(this);
    }

    public void Highlight(HighlightEnum select) {
        if (select == HighlightEnum.NORMAL) { GetComponent<Renderer>().material.color = normalTileColor; }
        if (select == HighlightEnum.HIGHLIGHT) { GetComponent<Renderer>().material.color = highlightTileColor; }
        if (select == HighlightEnum.FOCUS) { GetComponent<Renderer>().material.color = selectTileColor; }
    }
}
