using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : SelectableObject
{
    public District district { private set; get; }
    public PlayerEnum team { private set; get; }
    public Vector3 coords { private set; get; }
    public Asset asset;

    public override void GrabHighlightColors(Player player) {
        // normal, defocus, selected, highlighted, pathed, ranged, notRanged
        if (player != null) {
            SetColors(
                player.normalTileColor,
                GameMaster.game.map.defocusTile,
                player.selectTileColor,
                player.highlightTileColor,
                GameMaster.game.map.pathTile,
                GameMaster.game.map.rangeTile,
                GameMaster.game.map.notRangeTile
            );
        }
        else {
            SetColors(
                GameMaster.game.map.normalNoMans,
                GameMaster.game.map.defocusTile,
                GameMaster.game.map.selectNoMans,
                GameMaster.game.map.highlightNoMans,
                GameMaster.game.map.pathTile,
                GameMaster.game.map.rangeTile,
                GameMaster.game.map.notRangeTile
            );
        }
    }

    public void Initialise(District district, Vector3 coords, Player player) {
        this.district = district;
        this.coords = coords;
        if (player != null) { team = player.team; }
        GrabHighlightColors(player);
        SetNormal();
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
        // Moving placed assets
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
                asset.OnSelect(district, this);

                DoSelect(true);
                asset.DoSelect(true);
            }
        }
        // Some tile of some (friendly) asset is selected
        else {
            Asset activeAsset = activePlayer.selectedTile.asset;
            if (activeAsset.team != activePlayer.team) { throw new Exception("What the hell!?"); }

            activeAsset.ClickSomeTile(activePlayer, activePlayer.selectedTile, this, out bool nowDeselect);
            if (activePlayer.selectedTile != null && nowDeselect) {
                activePlayer.selectedTile.DoSelect(false);
                activeAsset.DoSelect(false);
                activePlayer.selectedTile = null;
            }
        }
    }

    // ~~~~~~~~ HIGHLIGHTING & MOUSE ~~~~~~~~~~ //

    public void MouseEnterTile() {
        DoHover(true);
        if (asset != null) { asset.DoHover(true); }
    }
    public void MouseExitTile() {
        DoHover(false);
        if (asset != null) { asset.DoHover(false); }
    }

    // ~~~~~~~~ BUILD ~~~~~~~~ //

    private static float width = 0.9f, height = 0.1f;
    public static Tile Build(District district, Vector3 pos, Player team) {
        GameObject tileObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tileObject.name = "Tile";
        tileObject.transform.parent = district.transform;
        tileObject.transform.localScale = new Vector3(width, height, width);
        tileObject.transform.localPosition = pos;
        tileObject.GetComponent<BoxCollider>().size = new Vector3(1 / width, 1, 1 / width);

        Tile tile = tileObject.AddComponent<Tile>();
        tile.Initialise(district, pos, team);
        return tile;
    }
}
