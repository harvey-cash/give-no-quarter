using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class District : MonoBehaviour
{
    public Map map { private set; get; }
    //public Tile[,] tiles { private set; get; }
    public Dictionary<(int,int),Tile> tiles { private set; get; }

    public Vector3 center { private set; get; }
    public BoxCollider pickCollider;

    private GameObject districtBase;
    private Color districtNormal = new Color(0.8f, 0.8f, 0.8f);
    private Color districtHighlight = new Color(1f, 1f, 1f);

    // Start is called before the first frame update
    public void Initialise(Map map, int districtWidth, int districtDepth) {
        this.map = map;

        int territoryDepth = Mathf.FloorToInt(districtDepth / 2f);

        //tiles = new Tile[districtWidth, districtDepth];
        tiles = new Dictionary<(int, int), Tile>();

        for (int i = 0; i < districtWidth; i++) {
            for (int j = 0; j < districtDepth; j++) {
                Vector3 pos = new Vector3(i, 0, j);
                Player team;
                if (j < territoryDepth) { team = PlayerManager.us; }
                else if (j >= districtDepth - territoryDepth) { team = PlayerManager.them; }
                else { team = null; }

                Tile tile = Tile.Build(this, pos, team);
                
                tiles[(i, j)] = tile;
            }
        }
    }

    // Click on district to pick it!
    private void OnMouseDown() {
        GameMaster.game.activePlayer.PickDistrict(this);
    }

    // Hover over district, and it'll pop up!
    private void OnMouseEnter() {
        transform.localPosition = new Vector3(transform.localPosition.x, 0.2f, transform.localPosition.z);
        GetComponent<BoxCollider>().center = center + Vector3.down * 0.2f;
        Highlight(HighlightEnum.HIGHLIGHT);
    }
    private void OnMouseExit() {
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        GetComponent<BoxCollider>().center = center;
        Highlight(HighlightEnum.NORMAL);
    }

    // ~~~~~~~~~~~~~~~~ FOCUSING ~~~~~~~~~~~~~~~ //

    public void Focus(bool enabled, GameState state) {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i] != pickCollider) { colliders[i].enabled = true; }
        }

        if (state != GameState.PREPARE_THEM && state != GameState.PREPARE_US) {
            ApplyFogOfWar(GameMaster.game.activePlayer.team);
        }

        if (!enabled) {
            for (int i = 0; i < colliders.Length; i++) {
                if (colliders[i] != pickCollider) { colliders[i].enabled = false; }
            }
        }

        if (!enabled) { Highlight(HighlightEnum.DEFOCUS); }
        else { Highlight(HighlightEnum.NORMAL); }
    }

    // ~~~~~~~~~~ HIGHLIGHTING ~~~~~~~~ //

    public void Highlight(HighlightEnum select) {
        if (select == HighlightEnum.NORMAL) { districtBase.GetComponent<Renderer>().material.color = districtNormal; }
        if (select == HighlightEnum.HIGHLIGHT) { districtBase.GetComponent<Renderer>().material.color = districtHighlight; }
        if (select == HighlightEnum.FOCUS) { return; }

        if (select == HighlightEnum.DEFOCUS) { districtBase.GetComponent<Renderer>().material.color = map.defocusDistrict; }
    }


    // ~~~~~~~~~~~~ FOG OF WAR ~~~~~~~~~~~ //

    public void ApplyFogOfWar(PlayerEnum playerTeam) {
        List<Tile> viewableTiles = new List<Tile>();

        // for each tile in district
        foreach (KeyValuePair<(int, int), Tile> coordTile in tiles) {
            if (coordTile.Value.asset != null) {
                // Friendly asset
                if (coordTile.Value.asset.team == playerTeam) {
                    Collider[] viewables = Physics.OverlapSphere(coordTile.Value.transform.position, 3f);
                    for (int i = 0; i < viewables.Length; i++) {
                        if (viewables[i].GetComponent<Tile>() != null) {
                            viewableTiles.Add(viewables[i].GetComponent<Tile>());
                        }
                    }
                }
            }
        }

        // Disable all
        foreach (KeyValuePair<(int, int), Tile> coordTile in tiles) {
            coordTile.Value.GetComponent<Collider>().enabled = false;
            coordTile.Value.DoFocus(false);
            if(coordTile.Value.asset != null) {
                coordTile.Value.asset.GetComponent<Renderer>().enabled = false;
            }
        }

        // Enable those we saw
        for (int i = 0; i < viewableTiles.Count; i++) {
            viewableTiles[i].GetComponent<Collider>().enabled = true;
            viewableTiles[i].DoFocus(true);
            if (viewableTiles[i].asset != null) {
                viewableTiles[i].asset.GetComponent<Renderer>().enabled = true;
            }
        }

    }

    // Disable all tiles in this district, then re-enable only those we can see!
    public void OLDFogOfWar(PlayerEnum playerTeam) {
        List<(int, int)> withinSightCoords = new List<(int, int)>();
        List<Tile> viewableTiles = new List<Tile>();
        List<Tile> enemyAssetTiles = new List<Tile>();

        // for each tile in district
        foreach(KeyValuePair<(int,int),Tile> coordTile in tiles) {
            // Disable ALL tiles!
            coordTile.Value.GetComponent<Collider>().enabled = false;
            coordTile.Value.DoFocus(false);

            // If tile has a friendly asset on it
            if (coordTile.Value.asset != null) {

                // Record enemy-occupied tile
                if (coordTile.Value.asset.team != playerTeam) {
                    coordTile.Value.asset.GetComponent<Renderer>().enabled = false;
                    enemyAssetTiles.Add(coordTile.Value);                    
                }

                else {
                    (int, int) tileCoords = coordTile.Key;
                    (int, int)[] viewSphere = coordTile.Value.asset.GetViewSphere();

                    // For each coordinate in view of the friendly asset
                    for (int i = 0; i < viewSphere.Length; i++) {
                        (int, int) coords = (
                            tileCoords.Item1 + viewSphere[i].Item1,
                            tileCoords.Item2 + viewSphere[i].Item2
                        );

                        // Add to list of viewable tiles
                        if (tiles.TryGetValue(coords, out Tile viewableTile)) {
                            Debug.Log(coords.ToString());
                            viewableTiles.Add(viewableTile);

                            // viewable, so re-enable!
                            viewableTile.GetComponent<Collider>().enabled = true;

                            /*
                            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            marker.transform.position = viewableTile.transform.position + Vector3.up * 0.5f;
                            marker.transform.localScale = Vector3.one * 0.1f;
                            */

                            viewableTile.GetComponent<Renderer>().material.color = Color.red;
                            viewableTile.DoFocus(true);
                            if (viewableTile.asset != null) { viewableTile.asset.GetComponent<Renderer>().enabled = true; }
                        }
                    }
                }
            }
        }
    }


    // Create a district object and return it
    public static District BuildDistrict(Map map, int districtIndex, int districtWidth, int districtDepth, float spacing) {
        GameObject districtObject = new GameObject("District: " + districtIndex);
        districtObject.transform.parent = map.transform;
        districtObject.transform.localPosition = Vector3.right * districtIndex * (districtWidth + spacing);

        Vector3 center = new Vector3(districtWidth / 2f - 0.5f, 0, districtDepth / 2f - 0.5f);        

        BoxCollider collider = districtObject.AddComponent<BoxCollider>();
        collider.center = center;
        collider.size = new Vector3(districtWidth, 1, districtDepth);

        District district = districtObject.AddComponent<District>();
        district.Initialise(map, districtWidth, districtDepth);

        GameObject districtBase = GameObject.CreatePrimitive(PrimitiveType.Cube);
        districtBase.transform.parent = districtObject.transform;
        districtBase.transform.localScale = new Vector3(districtWidth + 0.5f, 1, districtDepth + 0.5f);
        districtBase.transform.localPosition = center + Vector3.down * districtBase.transform.localScale.y / 2f;
        districtBase.GetComponent<Renderer>().material.color = district.districtNormal;
        Destroy(districtBase.GetComponent<Collider>());

        district.districtBase = districtBase;
        district.pickCollider = collider;
        district.center = center;

        return district;
    }
}
