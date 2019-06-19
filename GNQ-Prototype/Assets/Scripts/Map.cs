using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int districtCount, districtWidth, districtDepth;
    public float spacing;
    public District[] districts { private set; get; }

    public CameraControl cameraControl;

    public Vector3 center { private set; get; }
    public float totalWidth { private set; get; }

    public Color normalNoMans, highlightNoMans, selectNoMans;
    public Color defocusDistrict, defocusTile, defocusAsset;
    public Color pathTile, rangeTile, notRangeTile;
    public Color pathAsset, rangeAsset, notRangeAsset;

    private void Awake() {
        cameraControl.map = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        districts = new District[districtCount];
        for (int i = 0; i < districtCount; i++) {
            District district = District.BuildDistrict(this, i, districtWidth, districtDepth, spacing);
            districts[i] = district;
        }

        totalWidth = districts.Length * (districtWidth + spacing) - spacing;
        center = new Vector3(totalWidth / 2f, 0, districtDepth / 2f);
        cameraControl.transform.position = center;
        cameraControl.CalcOrthogSize(totalWidth);
    }

    public void FocusOnDistrict(District focusDistrict) {
        bool defocus = false;

        if (focusDistrict != null) {
            defocus = true;
            focusDistrict.DefocusDistrict(false);
        }

        for (int i = 0; i < districts.Length; i++) {
            if (districts[i] != focusDistrict) { districts[i].DefocusDistrict(defocus); }
        }
    }

    public void UpdateState(GameState state) {
        bool enabled = (state == GameState.PICK_THEM || state == GameState.PICK_US);

        for (int i = 0; i < districts.Length; i++) {
            districts[i].GetComponent<Collider>().enabled = enabled;
        }
    }
}
