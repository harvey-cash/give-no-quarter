using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int districtCount, districtWidth, districtDepth;
    public float spacing;
    public District[] districts { private set; get; }

    public CameraControl cameraControl;

    // Start is called before the first frame update
    void Start()
    {
        districts = new District[districtCount];
        for (int i = 0; i < districtCount; i++) {
            District district = District.BuildDistrict(this, i, districtWidth, districtDepth, spacing);
            districts[i] = district;
        }

        float totalLength = districts.Length * (districtWidth + spacing) - spacing;
        cameraControl.transform.position = new Vector3(totalLength / 2f, 0, districtDepth / 2f);
        float orthogSize = totalLength / 4f + 3;
        cameraControl.GetComponentInChildren<Camera>().orthographicSize = orthogSize;
    }

    public void UpdateState(GameState state) {
        bool enabled = (state == GameState.PICK_THEM || state == GameState.PICK_US);

        for (int i = 0; i < districts.Length; i++) {
            districts[i].GetComponent<Collider>().enabled = enabled;
        }
    }
}
