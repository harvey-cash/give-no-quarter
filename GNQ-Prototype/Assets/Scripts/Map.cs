using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int districtCount, districtWidth, districtDepth;
    public float spacing;
    public District[] districts { private set; get; }

    // Start is called before the first frame update
    void Start()
    {
        districts = new District[districtCount];
        for (int i = 0; i < districtCount; i++) {
            GameObject districtObject = new GameObject("District: " + i);
            districtObject.transform.parent = this.transform;
            districtObject.transform.localPosition = Vector3.right * i * (districtWidth + spacing);

            District district = districtObject.AddComponent<District>();
            district.Initialise(districtWidth, districtDepth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
