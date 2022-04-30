using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mitt : MonoBehaviour
{
    // Properties
    [SerializeField] private OVRInput.Controller MyHandType;
    // References
    [SerializeField] private GameController gameController;
    private List<ProximityVibrator> proxVibsInRange = new List<ProximityVibrator>();



    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    private void Update() {
        // How intense should we vibrate?
        float vibVol = 0; // from 0 to 1.
        bool isInAVibCore = false; // TRUE if we're RIGHT inside the vibrator! If this is true, we'll go BUMP BUMP instead of vrrrrr.
        foreach (ProximityVibrator pv in proxVibsInRange) {
            float dist = Vector3.Distance(this.transform.position, pv.transform.position);
            isInAVibCore |= dist < pv.DistMin;
            float thisVol = Mathf.InverseLerp(pv.DistMax, pv.DistMin, dist);
            thisVol = Mathf.Pow(thisVol, 3); // cube it! For a much steeper dropoff.
            vibVol += thisVol;
        }
        vibVol = Mathf.Clamp01(vibVol); // just clamp it to 1. A vibration over 1 would explode the controller to smithereens.

        // Hey, if we're inside a vib's core, we do a heartbeat style instead.
        if (isInAVibCore) {
            // Rhythmically disable vibration.
            if (Mathf.Sin(Time.unscaledTime*70f) > 0) {
                vibVol = 0;
            }
        }

        // Apply vibration!
        if (vibVol > 0) {
            GameManagers.Instance.VibrationManager.Vibrate(0.01f, vibVol, MyHandType);
        }
    }


    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    private void AddProxVibInRange(ProximityVibrator pv) {
        if (!proxVibsInRange.Contains(pv))
            proxVibsInRange.Add(pv);
    }
    private void RemoveProxVibInRange(ProximityVibrator pv) {
        if (proxVibsInRange.Contains(pv))
            proxVibsInRange.Remove(pv);
    }


    // ----------------------------------------------------------------
    //  Collision Events
    // ----------------------------------------------------------------
    private void OnTriggerEnter(Collider other) {
        Debug.Log("OnTriggerEnter: " + other.gameObject);
        ProximityVibrator pv = other.gameObject.GetComponentInParent<ProximityVibrator>();
        if (pv != null) {
            AddProxVibInRange(pv);
        }
    }
    private void OnTriggerExit(Collider other) {
        Debug.Log("OnTriggerExit: " + other.gameObject);
        ProximityVibrator pv = other.gameObject.GetComponentInParent<ProximityVibrator>();
        if (pv != null) {
            RemoveProxVibInRange(pv);
        }
    }
}
