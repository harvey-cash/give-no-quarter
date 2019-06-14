using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // degrees
    public float angleOffset, moveTime;
    private float startAngle;

    public void Start() {
        startAngle = angleOffset;
        transform.rotation = Quaternion.Euler(new Vector3(0, angleOffset, 0));
    }

    public void MoveCamera(PlayerEnum player) {
        startAngle = transform.rotation.eulerAngles.y;

        float targetAngle = angleOffset;
        if (player == PlayerEnum.THEM) { targetAngle = angleOffset + 180; }

        StartCoroutine(LerpCamera(targetAngle));
    }

    private IEnumerator LerpCamera(float targetAngle) {
        float time = 0;
        float t = time / moveTime;

        while (t <= 1) {
            time = time + Time.deltaTime;
            t = time / moveTime;

            float lerpAngle = Mathf.Lerp(startAngle, targetAngle, t);
            transform.rotation = Quaternion.Euler(new Vector3(0, lerpAngle, 0));

            yield return new WaitForEndOfFrame();
        }

        transform.rotation = Quaternion.Euler(new Vector3(0, targetAngle, 0));
    }
}
