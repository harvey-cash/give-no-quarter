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

                GameObject tileObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tileObject.transform.parent = this.transform;
                tileObject.transform.localScale = new Vector3(0.9f, 0.1f, 0.9f);
                tileObject.transform.localPosition = pos;

                Tile tile = tileObject.AddComponent<Tile>();
                tile.Initialise(this, pos, team);
                
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

    // While turns are happening, can't click on other districts!
    public void EnableColliders(bool enabled) {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++) {
            Collider collider = colliders[i];

            if (collider != pickCollider) { collider.enabled = enabled; }
        }
    }

    public void DefocusDistrict(bool defocus) {
        
        if (defocus) { Highlight(HighlightEnum.DEFOCUS); }
        else { Highlight(HighlightEnum.NORMAL); }

        SelectableObject[] selectables = GetComponentsInChildren<SelectableObject>();
        for (int i = 0; i < selectables.Length; i++) {
            selectables[i].DoFocus(!defocus);
        }
    }

    // ~~~~~~~~~~ HIGHLIGHTING ~~~~~~~~ //

    public void Highlight(HighlightEnum select) {
        if (select == HighlightEnum.NORMAL) { districtBase.GetComponent<Renderer>().material.color = districtNormal; }
        if (select == HighlightEnum.HIGHLIGHT) { districtBase.GetComponent<Renderer>().material.color = districtHighlight; }
        if (select == HighlightEnum.FOCUS) { return; }

        if (select == HighlightEnum.DEFOCUS) { districtBase.GetComponent<Renderer>().material.color = map.defocusDistrict; }
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
