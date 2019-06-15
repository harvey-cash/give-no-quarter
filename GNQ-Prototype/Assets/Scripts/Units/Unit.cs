using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Build a Unit Workshop!
    public static Unit Build(District district, Tile tile, Player player) {
        GameObject unitObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        unitObject.transform.parent = district.transform;
        unitObject.transform.localScale = Vector3.one * 0.5f;
        unitObject.transform.localPosition = tile.coords;

        Unit unit = unitObject.AddComponent<Unit>();
        unit.Initialise(player);
        tile.unit = unit;

        return unit;
    }

    public PlayerEnum team { private set; get; }

    public int health { private set; get; }
    public int attack { private set; get; }
    public int moveRange { private set; get; }
    public int attackRange { private set; get; }
    public int viewRange { private set; get; }

    private Color normalUnitColor, highlightUnitColor, selectUnitColor;

    private void Start() {
        Collider collider = GetComponent<Collider>();
        Destroy(collider);
    }

    public virtual void Initialise(Player player) {
        this.team = player.team;

        normalUnitColor = player.normalUnitColor;
        highlightUnitColor = player.highlightUnitColor;
        selectUnitColor = player.selectUnitColor;

        Highlight(HighlightEnum.NORMAL);

        // Change depending on Unit class
        health = 1;
        attack = 1;
        moveRange = 1;
        attackRange = 1;
        viewRange = 1;
    }

    // Contextually choose what to do
    public void ActUpon(Player actingPlayer, Tile currentTile, Tile targetTile, out bool success) {

        if (targetTile.unit == null) {
            currentTile.unit = null;
            transform.localPosition = targetTile.coords;
            targetTile.unit = this;
        }
        else {
            // Attack enemy?
        }

        success = true;
    }

    public void Highlight(HighlightEnum select) {
        if (select == HighlightEnum.NORMAL) { GetComponent<Renderer>().material.color = normalUnitColor; }
        if (select == HighlightEnum.HIGHLIGHT) { GetComponent<Renderer>().material.color = highlightUnitColor; }
        if (select == HighlightEnum.FOCUS) { GetComponent<Renderer>().material.color = selectUnitColor; }
    }
}
