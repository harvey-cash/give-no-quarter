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

            if (unit == null) {
                Unit.Build(district, this, activePlayer);
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
            if (unit != null && unit.team == activePlayer.team) {
                Highlight(HighlightEnum.NORMAL);
                activePlayer.selectedTile = this;
                Highlight(HighlightEnum.FOCUS);
            }
        }
        // Something selected (own team)
        else {
            activePlayer.selectedTile.unit.ActUpon(
                activePlayer, 
                activePlayer.selectedTile, 
                this, 
                out bool success
            );

            if (success) {
                Highlight(HighlightEnum.NORMAL);
                activePlayer.selectedTile = null;
            }
        }
    }

    // ~~~~~~~~ HIGHLIGHTING & MOUSE ~~~~~~~~~~ //

    public void MouseEnterTile() {
        Player activePlayer = GameMaster.game.activePlayer;

        // Deal with old mouseOverTile first
        if (activePlayer.selectedTile 
            != activePlayer.mouseOverTile) {
            Highlight(HighlightEnum.NORMAL);
        }
        activePlayer.mouseOverTile = this;

        // New tile
        if (activePlayer.selectedTile != this) { Highlight(HighlightEnum.HIGHLIGHT); }
    }

    public void MouseExitTile() {
        Player activePlayer = GameMaster.game.activePlayer;

        if (activePlayer.selectedTile != this) { Highlight(HighlightEnum.NORMAL); }
        if (activePlayer.mouseOverTile == this) { activePlayer.mouseOverTile = null; }
    }

    public void Highlight(HighlightEnum select) {
        if (unit != null) { unit.Highlight(select); }

        if (select == HighlightEnum.NORMAL) { GetComponent<Renderer>().material.color = normalTileColor; }
        if (select == HighlightEnum.HIGHLIGHT) { GetComponent<Renderer>().material.color = highlightTileColor; }
        if (select == HighlightEnum.FOCUS) { GetComponent<Renderer>().material.color = selectTileColor; }
    }
}
