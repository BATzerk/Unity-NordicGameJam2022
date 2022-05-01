using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHUD : MonoBehaviour
{
    // Components
    [SerializeField] private GameObject go_gameOverPopup;
    [SerializeField] private GameObject go_paused;
    [SerializeField] private TextMeshProUGUI t_timeLeft;
    [SerializeField] private TextMeshProUGUI t_numGhoulsSlayedVal;
    // References
    [SerializeField] private GameController gameController;


    // ----------------------------------------------------------------
    // Awake
    // ----------------------------------------------------------------
    private void Awake() {
        go_gameOverPopup.SetActive(false);
        go_paused.SetActive(false);
    }
    public void OnSetGameState_Playing() {
        this.gameObject.SetActive(true);
        go_gameOverPopup.SetActive(false);
        UpdateTexts();
    }

    // ----------------------------------------------------------------
    // Update
    // ----------------------------------------------------------------
    private void Update() {
        // Update timer.
        t_timeLeft.text = TextUtils.ToTimeString_ms(gameController.TimeLeft);
    }

    // ----------------------------------------------------------------
    // Doers
    // ----------------------------------------------------------------
    public void UpdateTexts() {
        t_numGhoulsSlayedVal.text = gameController.NumGhoulsCaught.ToString();
        go_paused.SetActive(gameController.IsPaused);
    }

    // ----------------------------------------------------------------
    // Events
    // ----------------------------------------------------------------
    public void OnGameOver() {
        go_gameOverPopup.SetActive(true);
    }


}
