using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AssetCreator : MonoBehaviour, IPointerClickHandler
{
    public GameObject asset;
    public AssetEnum assetType;
    public Text quantityText;

    public void SetQuantity(Dictionary<AssetEnum, int> assetAllowance) {
        int quantity = assetAllowance[assetType];
        quantityText.text = quantity.ToString();
    }

    public void OnPointerClick(PointerEventData eventData) {
        GameMaster.game.activePlayer.OnAssetClick(this);
    }
}
