using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelResetter : MonoBehaviour
{
    // Properties
    private float TimeToHoldResetButton = 1.8f;
    private float timeHeldResetButton;
    private bool isResetButtonHeld;

    void Update() {
        // Button inputs!
        if (InputManager.Instance.GetButtonDown_ResetLevel()) {
            isResetButtonHeld = true;
            timeHeldResetButton = 0;
        }
        else if (InputManager.Instance.GetButtonUp_ResetLevel()) {
            isResetButtonHeld = false;
        }

        // Maybe reset!
        if (isResetButtonHeld) {
            timeHeldResetButton += Time.deltaTime;
            if (timeHeldResetButton > TimeToHoldResetButton) {
                SceneHelper.ReloadScene();
            }
        }
    }
}
