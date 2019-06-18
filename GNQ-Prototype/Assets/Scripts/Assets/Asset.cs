using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Asset : MonoBehaviour, ISelectable, IPlaceable {
    public District district;
    public PlayerEnum team { protected set; get; }
    protected Color normalColor, highlightColor, selectColor;

    public GameObject GetGameObject() { return gameObject; }

    public abstract void ClickSomeTile(Player actingPlayer, Tile currentTile, Tile targetTile, out bool nowDeselect);

    public IPlaceable InitialiseAsset(District district, Tile tile, Player player) {
        transform.parent = district.transform;
        transform.localPosition = tile.coords + Vector3.up * transform.localScale.y / 2f;
        //GetComponent<Collider>().enabled = true;

        Initialise(player);
        this.district = district;
        tile.asset = this;

        return this;
    }

    public void Initialise(Player player) {
        team = player.team;
        GrabHighlightColors(player);
        Highlight(HighlightEnum.NORMAL);
    }

    public abstract void GrabHighlightColors(Player player);

    public void Highlight(HighlightEnum select) {
        if (select == HighlightEnum.NORMAL) { GetComponent<Renderer>().material.color = normalColor; }
        if (select == HighlightEnum.HIGHLIGHT) { GetComponent<Renderer>().material.color = highlightColor; }
        if (select == HighlightEnum.FOCUS) { GetComponent<Renderer>().material.color = selectColor; }

        if (select == HighlightEnum.DEFOCUS) { GetComponent<Renderer>().material.color = district.map.deselectUnit; }
    }
}
