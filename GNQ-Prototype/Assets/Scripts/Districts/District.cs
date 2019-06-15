using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class District : MonoBehaviour
{
    public Tile[,] tiles { private set; get; }

    private Vector3 center;

    private GameObject districtBase;
    private Color districtNormal = new Color(0.8f, 0.8f, 0.8f);
    private Color districtHighlight = new Color(1f, 1f, 1f);

    // Start is called before the first frame update
    public void Initialise(int districtWidth, int districtDepth) {

        int territoryDepth = Mathf.FloorToInt(districtDepth / 2f);

        tiles = new Tile[districtWidth, districtDepth];
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
                
                tiles[i, j] = tile;
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
        districtBase.GetComponent<Renderer>().material.color = districtHighlight;
    }
    private void OnMouseExit() {
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        GetComponent<BoxCollider>().center = center;
        districtBase.GetComponent<Renderer>().material.color = districtNormal;
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
        district.Initialise(districtWidth, districtDepth);

        GameObject districtBase = GameObject.CreatePrimitive(PrimitiveType.Cube);
        districtBase.transform.parent = districtObject.transform;
        districtBase.transform.localScale = new Vector3(districtWidth + 0.5f, 1, districtDepth + 0.5f);
        districtBase.transform.localPosition = center + Vector3.down * districtBase.transform.localScale.y / 2f;
        districtBase.GetComponent<Renderer>().material.color = district.districtNormal;
        Destroy(districtBase.GetComponent<Collider>());

        district.districtBase = districtBase;
        district.center = center;

        return district;
    }
}
