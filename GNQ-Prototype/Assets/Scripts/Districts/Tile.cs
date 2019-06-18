using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, ISelectable
{
    public District district { private set; get; }
    public PlayerEnum team { private set; get; }
    public Vector3 coords { private set; get; }
    public Asset asset;

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
            normalTileColor = district.map.normalNoMans;
            highlightTileColor = district.map.highlightNoMans;
            selectTileColor = district.map.selectNoMans;
        }

        Highlight(HighlightEnum.NORMAL);
    }

    private void OnMouseEnter() {
        MouseEnterTile();
    }
    private void OnMouseExit() {
        MouseExitTile();
    }
    public void OnMouseDown() {
        ClickTile();
    }

    // ~~~~~~~~~~ CLICKING TILES ~~~~~~~~~~ //

    public void ClickTile() {
        if (GameMaster.stateMachine.state == GameState.PREPARE_US
            || GameMaster.stateMachine.state == GameState.PREPARE_THEM) {

            PreparationClick();
        }
        else if (GameMaster.stateMachine.state == GameState.PICK_US
            || GameMaster.stateMachine.state == GameState.PICK_THEM) {

            MapViewClick();
        }
        else if (GameMaster.stateMachine.state == GameState.TURN_US
            || GameMaster.stateMachine.state == GameState.TURN_THEM) {

            DistrictViewClick();
        }
        else {
            throw new Exception("What other states ARE THERE!?");
        }
    }

    // ~~~~ Clicks depend on State ~~~~ //

    public void PreparationClick() {
        Player activePlayer = GameMaster.game.activePlayer;

        if (team == activePlayer.team) {
            if (asset == null && activePlayer.unInitAsset != null) {                
                activePlayer.BuildAsset(district, this);
            }
        }
    }

    public void MapViewClick() {
        Player activePlayer = GameMaster.game.activePlayer;
    }

    public void DistrictViewClick() {
        Player activePlayer = GameMaster.game.activePlayer;
        // Do we have anything selected already?

        // Nothing selected
        if (activePlayer.selectedTile == null) {
            if (asset != null && asset.team == activePlayer.team) {
                activePlayer.selectedTile = this;
                Highlight(HighlightEnum.FOCUS);
            }
        }
        // Some tile of some (friendly) asset is selected
        else {
            Asset asset = activePlayer.selectedTile.asset;
            asset.ClickSomeTile(activePlayer, activePlayer.selectedTile, this, out bool nowDeselect);
            if (nowDeselect) {
                activePlayer.selectedTile.Highlight(HighlightEnum.NORMAL);
                activePlayer.selectedTile = null;
            }
        }
    }

    // ~~~~~~~~ HIGHLIGHTING & MOUSE ~~~~~~~~~~ //

    public void MouseEnterTile() {
        Player activePlayer = GameMaster.game.activePlayer;

        // Deal with old mouseOverTile first
        if (activePlayer.selectedTile != activePlayer.mouseOverTile) {
            Highlight(HighlightEnum.NORMAL);
        }
        activePlayer.mouseOverTile = this;

        if (activePlayer.selectedTile == this) {
            Highlight(HighlightEnum.FOCUS);
        }
        else { Highlight(HighlightEnum.HIGHLIGHT); }
    }

    public void MouseExitTile() {
        Player activePlayer = GameMaster.game.activePlayer;

        if (activePlayer.selectedTile != this) { Highlight(HighlightEnum.NORMAL); }
        if (activePlayer.mouseOverTile == this) { activePlayer.mouseOverTile = null; }
    }

    public void Highlight(HighlightEnum select) {
        if (asset != null) { asset.Highlight(select); }

        if (select == HighlightEnum.NORMAL) { GetComponent<Renderer>().material.color = normalTileColor; }
        if (select == HighlightEnum.HIGHLIGHT) { GetComponent<Renderer>().material.color = highlightTileColor; }
        if (select == HighlightEnum.FOCUS) { GetComponent<Renderer>().material.color = selectTileColor; }

        if (select == HighlightEnum.DEFOCUS) { GetComponent<Renderer>().material.color = district.map.deselectTile; }
    }
}
