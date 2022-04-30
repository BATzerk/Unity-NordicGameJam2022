using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {
    // Properties
    public bool LockAxisX = false;
    public bool LockAxisY = false;
    public bool LockAxisZ = false;
    private System.Func<bool> LookAtCamera;


    // ----------------------------------------------------------------
    //  Awake
    // ----------------------------------------------------------------
    private void Awake() {
        if (LockAxisX || LockAxisY || LockAxisZ) LookAtCamera = LookAtCamera_LockedAxis;
        else LookAtCamera = LookAtCamera_NoLockedAxis;
    }


    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    void Update() {
        LookAtCamera();
    }
    private bool LookAtCamera_NoLockedAxis() {
        this.transform.LookAt(Camera.main.transform);
        return true; // Note: Not used for anything.
    }
    private bool LookAtCamera_LockedAxis() {
        Vector3 currEulerAngles = this.transform.eulerAngles;

        this.transform.LookAt(Camera.main.transform);

        if (LockAxisX) this.transform.eulerAngles = new Vector3(currEulerAngles.x, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
        if (LockAxisY) this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, currEulerAngles.y, this.transform.eulerAngles.z);
        if (LockAxisZ) this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, currEulerAngles.z);
        return true; // Note: Not used for anything.
    }
}
