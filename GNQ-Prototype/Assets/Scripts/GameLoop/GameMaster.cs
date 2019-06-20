using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster game;
    private void Awake() {
        game = this;
    }

    public UIManager ui;
    public CameraControl cameraController;
    public Map map;

    public Player activePlayer { private set; get; }
    public static StateMachine stateMachine;

    public List<AssetEnum> assetTypes;
    public List<int> assetAllowances;

    void Start()
    {
        stateMachine = new StateMachine(6, GameState.PREPARE_US);
        SetActivePlayer(PlayerEnum.US);
        StartCoroutine(SetupGame());
    }

    private IEnumerator SetupGame() {
        yield return new WaitForEndOfFrame();
        UpdateState(stateMachine.state);
    }

    // Called by player when EndTurn button is clicked
    public void EndTurn() {
        DeselectCurrentTile();

        PlayerEnum nextPlayer = stateMachine.EndTurn();
        SetActivePlayer(nextPlayer);
        UpdateState(stateMachine.state);

        // Runs once after preparations are complete
        //DisableUnitColliders();
    }

    private void DeselectCurrentTile() {
        // Cleanup, deselect current tile
        if (activePlayer.selectedTile != null) {
            activePlayer.selectedTile.DoSelect(false);
            if (activePlayer.selectedTile.asset != null) {
                activePlayer.selectedTile.asset.DoSelect(false);
            }
        }
        activePlayer.selectedTile = null;
    }

    private bool finishedPrep = false;
    private void DisableUnitColliders() {
        if (!finishedPrep && stateMachine.state == GameState.PICK_US || stateMachine.state == GameState.PICK_THEM) {
            finishedPrep = true;

            for (int i = 0; i < map.districts.Length; i++) {
                //map.districts[i].EnableColliders(false);
            }
        }
    }

    // Update UI and Map appropriately
    public void UpdateState(GameState state) {
        if (gameOver) { return; }

        ui.UpdateState(stateMachine.state);
        ui.UpdateCounters(stateMachine.roundCounter, stateMachine.turnCounter);
        map.UpdateState(stateMachine.state);
        ui.ShowPrivacyScreen(true);

        if (state == GameState.PREPARE_THEM || state == GameState.PREPARE_US) {
            ui.ShowAssetPanel(true);
            ui.UpdateAssetQuantities(activePlayer.assetAllowance);
            ui.ShowEndTurn(activePlayer.ExhaustedAllowances());
        }
        else {
            ui.ShowAssetPanel(false);
            ui.ShowEndTurn(false);
            map.DestroyWalls();
        }

        if (state == GameState.TURN_THEM || state == GameState.TURN_US) {
            cameraController.SwitchDistrict(activePlayer.pickedDistrict);
            cameraController.SwitchView(false);
            map.FocusOnDistrict(activePlayer.pickedDistrict, state);
        }
        else {
            cameraController.SwitchDistrict(null);
            cameraController.SwitchView(true);
            map.FocusOnDistrict(null, state);
        }
    }

    private bool gameOver = false;
    public void Win(PlayerEnum team) {
        gameOver = true;
        ui.ShowPrivacyScreen(false);
        ui.Win(team);
    }

    // Called on EndTurn, update UI and move Camera appropriately
    private void SetActivePlayer(PlayerEnum player) {
        activePlayer = PlayerManager.us;
        if (player == PlayerEnum.THEM) { activePlayer = PlayerManager.them; }

        activePlayer.BeginTurn();
        ui.UpdatePlayer(activePlayer.team);
        cameraController.SwitchPlayer(activePlayer.team);

        return;
    }

}
