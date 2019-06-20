using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Asset : SelectableObject, IPlaceable {

    public District district;
    public PlayerEnum team { protected set; get; }

    public GameObject GetGameObject() {        
        return gameObject;
    }

    public abstract void OnSelect(District district, Tile tile);
    public abstract void HoverOverSomeTile(Player actingPlayer, Tile currentTile, Tile targetTile);
    public abstract void ClickSomeTile(Player actingPlayer, Tile currentTile, Tile targetTile, out bool nowDeselect);

    public IPlaceable InitialiseAsset(District district, Tile tile, Player player) {
        transform.parent = district.transform;
        transform.localPosition = tile.coords + Vector3.up * transform.localScale.y / 2f;

        Initialise(player);
        this.district = district;
        tile.asset = this;

        return this;
    }

    public void Initialise(Player player) {
        team = player.team;
        GrabHighlightColors(player);
        SetNormal();
    }

    public override void GrabHighlightColors(Player player) {
        SetColors(
            player.normalAssetColor,
            GameMaster.game.map.defocusAsset,
            player.selectAssetColor,
            player.highlightAssetColor,
            GameMaster.game.map.pathAsset,
            GameMaster.game.map.rangeAsset,
            GameMaster.game.map.notRangeAsset
        );
    }

    private int viewRadius = 3;
    public (int, int)[] GetViewSphere() {
        List<(int, int)> coords = new List<(int, int)>();
        for (int dX = -viewRadius; dX <= viewRadius; dX++) {
            for (int dZ = -viewRadius; dZ <= viewRadius; dZ++) {
                coords.Add((dX, dZ));
            }
        }
        return coords.ToArray();
    }
}
