using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamFireVisuals : MonoBehaviour
{
    // Components
    [SerializeField] private Transform tf_circleParent;
    [SerializeField] private SpriteRenderer sr_circle;
    [SerializeField] private SpriteRenderer sr_centerDot;
    [SerializeField] private ParticleSystem ps_beamBurst1;
    [SerializeField] private ParticleSystem ps_beamBurst2;
    // References
    [SerializeField] private GameController gameController;

    /*
    // ----------------------------------------------------------------
    // Start
    // ----------------------------------------------------------------
    private void Start() {
        sr_circle.enabled = false;
        sr_centerDot.enabled = false;
    }
    
    // ----------------------------------------------------------------
    // Update
    // ----------------------------------------------------------------
    void Update() {
        if (gameController.IsChargingBeam) {
            sr_centerDot.enabled = Time.frameCount%6<3;
            float chargeLoc = Mathf.InverseLerp(0, GameController.BeamChargeupDuration, gameController.TimeChargingBeam);
            float spriteScale = Mathf.Lerp(1.2f, 0, chargeLoc);
            tf_circleParent.transform.localScale = Vector3.one * spriteScale;
            GameUtils.SetSpriteAlpha(sr_circle, chargeLoc);
        }
    }


    public void OnStartChargingBeam() {
        sr_circle.enabled = true;
        sr_centerDot.enabled = true;
    }
    public void OnFireBeam(bool didHitGhoul) {
        if (didHitGhoul) ps_beamBurst1.Emit(120);
        ps_beamBurst2.Emit(60);
        sr_circle.enabled = false;
        sr_centerDot.enabled = false;
    }
    public void OnGameOver() {
        sr_circle.enabled = false;
        sr_centerDot.enabled = false;
    }
    */

}
