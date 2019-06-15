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

    public Player activePlayer { private set; get; }
    public static StateMachine stateMachine;

    public Color normalNoMans, highlightNoMans, selectNoMans;

    void Start()
    {
        stateMachine = new StateMachine(6, GameState.PREPARE_US);
        SetActivePlayer(PlayerEnum.US);
    }

    private void SetActivePlayer(PlayerEnum player) {
        activePlayer = PlayerManager.us;
        if (player == PlayerEnum.THEM) { activePlayer = PlayerManager.them; }

        ui.UpdatePlayer(activePlayer.team);
        cameraController.MoveCamera(activePlayer.team);
        return;
    }

    public void EndTurn() {
        PlayerEnum nextPlayer = stateMachine.EndTurn();
        SetActivePlayer(nextPlayer);
    }

}
