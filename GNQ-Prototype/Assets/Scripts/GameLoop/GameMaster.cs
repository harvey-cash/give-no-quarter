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
    public GameState state;

    public Color normalNoMans, highlightNoMans, selectNoMans;

    void Start()
    {
        state = GameState.PREPARE;
        activePlayer = PlayerManager.us;
        ui.UpdatePlayer(activePlayer.team);
    }

    public void EndTurn() {

        Player nextPlayer = PlayerManager.them;
        if (activePlayer == PlayerManager.them) { nextPlayer = PlayerManager.us; }

        ui.UpdatePlayer(nextPlayer.team);
        cameraController.MoveCamera(nextPlayer.team);

        // ...

        activePlayer = nextPlayer;
    }

}
