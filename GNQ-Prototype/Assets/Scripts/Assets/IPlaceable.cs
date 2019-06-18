using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaceable {

    GameObject GetGameObject();
    IPlaceable InitialiseAsset(District district, Tile tile, Player player);    

}
