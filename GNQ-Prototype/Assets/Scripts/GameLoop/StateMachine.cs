using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public int TURNS_PER_ROUND;
    public GameState state { private set; get; }

    public int roundCounter, turnCounter;

    public StateMachine(int turnsPerRound, GameState initialState) {
        TURNS_PER_ROUND = turnsPerRound;
        state = initialState;
        roundCounter = 0;
        turnCounter = 0;
    }

    public PlayerEnum EndTurn() {
        if (state == GameState.PREPARE_US) { state = GameState.PREPARE_THEM; return PlayerEnum.THEM; }

        // For now, assume always US then THEM
        if (state == GameState.PREPARE_THEM) {
            state = GameState.PICK_US;
            roundCounter++;
            return PlayerEnum.US;
        }
        if (state == GameState.PICK_US) {
            state = GameState.PICK_THEM;
            return PlayerEnum.THEM;
        }

        if (state == GameState.PICK_THEM) { state = GameState.TURN_US; return PlayerEnum.US; }

        if (state == GameState.TURN_US) {
            turnCounter++;
            if (turnCounter < TURNS_PER_ROUND) { state = GameState.TURN_THEM; return PlayerEnum.THEM; }
            else {
                roundCounter++;
                turnCounter = 0;
                state = GameState.PICK_US;
                return PlayerEnum.US;
            }
        }

        if (state == GameState.TURN_THEM) {
            turnCounter++;
            if (turnCounter < TURNS_PER_ROUND) { state = GameState.TURN_US; return PlayerEnum.US; }
            else {
                roundCounter++;
                turnCounter = 0;
                state = GameState.PICK_US;
                return PlayerEnum.US;
            }
        }

        throw new Exception("Invalid game state?");
    }
}
