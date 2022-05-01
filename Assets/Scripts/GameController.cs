using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common.Utils;
using UnityEngine;

public class GameController : MonoBehaviour {
    public enum GameState { Undefined, Setup0, Setup1, PrePlayCountdown, Playing, GameOver }
    // References
    [SerializeField] private GameHUD gameHUD;
    [SerializeField] private GameObject go_gameOverLight;
    [SerializeField] private Transform tf_head; // headset.
    [SerializeField] private Mitt mittL;
    [SerializeField] private Mitt mittR;
    [SerializeField] private PlayerNarration playerNarration;
    [SerializeField] private AudioClip[] fireBeamSounds;
    [SerializeField] private AudioClip[] grabSounds;
    [SerializeField] private AudioClip[] slaySounds;
    [SerializeField] private AudioClip[] escapeSounds;
    [SerializeField] private AudioClip clip_correctChime;
    [SerializeField] private AudioClip clip_prePlayCountdown;
    [SerializeField] private AudioClip gameplayCountdown;
    [SerializeField] private float ghoulSpeedUpTime;
    private List<Ghoul> ghouls = new List<Ghoul>();

    // Properties
    [SerializeField] public float TimePerRound = 91;
    public GameState gameState { get; private set; } = GameState.Setup0;
    public int NumGhoulsSlainL { get; private set; } = 0;
    public int NumGhoulsSlainR { get; private set; } = 0;
    public float TimeLeft { get; private set; }
    public bool IsPaused { get; private set; } = false;
    public bool IsGameOver { get { return gameState == GameState.GameOver; } }



    // ----------------------------------------------------------------
    // Start
    // ----------------------------------------------------------------
    void Start() {
        //SetGameState_Setup0();
        SetGameState(GameState.Setup0);

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

    private void MaybeSpawnAGhoul() {
        if (gameState == GameState.Playing && ghouls.Count < 2) {
            var newObj = Instantiate(ResourcesHandler.Instance.Ghoul).GetComponent<Ghoul>();
            newObj.Initialize(this);
            ghouls.Add(newObj);
        }
        // Try again in a few seconds.
        Invoke("MaybeSpawnAGhoul", 2f);
    }


    // ----------------------------------------------------------------
    // Update
    // ----------------------------------------------------------------
    void Update() {
        if (gameState == GameState.Playing && InputManager.Instance.GetButtonDown_Pause()) {
            TogglePause();
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            SceneHelper.ReloadScene();
            return;
        }

        if (IsPaused) return; // No updates if paused.

        switch (gameState) {
            case GameState.Setup0: Update_Setup0(); break;
            //case GameState.Setup1: Update_Setup1(); break;
            case GameState.Playing: Update_Playing(); break;
            case GameState.GameOver: Update_GameOver(); break;
        }
    }

    private void Update_Setup0() {
        if ((InputManager.Instance.handTriggerIndexL>0.5f && InputManager.Instance.handTriggerIndexR>0.5f)
            || Input.GetKeyDown(KeyCode.Space)
        ) {
            SetGameState(GameState.PrePlayCountdown);
        }
    }
    //private void Update_Setup1() {

    //}
    private void Update_Playing() {
        RegisterButtonInput();

        // Countdown!
        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= 0) {
            OnTimeOver();
        }
    }
    private void Update_GameOver() {
        if (InputManager.Instance.handTriggerIndexL>0.5f && InputManager.Instance.handTriggerIndexR>0.5f) {
            SceneHelper.ReloadScene();
        }
    }


    private void SetGameState(GameState state) {
        gameState = state;
        switch (gameState) {
            case GameState.Setup0:
                go_gameOverLight.SetActive(false);
                break;
            case GameState.Setup1:
                playerNarration.OnSetGameState_Setup1();
                break;
            case GameState.PrePlayCountdown:
                SoundController.Instance.Play(clip_prePlayCountdown);
                Invoke("OnPrePlayCountdownDone", 3);
                break;
            case GameState.Playing:
                TimeLeft = TimePerRound;
                playerNarration.OnSetGameState_Playing();
                MaybeSpawnAGhoul(); // this will definitely spawn a ghoul.
                break;
            case GameState.GameOver:
                gameState = GameState.GameOver;
                break;
        }
        // Tell the HUD!
        gameHUD.OnSetGameState(gameState);
    }
    private void OnPrePlayCountdownDone() {
        SetGameState(GameState.Playing);
    }





