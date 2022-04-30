using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationResetter : MonoBehaviour
{
    // References
    [SerializeField] private Transform tf_cameraOffset;

    public void Recenter() {
        Transform cam = Camera.main.transform;
        float yawOffset = -cam.localEulerAngles.y;
        float ang = -yawOffset * Mathf.Deg2Rad;
        float sinAng = Mathf.Sin(ang);
        float cosAng = Mathf.Cos(ang);
        float xOffset = cam.localPosition.z * sinAng - cam.localPosition.x * cosAng;
        float zOffset = -cam.localPosition.z * cosAng - cam.localPosition.x * sinAng;

        tf_cameraOffset.localPosition = new Vector3(xOffset, 0, zOffset);
        tf_cameraOffset.localEulerAngles = new Vector3(0, yawOffset, 0);
    }

    private void Update() {
        if (InputManager.Instance.GetButtonDown_ResetOrientation())
            Recenter();
    }
}
