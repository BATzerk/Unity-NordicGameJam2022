using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    // References
    [SerializeField] private BeamFireVisuals beamFireVisuals;
    [SerializeField] private GameHUD gameHUD;
    [SerializeField] private GameObject go_gameOverLight;
    [SerializeField] private Transform tf_head; // headset.
    [SerializeField] private Mitt mittL;
    [SerializeField] private Mitt mittR;
    private List<Ghoul> ghouls;
    // Properties
    [SerializeField] public float TimePerRound = 91;
    public const float BeamChargeupDuration = 2f;
    public int NumBeamsFired { get; private set; } = 0;
    public int NumGhoulsCaught { get; private set; } = 0;
    public bool IsChargingBeam { get; private set; }
    public float TimeChargingBeam { get; private set; } // how long we've held down a beam charge button.
    public float TimeLeft { get; private set; }
    public bool IsPaused { get; private set; } = false;
    public bool IsGameOver { get; private set; } = false;

    // Getters
    private bool MayStartChargingBeam() {
        return !IsChargingBeam && (mittL.IsTouchingVibCore || mittR.IsTouchingVibCore);
    }


    // ----------------------------------------------------------------
    // Start
    // ----------------------------------------------------------------
    void Start() {
        TimeLeft = TimePerRound;
        gameHUD.UpdateTexts();
        go_gameOverLight.SetActive(false);
        ghouls = new List<Ghoul>(FindObjectsOfType<Ghoul>()); // hacky, just find all the ghouls that are already in the scene just in case.

        MaybeSpawnAGhoul();

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
        applicationHasFocus = _hasFocus;
        UpdateTimeScale();
    }
    private void OnApplicationPause(bool _pauseStatus) {
        applicationHasFocus = !_pauseStatus; // we DO have focus if it's NOT paused. For mobile. TODO: Test this out.
        UpdateTimeScale();
    }


    // ----------------------------------------------------------------
    // Update
    // ----------------------------------------------------------------
    void Update() {
        if (InputManager.Instance.GetButtonDown_Pause()) {
            TogglePause();
        }
        if (IsPaused || IsGameOver) return; // No updates if paused, OR game over.

        // Start charging beam!
        if (MayStartChargingBeam() && InputManager.Instance.GetButtonDown_FireHeadBullet()) {
            StartChargingBeam();
        }
        if (IsChargingBeam) {
            TimeChargingBeam += Time.deltaTime;
            if (TimeChargingBeam >= BeamChargeupDuration) {
                FireBeam();
            }
        }

        // Countdown!
        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= 0) {
            OnTimeOver();
        }
    }

    private void StartChargingBeam() {
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
    private void FireBeam() {
        // Increment NumBeamsFired.
        NumBeamsFired++;
        IsChargingBeam = false;
        // Raycast!
        Ghoul ghoulToSlay = null;
        hits = Physics.RaycastAll(tf_head.position, tf_head.forward);
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
        IsGameOver = true;
        gameHUD.OnGameOver();
        beamFireVisuals.OnGameOver();
        go_gameOverLight.SetActive(true);
    }


}






//void FireHeadBullet() {
//    Bullet newObj = Instantiate(ResourcesHandler.Instance.Bullet).GetComponent<Bullet>();
//    newObj.Initialize(this, tf_head);
//}