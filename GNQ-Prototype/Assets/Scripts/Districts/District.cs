using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class District : MonoBehaviour
{
    public Tile[,] tiles { private set; get; }

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
