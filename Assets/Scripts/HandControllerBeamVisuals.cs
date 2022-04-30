using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandControllerBeamVisuals : MonoBehaviour {
    // Components
    [SerializeField] LineRenderer lr_beamL;
    [SerializeField] LineRenderer lr_beamR;
    [SerializeField] GameObject go_reticleSprite;
    // Properties
    //private bool isPauseScreenOpen;


    // Getters
    private InputManager.CursorSource CurrCursorSource { get { return InputManager.Instance.CurrCursorSource; } }

    // ----------------------------------------------------------------
    //  Awake / Destroy
    // ----------------------------------------------------------------
    private void Start() {
        UpdateVisibilities();
    }
    private void Awake() {
        // Add event listeners!
        GameManagers.Instance.EventManager.ChangeCurrCursorSourceEvent += OnChangeCurrCursorSource;
        //GameManagers.Instance.EventManager.PauseScreenOpenedEvent += OnPauseScreenOpened;
        //GameManagers.Instance.EventManager.PauseScreenClosedEvent += OnPauseScreenClosed;
    }
    private void OnDestroy() {
        // Remove event listeners!
        GameManagers.Instance.EventManager.ChangeCurrCursorSourceEvent -= OnChangeCurrCursorSource;
        //GameManagers.Instance.EventManager.PauseScreenOpenedEvent -= OnPauseScreenOpened;
        //GameManagers.Instance.EventManager.PauseScreenClosedEvent -= OnPauseScreenClosed;
    }


    private void Update() {
        go_reticleSprite.transform.position = InputManager.Instance.ReticlePos;
        
        lr_beamL.SetPosition(0, InputManager.Instance.ReticlePos);
        lr_beamR.SetPosition(0, InputManager.Instance.ReticlePos);
        lr_beamL.SetPosition(1, HandsTransformKnower.HandL.position);
        lr_beamR.SetPosition(1, HandsTransformKnower.HandR.position);
    }


    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    private void UpdateVisibilities() {
        bool isABeam = true;// isPauseScreenOpen;
        go_reticleSprite.SetActive(isABeam);
        lr_beamL.enabled = isABeam && CurrCursorSource==InputManager.CursorSource.HandL;
        lr_beamR.enabled = isABeam && CurrCursorSource==InputManager.CursorSource.HandR;
    }


    // ----------------------------------------------------------------
    //  Events
    // ----------------------------------------------------------------
    private void OnChangeCurrCursorSource() {
        UpdateVisibilities();
    }
    //private void OnPauseScreenOpened() {
    //    isPauseScreenOpen = true;
    //    UpdateVisibilities();
    //}
    //private void OnPauseScreenClosed() {
    //    isPauseScreenOpen = false;
    //    UpdateVisibilities();
    //}




}
