using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mitt : MonoBehaviour
{
    // Properties
    [SerializeField] private OVRInput.Controller MyHandType;
    public bool IsTouchingVibCore { get; private set; } // TRUE if we're RIGHT inside the vibrator! If this is true, we'll go BUMP BUMP instead of vrrrrr.
    // References
    [SerializeField] private GameController gameController;
    private List<ProximityVibrator> proxVibsInRange = new List<ProximityVibrator>();



    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    private void Update() {
        // Game over? Do nothin'.
        if (gameController.IsGameOver || Time.timeScale < 0.1f) return;

        // How intense should we vibrate?
        float vibVol = 0; // from 0 to 1.
        IsTouchingVibCore = false; // I'll say otherwise shortly.
        for (int i=proxVibsInRange.Count-1; i>=0; --i) {
            ProximityVibrator pv = proxVibsInRange[i];
            if (pv == null) { proxVibsInRange.RemoveAt(i); continue; } // Safety check.
            float dist = Vector3.Distance(this.transform.position, pv.transform.position);
            IsTouchingVibCore |= dist < pv.DistMin;
            float thisVol = Mathf.InverseLerp(pv.DistMax, pv.DistMin, dist);
            thisVol = Mathf.Pow(thisVol, 3); // cube it! For a much steeper dropoff.
            vibVol += thisVol;
        }
        vibVol = Mathf.Clamp01(vibVol); // just clamp it to 1. A vibration over 1 would explode the controller to smithereens.

        // Hey, if we're inside a vib's core, we do a heartbeat style instead.
        if (IsTouchingVibCore) {
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
        ProximityVibrator pv = other.gameObject.GetComponentInParent<ProximityVibrator>();
        if (pv != null) {
            AddProxVibInRange(pv);
        }
        // Ghoul body??
        GhoulCore gb = other.gameObject.GetComponent<GhoulCore>();
        if (gb != null) gb.OnMittEnterMe();
    }
    private void OnTriggerExit(Collider other) {
        ProximityVibrator pv = other.gameObject.GetComponentInParent<ProximityVibrator>();
        if (pv != null) {
            RemoveProxVibInRange(pv);
        }
        // Ghoul body??
        GhoulCore gb = other.gameObject.GetComponent<GhoulCore>();
        if (gb != null) gb.OnMittExitMe();
    }
}
