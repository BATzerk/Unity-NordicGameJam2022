using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public enum GameState { Undefined, Setup0, Setup1, Playing, GameOver }
    // References
    [SerializeField] private BeamFireVisuals beamFireVisuals;
    [SerializeField] private GameHUD gameHUD;
    [SerializeField] private GameObject go_gameOverLight;
    [SerializeField] private Transform tf_head; // headset.
    [SerializeField] private Mitt mittL;
    //[SerializeField] private Mitt mittR;
    [SerializeField] private Gun gunR;
    [SerializeField] private PlayerNarration playerNarration;
    private List<Ghoul> ghouls = new List<Ghoul>();
    // Properties
    [SerializeField] public float TimePerRound = 91;
    public const float BeamChargeupDuration = 2f;
    public GameState gameState { get; private set; } = GameState.Setup0;
    public int NumBeamsFired { get; private set; } = 0;
    public int NumGhoulsCaught { get; private set; } = 0;
    public bool IsChargingBeam { get; private set; }
    public float TimeChargingBeam { get; private set; } // how long we've held down a beam charge button.
    public float TimeLeft { get; private set; }
    public bool IsPaused { get; private set; } = false;
    public bool IsGameOver { get { return gameState == GameState.GameOver; } }

    // Getters
    private bool MayStartChargingBeam() {
        return !IsChargingBeam;// && (mittL.IsTouchingVibCore);// || mittR.IsTouchingVibCore);
    }


    // ----------------------------------------------------------------
    // Start
    // ----------------------------------------------------------------
    void Start() {
        //SetGameState_Setup0();
        SetGameState_Playing();

        // Add event listeners!
        OVRManager.HMDMounted += OnGainFocus;
        OVRManager.HMDUnmounted += OnLoseFocus;
        OVRManager.InputFocusLost += OnLoseFocus;
        OVRManager.InputFocusAcquired += OnGainFocus;
    }
    private void OnDestroy() {
        // Remove event listeners!
        OVRManager.HMDMounted -= OnGainFocus;
        OVRManager.HMDUnmounted -= OnLoseFocus;
        OVRManager.InputFocusLost -= OnLoseFocus;
        OVRManager.InputFocusAcquired -= OnGainFocus;
    }
    private bool applicationHasFocus = true;
    void OnGainFocus() { OnApplicationFocus(true); }
    void OnLoseFocus() { OnApplicationFocus(false); }
    private void OnApplicationFocus(bool _hasFocus) {
#if !UNITY_EDITOR
        applicationHasFocus = _hasFocus;
        UpdateTimeScale();
#endif
    }
    private void OnApplicationPause(bool _pauseStatus) {
        applicationHasFocus = !_pauseStatus; // we DO have focus if it's NOT paused. For mobile. TODO: Test this out.
        UpdateTimeScale();
    }


    // ----------------------------------------------------------------
    // Update
    // ----------------------------------------------------------------
    void Update() {
        if (gameState == GameState.Playing && InputManager.Instance.GetButtonDown_Pause()) {
            TogglePause();
        }

        if (IsPaused) return; // No updates if paused.

        switch (gameState) {
            case GameState.Setup0: Update_Setup0(); break;
            case GameState.Setup1: Update_Setup1(); break;
            case GameState.Playing: Update_Playing(); break;
            case GameState.GameOver: Update_GameOver(); break;
        }
    }

    private void Update_Setup0() {
        if (InputManager.Instance.handTriggerIndexL>0.5f && InputManager.Instance.handTriggerIndexR>0.5f) {
            SetGameState_Setup1();
        }
    }
    private void Update_Setup1() {

    }
    private void Update_Playing() {
        RegisterButtonInput();

        if (IsChargingBeam) {
            TimeChargingBeam += Time.deltaTime;
            if (TimeChargingBeam >= BeamChargeupDuration) {
                FireBeam(gunR.transform);
            }
        }

        // Countdown!
        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= 0) {
            OnTimeOver();
        }
    }
    private void Update_GameOver() {

    }


    private void SetGameState_Setup0() {
        go_gameOverLight.SetActive(false);
        gameHUD.gameObject.SetActive(false);
        //ghouls = new List<Ghoul>(FindObjectsOfType<Ghoul>()); // hacky, just find all the ghouls that are already in the scene just in case.
    }
    private void SetGameState_Setup1() {
        gameState = GameState.Playing;
        playerNarration.OnSetGameState_Setup1();
    }
    private void SetGameState_Playing() {
        gameState = GameState.Playing;
        TimeLeft = TimePerRound;
        playerNarration.OnSetGameState_Playing();
        gameHUD.OnSetGameState_Playing();
        MaybeSpawnAGhoul(); // this will definitely spawn a ghoul.
    }
    private void SetGameState_GameOver() {
        gameState = GameState.GameOver;
    }





    private void RegisterButtonInput() {
        // Start charging beam!
        if (MayStartChargingBeam()) {
            if (InputManager.Instance.GetTriggerDown_PointerR()) {
                StartChargingBeam();
            }
        }
    }

    private void StartChargingBeam() {
        // HACK! Note: poorly worded functions. #gamejam
        beamFireVisuals.OnStartChargingBeam();
        IsChargingBeam = true;
        TimeChargingBeam = 0;
    }


    // ----------------------------------------------------------------
    // Doers
    // ----------------------------------------------------------------
    private void TogglePause() {
        IsPaused = !IsPaused;
        UpdateTimeScale();
        gameHUD.UpdateTexts();
    }
    private void UpdateTimeScale() {
        if (!applicationHasFocus || IsPaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    RaycastHit[] hits;
    private void FireBeam(Transform tf_fireSource) {
        // Increment NumBeamsFired.
        NumBeamsFired++;
        IsChargingBeam = false;
        // Raycast!
        Ghoul ghoulToSlay = null;
        hits = Physics.RaycastAll(tf_fireSource.position, tf_fireSource.forward);
        foreach (RaycastHit hit in hits) {
            GhoulCore ghoulBody = hit.collider.GetComponent<GhoulCore>();
            if (ghoulBody != null) {
                ghoulToSlay = hit.collider.GetComponentInParent<Ghoul>();
                break;
            }
        }
        // Slay or Miss!
        if (ghoulToSlay != null) {
            SlayGhoul(ghoulToSlay);
            Debug.Log("SLAYED ghoul!");
        }
        else {
            Debug.Log("Misssed ghoul!");
        }

        beamFireVisuals.OnFireBeam(ghoulToSlay!=null);
    }
    private void MaybeSpawnAGhoul() {
        Ghoul newObj = Instantiate(ResourcesHandler.Instance.Ghoul).GetComponent<Ghoul>();
        ghouls.Add(newObj);
        // Maybe make another one in a minute if there aren't enough!
        if (ghouls.Count < 2) {
            Invoke("MaybeSpawnAGhoul", 2f);
        }
    }


    // ----------------------------------------------------------------
    // Events
    // ----------------------------------------------------------------
    public void SlayGhoul(Ghoul ghoul) {
        // Slay ghoul!
        ghoul.SlayMe();
        NumGhoulsCaught ++;
        gameHUD.UpdateTexts();
        // Make a new one, baby.
        Invoke("MaybeSpawnAGhoul", 1f);
    }
    private void OnTimeOver() {
        TimeLeft = 0;
        SetGameState_GameOver();
        gameHUD.OnGameOver();
        beamFireVisuals.OnGameOver();
        go_gameOverLight.SetActive(true);
    }


}






//void FireHeadBullet() {
//    Bullet newObj = Instantiate(ResourcesHandler.Instance.Bullet).GetComponent<Bullet>();
//    newObj.Initialize(this, tf_head);
//}