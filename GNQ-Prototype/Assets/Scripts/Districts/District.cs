using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class District : MonoBehaviour
{
    public Tile[,] tiles { private set; get; }

    // Start is called before the first frame update
    void Start()
    {
        int x = Mathf.RoundToInt(transform.localScale.x);
        int z = Mathf.RoundToInt(transform.localScale.z);

        int territoryDepth = Mathf.FloorToInt(z / 2f);

        tiles = new Tile[x, z];
        for (int i = 0; i < x; i++) {
            for (int j = 0; j < z; j++) {
                Vector3 pos = new Vector3(i, 0, j);
                PlayerEnum affiliation;
                if (j < territoryDepth) { affiliation = PlayerEnum.US; }
                else if (j >= z - territoryDepth) { affiliation = PlayerEnum.THEM; }
                else { affiliation = PlayerEnum.NEITHER; }

                GameObject tileObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tileObject.transform.localScale = new Vector3(0.9f, 0.1f, 0.9f);
                tileObject.transform.localPosition = pos;

                Tile tile = tileObject.AddComponent<Tile>();
                tile.Initialise(pos, affiliation);
                
                tiles[i, j] = tile;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
