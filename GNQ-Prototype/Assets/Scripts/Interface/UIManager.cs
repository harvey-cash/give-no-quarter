﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text currentPlayer;
    public void UpdatePlayer(PlayerEnum player) {
        currentPlayer.text = player.ToString().ToUpper();
    }

    public Text currentState;
    public void UpdateState(GameState state) {
        currentState.text = state.ToString().ToUpper();
    }

    public Text roundText, turnText;
    public void UpdateCounters(int round, int turn) {
        roundText.text = "Round: " + round;
        turnText.text = "Turn: " + turn;
    }
}
