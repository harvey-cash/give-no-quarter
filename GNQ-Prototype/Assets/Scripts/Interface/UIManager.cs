using System.Collections;
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

    public GameObject assetPanel;
    public void ShowAssetPanel(bool enabled) {
        assetPanel.SetActive(enabled);
    }
    public List<AssetCreator> assetCreators;
    public void UpdateAssetQuantities(Dictionary<AssetEnum, int> assetAllowance) {
        for (int i = 0; i < assetCreators.Count; i++) {
            assetCreators[i].SetQuantity(assetAllowance);
        }
    }

    public GameObject endTurnButton;
    public void ShowEndTurn(bool enabled) {
        endTurnButton.SetActive(enabled);
    }

    public GameObject privacyScreen;
    public void ShowPrivacyScreen(bool enabled) {
        privacyScreen.SetActive(enabled);
    }

    private void Update() {
        if (Input.GetKeyUp(KeyCode.Space)) {
            ShowPrivacyScreen(false);
        }
    }

    public Text winText;
    public void Win(PlayerEnum player) {
        winText.gameObject.SetActive(true);
        winText.text = player.ToString() + " WINS!";
    }

}
