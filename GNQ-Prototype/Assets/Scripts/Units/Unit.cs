using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public int health { private set; get; }
    public int attack { private set; get; }
    public int moveRange { private set; get; }
    public int attackRange { private set; get; }
    public int viewRange { private set; get; }
    
    public void Move(Tile tile) {
        if (tile.unit == null) {
            transform.position = tile.coords;
        }
        else {
            // Attack enemy?
        }
    }
}
