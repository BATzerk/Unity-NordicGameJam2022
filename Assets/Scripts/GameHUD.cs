using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHUD : MonoBehaviour
{
    // Components
    [SerializeField] private GameOverPopup gameOverPopup;
    [SerializeField] private GameObject go_paused;
    [SerializeField] private GameObject go_startPlayingInstructions;
    [SerializeField] private GameObject go_timeLeft;
    [SerializeField] private TextMeshProUGUI t_timeLeft;
    [SerializeField] private TextMeshProUGUI t_numGhoulsSlainLVal;
    [SerializeField] private TextMeshProUGUI t_numGhoulsSlainRVal;
    // References
    [SerializeField] private GameController gameController;


    // ----------------------------------------------------------------
    // Awake
    // ----------------------------------------------------------------
    private void Awake() {
        go_paused.SetActive(false);
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
        t_numGhoulsSlainLVal.text = gameController.NumGhoulsSlainL.ToString();
        t_numGhoulsSlainRVal.text = gameController.NumGhoulsSlainR.ToString();
        go_paused.SetActive(gameController.IsPaused);
    }


    // ----------------------------------------------------------------
    // Events
    // ----------------------------------------------------------------
    public void OnSetGameState(GameController.GameState state) {
        // Hide things by default.
        go_timeLeft.SetActive(false);
        go_startPlayingInstructions.SetActive(false);
        gameOverPopup.Hide();

        // Show what makes sense.
        switch (state) {
            case GameController.GameState.Setup0:
                go_startPlayingInstructions.SetActive(true);
                break;
            case GameController.GameState.Playing:
                UpdateTexts();
                go_timeLeft.SetActive(true);
                break;
            case GameController.GameState.GameOver:
                gameOverPopup.Show();
                break;
        }
    }


}
