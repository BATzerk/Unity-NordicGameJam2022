using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mitt : MonoBehaviour
{
    // Components
    [SerializeField] private ParticleSystem ps_beamBurst1;
    [SerializeField] private ParticleSystem ps_beamBurst2;
    [SerializeField] private MeshRenderer mr_bulb; // blinks!
    // Properties
    [SerializeField] private OVRInput.Controller MyHandType;
    public bool IsTouchingCore { get; private set; } // TRUE if we're RIGHT inside the vibrator! If this is true, we'll go BUMP BUMP instead of vrrrrr.
    // References
    [SerializeField] private GameController gameController;
    [SerializeField] private AudioSource as_tone; // looping tone.
    private List<ProximityVibrator> proxVibsInRange = new List<ProximityVibrator>();



    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    private void Update() {
        // Game over? Do nothin'.
        if (gameController.IsGameOver || Time.timeScale < 0.1f) return;

        // How intense should we vibrate?
        float locToCore = 0; // from 0 to 1. 0 is far, 1 is in the core.
        IsTouchingCore = false; // I'll say otherwise shortly.
        for (int i=proxVibsInRange.Count-1; i>=0; --i) {
            ProximityVibrator pv = proxVibsInRange[i];
            if (pv == null) { proxVibsInRange.RemoveAt(i); continue; } // Safety check.
            float dist = Vector3.Distance(this.transform.position, pv.transform.position);
            IsTouchingCore |= dist < pv.DistMin;
            float thisLoc = Mathf.InverseLerp(pv.DistMax, pv.DistMin, dist);
            thisLoc = Mathf.Pow(thisLoc, 3); // cube it! For a much steeper dropoff.
            locToCore += thisLoc;
        }
        locToCore = Mathf.Clamp01(locToCore); // just clamp it to 1. A vibration over 1 would explode the controller to smithereens.


        float vibVol = Mathf.InverseLerp(0.3f, 1, locToCore); // start vibrating only when we're closer than when the sound starts.
        float toneVol = Mathf.Min(locToCore, 0.85f); // don't get like, too loud.
        float tonePitch = Mathf.Lerp(0.05f, 1.3f, locToCore);


        // Hey, if we're inside a vib's core, we do a heartbeat style instead.
        if (IsTouchingCore) {
            tonePitch = 2f;
            // Rhythmically disable vibration.
            if (Mathf.Sin(Time.unscaledTime*70f) > 0) {
                vibVol = 0;
                toneVol = 0.2f;
            }
        }

        // Apply vibration!
        if (vibVol > 0) {
            GameManagers.Instance.VibrationManager.Vibrate(0.01f, vibVol, MyHandType);
        }
        // Apply sound!
        as_tone.pitch = tonePitch;
        as_tone.volume = toneVol;
        // Apply bulb color!
        Color bulbEmissionColor = new ColorHSB(0.2f, 255, vibVol).ToColor();
        mr_bulb.material.SetColor("_EmissionColor", bulbEmissionColor);
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


    public void OnFireGun(bool didSlayGhoul) {
        if (didSlayGhoul) ps_beamBurst1.Emit(120);
        ps_beamBurst2.Emit(60);
    }


    // ----------------------------------------------------------------
    //  Collision Events
    // ----------------------------------------------------------------
    private void OnTriggerEnter(Collider other) {
        GhoulCore gb = other.gameObject.GetComponent<GhoulCore>();
        ProximityVibrator pv = other.gameObject.GetComponentInParent<ProximityVibrator>();
        GhoulProximityTrigger gpt = other.gameObject.GetComponent<GhoulProximityTrigger>();
        // Ghoul proximity trigger??
        if (gpt != null) AddProxVibInRange(pv);
        // Ghoul body??
        if (gb != null) gb.OnMittEnterMe();
    }
    private void OnTriggerExit(Collider other) {
        GhoulCore gb = other.gameObject.GetComponent<GhoulCore>();
        ProximityVibrator pv = other.gameObject.GetComponentInParent<ProximityVibrator>();
        GhoulProximityTrigger gpt = other.gameObject.GetComponent<GhoulProximityTrigger>();
        // Ghoul proximity trigger??
        if (gpt != null) RemoveProxVibInRange(pv);
        // Ghoul body??
        if (gb != null) gb.OnMittExitMe();
    }
}
