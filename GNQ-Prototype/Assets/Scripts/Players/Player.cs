using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerEnum team;
    public Tile mouseOverTile, selectedTile;

    public Color teamColor;

    public void Awake() {
        if (team == PlayerEnum.US) { PlayerManager.us = this; }
        if (team == PlayerEnum.THEM) { PlayerManager.them = this; }
    }

    // ~~~~~~~~~~ PLAYER ACTIONS ~~~~~~~~~~ //

    public void ClickTile(Tile targetTile) {
        if (GameMaster.stateMachine.state == GameState.PREPARE_US 
            || GameMaster.stateMachine.state == GameState.PREPARE_THEM) {

            PreparationClick(targetTile);
        }
        else if (GameMaster.stateMachine.state == GameState.PICK_US
            || GameMaster.stateMachine.state == GameState.PICK_THEM) {

            MapViewClick(targetTile);
        }
        else if (GameMaster.stateMachine.state == GameState.TURN_US
            || GameMaster.stateMachine.state == GameState.TURN_THEM) {

            DistrictViewClick(targetTile);
        }
        else {
            throw new Exception("What other states ARE THERE!?");
        }
    }

    public void PreparationClick(Tile targetTile) {
        if (targetTile.team == team) {

            if (targetTile.unit == null) {
                Unit.Build(targetTile.district, targetTile, this);
            }
        }
    }

    public void MapViewClick(Tile targetTile) {

    }

    public void DistrictViewClick(Tile targetTile) {
        // Do we have anything selected already?

        // Nothing selected
        if (selectedTile == null) {
            if (targetTile.unit != null && targetTile.unit.team == team) {
                Highlight(selectedTile, HighlightEnum.NORMAL);
                selectedTile = targetTile;
                Highlight(targetTile, HighlightEnum.FOCUS);
            }
        }
        // Something selected (own team)
        else {
            selectedTile.unit.ActUpon(this, selectedTile, targetTile, out bool success);

            if (success) {
                Highlight(selectedTile, HighlightEnum.NORMAL);
                selectedTile = null;
            }
        }
    }


    // ~~~~~~~~ HIGHLIGHTING & MOUSE ~~~~~~~~~~ //

    public Color normalUnitColor, highlightUnitColor, selectUnitColor;
    public Color normalTileColor, highlightTileColor, selectTileColor;    

    public void MouseEnterTile(Tile tile) {
        // Deal with old mouseOverTile first
        if (selectedTile != mouseOverTile) { Highlight(mouseOverTile, HighlightEnum.NORMAL); }
        mouseOverTile = tile;

        // New tile
        if (selectedTile != tile) { Highlight(tile, HighlightEnum.HIGHLIGHT); }            
    }

    public void MouseExitTile(Tile tile) {
        if (selectedTile != tile) { Highlight(tile, HighlightEnum.NORMAL); }
        if (mouseOverTile == tile) { mouseOverTile = null; }
    }

    public void Highlight(Tile tile, HighlightEnum select) {
        if (tile == null || tile.unit == null) { return; }
        tile.Highlight(select);
        tile.unit.Highlight(select);
    }
}
