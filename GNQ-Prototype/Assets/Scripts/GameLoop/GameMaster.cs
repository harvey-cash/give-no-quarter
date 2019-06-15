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

    public Color normalNoMans, highlightNoMans, selectNoMans;

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
        PlayerEnum nextPlayer = stateMachine.EndTurn();
        SetActivePlayer(nextPlayer);
        UpdateState(stateMachine.state);

        // Runs once after preparations are complete
        DisableUnitColliders();
    }

    private bool finishedPrep = false;
    private void DisableUnitColliders() {
        if (!finishedPrep && stateMachine.state == GameState.PICK_US || stateMachine.state == GameState.PICK_THEM) {
            finishedPrep = true;

            for (int i = 0; i < map.districts.Length; i++) {
                map.districts[i].EnableUnitColliders(false);
            }
        }
    }

    // Update UI and Map appropriately
    public void UpdateState(GameState state) {
        ui.UpdateState(stateMachine.state);
        map.UpdateState(stateMachine.state);

        if (state == GameState.TURN_THEM || state == GameState.TURN_US) {
            cameraController.SwitchDistrict(activePlayer.pickedDistrict);
            cameraController.SwitchView(false);
            activePlayer.pickedDistrict.EnableUnitColliders(true);
        }
        else {
            cameraController.SwitchDistrict(null);
            cameraController.SwitchView(true);
        }
    }

    // Called on EndTurn, update UI and move Camera appropriately
    private void SetActivePlayer(PlayerEnum player) {
        activePlayer = PlayerManager.us;
        if (player == PlayerEnum.THEM) { activePlayer = PlayerManager.them; }

        ui.UpdatePlayer(activePlayer.team);
        cameraController.SwitchPlayer(activePlayer.team);
        return;
    }

}
