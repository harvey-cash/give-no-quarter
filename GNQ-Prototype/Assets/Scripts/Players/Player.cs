using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerEnum team;
    public Tile mouseOverTile, selectedTile;

    public District pickedDistrict;

    public Color teamColor;

    public void Awake() {
        if (team == PlayerEnum.US) { PlayerManager.us = this; }
        if (team == PlayerEnum.THEM) { PlayerManager.them = this; }
    }

    // ~~~~~ PICKING DISTRICTS ~~~~~~~~~ //

    public void PickDistrict(District district) {
        pickedDistrict = district;
        GameMaster.game.EndTurn();
    }

    // ~~~~~~~~ HIGHLIGHTING & MOUSE ~~~~~~~~~~ //

    public Color normalUnitColor, highlightUnitColor, selectUnitColor;
    public Color normalTileColor, highlightTileColor, selectTileColor;    

}
