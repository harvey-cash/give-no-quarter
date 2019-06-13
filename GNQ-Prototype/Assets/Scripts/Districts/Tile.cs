using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public PlayerEnum team { private set; get; }
    public Vector3 coords { private set; get; }
    public Unit unit;

    public void Initialise(Vector3 coords, PlayerEnum team) {
        this.coords = coords;
        this.team = team;

        Color teamColor;
        if (team == PlayerEnum.US) { teamColor = Color.green; }
        else if (team == PlayerEnum.THEM) { teamColor = Color.red; }
        else { teamColor = Color.gray; }

        GetComponent<Renderer>().material.color = teamColor;
    }
}