    private void RegisterButtonInput() {
        if (mittL.IsTouchingCore && InputManager.Instance.GetButtonDown_FireGunL()) {
            FireGunL();
        }
        if (mittR.IsTouchingCore && InputManager.Instance.GetButtonDown_FireGunR()) {
            FireGunR();
        }
    }

    private const float GhoulCoreSize = 0.1f; // NOTE: Not applied anywhere else! Independent from ProximityVibrator.
    private Ghoul GetGhoulAtPos(Vector3 pos) {
        foreach (Ghoul ghoul in ghouls) {
            if (Vector3.Distance(pos, ghoul.transform.position) < GhoulCoreSize) return ghoul;
        }
        return null;
    }

    private void FireGunL() {
        Ghoul ghoulToSlay = GetGhoulAtPos(mittL.transform.position);
        mittL.OnFireGun(ghoulToSlay == null);
        // Slay or Miss!
        if (ghoulToSlay != null) {
            SlayGhoulL(ghoulToSlay);
        }
        else {
            OnMissGhoul();
        }
    }
    private void FireGunR() {
        Ghoul ghoulToSlay = GetGhoulAtPos(mittR.transform.position);
        mittR.OnFireGun(ghoulToSlay == null);
        // Slay or Miss!
        if (ghoulToSlay != null) {
            SlayGhoulR(ghoulToSlay);
        }
        else {
            OnMissGhoul();
        }
    }

    private void OnMissGhoul() {
        foreach (var ghoul in ghouls) {
            ghoul.SpeedUpTimeLeft = ghoulSpeedUpTime;
        }
        var aGhoul = ghouls.RandomItem();
        SoundController.Instance.PlayRandomAt(escapeSounds, aGhoul.transform.position);
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


    // ----------------------------------------------------------------
    // Events
    // ----------------------------------------------------------------
    private void SlayGhoulL(Ghoul ghoul) {
        NumGhoulsSlainL++;
        OnEitherPlayerSlayGhoul(ghoul);
    }
    private void SlayGhoulR(Ghoul ghoul) {
        NumGhoulsSlainR++;
        OnEitherPlayerSlayGhoul(ghoul);
    }
    private void OnEitherPlayerSlayGhoul(Ghoul ghoul) {
        SoundController.Instance.PlayRandomAt(slaySounds, ghoul.transform.position);
        SoundController.Instance.PlayAt(clip_correctChime, ghoul.transform.position);
        ghoul.SlayMe();
        gameHUD.UpdateTexts();
    }


    private void OnTimeOver() {
        TimeLeft = 0;
        SetGameState(GameState.GameOver);
        go_gameOverLight.SetActive(true);
    }
    public void OnGhoulDestroyed(Ghoul ghoul) {
        ghouls.Remove(ghoul);
    }


}





//private void StartChargingBeam() {
//    // HACK! Note: poorly worded functions. #gamejam
//    beamFireVisuals.OnStartChargingBeam();
//    SoundController.Instance.PlayRandom(fireBeamSounds);
//    IsChargingBeam = true;
//    TimeChargingBeam = 0;
//}

//void FireHeadBullet() {
//    Bullet newObj = Instantiate(ResourcesHandler.Instance.Bullet).GetComponent<Bullet>();
//    newObj.Initialize(this, tf_head);
//}

/*
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
    // Play sound
    else if (ghouls.Any())
    {
        foreach (var ghoul in ghouls)
        {
            ghoul.SpeedUpTimeLeft = ghoulSpeedUpTime;
        }
        var aGhoul = ghouls.RandomItem();
        SoundController.Instance.PlayRandomAt(escapeSounds, aGhoul.transform.position);
        Debug.Log("Misssed ghoul!");
    }

    //TODO speed up ghosts on miss

    beamFireVisuals.OnFireBeam(ghoulToSlay!=null);
}
*/