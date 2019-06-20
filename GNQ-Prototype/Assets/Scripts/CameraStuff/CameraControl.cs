using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Map map;
    private Camera cam;

    // degrees
    public float angleOffset, moveTime;
    private float startAngle;

    public void Start() {
        cam = GetComponentInChildren<Camera>();
        startAngle = angleOffset;
        transform.rotation = Quaternion.Euler(new Vector3(0, angleOffset, 0));
    }

    // Do Mathf.Lerp on the given objects
    object FloatLerp(object a, object b, float t) { return Mathf.Lerp((float)a, (float)b, t); }

    public float CalcOrthogSize(float length) {
        return length / 4f + 3;
    }

    public void SwitchPlayer(PlayerEnum player) {
        startAngle = transform.rotation.eulerAngles.y;
        float targetAngle = angleOffset;
        if (player == PlayerEnum.THEM) { targetAngle = angleOffset + 180; }

        void Set(object v) { transform.rotation = Quaternion.Euler(new Vector3(0, (float)v, 0)); }

        StartCoroutine(LerpRoutine(FloatLerp, Set, startAngle, targetAngle, moveTime));
    }

    public void SwitchDistrict(District district) {
        Vector3 targetPosition = map.center;
        if (district != null) { targetPosition = district.transform.position + district.center; }

        object Lerp(object a, object b, float t) { return Vector3.Lerp((Vector3)a, (Vector3)b, t); }
        void Set(object v) { transform.position = (Vector3)v; }

        StartCoroutine(LerpRoutine(Lerp, Set, transform.position, targetPosition, moveTime));
    }

    public void SwitchView(bool seeMap) {
        void Set(object v) { cam.orthographicSize = (float)v; }

        float targetWidth = map.districtWidth;
        if (seeMap) { targetWidth = map.totalWidth; }

        StartCoroutine(LerpRoutine(FloatLerp, Set, cam.orthographicSize, CalcOrthogSize(targetWidth), moveTime));
    }

    private IEnumerator LerpRoutine(Func<object,object,float,object> Lerp, Action<object> Set, object a, object b, float maxTime) {
        float time = 0;
        float t = time / maxTime;

        while (t <= 1) {
            time = time + Time.deltaTime;
            t = time / maxTime;

            object lerpResult = Lerp(a, b, t);
            Set(lerpResult);

            yield return new WaitForEndOfFrame();
        }
        Set(b);
    }

    private void KeineZeitFunction(float t) {
        //
    }
}
