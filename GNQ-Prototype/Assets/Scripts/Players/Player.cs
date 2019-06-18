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

    private void Start() {
        InitAssetAllowances();
    }

    private void Update() {
        MoveUnInitAsset();
    }

    // ~~~~~~ PLACING DISTRICTS ~~~~~~~~~ //
    public Dictionary<AssetEnum, int> assetAllowance = new Dictionary<AssetEnum, int>();
    public void InitAssetAllowances() {
        for (int i = 0; i < GameMaster.game.assetTypes.Count; i++) {
            assetAllowance[GameMaster.game.assetTypes[i]] = GameMaster.game.assetAllowances[i];
        }
        GameMaster.game.ui.UpdateAssetQuantities(assetAllowance);
    }

    public GameObject unInitAsset;
    private AssetCreator assetCreator;

    public void OnAssetClick(AssetCreator creator, AssetEnum assetType) {
        if (assetAllowance[assetType] <= 0) { return; }

        if (unInitAsset != null) { Destroy(unInitAsset); }
        else {
            assetCreator = creator;
            unInitAsset = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(unInitAsset.GetComponent<Collider>());
        }
    }

    private void MoveUnInitAsset() {
        if (unInitAsset != null) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                Vector3 pos = new Vector3(
                    Mathf.RoundToInt(hit.point.x),
                    hit.point.y + unInitAsset.transform.localScale.y / 2f, 
                    Mathf.RoundToInt(hit.point.z)
                );
                unInitAsset.transform.position = pos;
            }
        }
    }

    public void BuiltUnit() {
        Destroy(unInitAsset);
        assetAllowance[assetCreator.assetType]--;
        GameMaster.game.ui.UpdateAssetQuantities(assetAllowance);
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
