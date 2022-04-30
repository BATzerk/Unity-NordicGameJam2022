using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class VibrationManager {
	// Constants
	public const float LIGHT_TAP = 0.05f;
	public const float MEDIUM_TAP = 0.09f;
	public const float HARD = 0.13f;
	// Properties
	private bool isVibrationEnabled = true;

	// Getters
	public bool IsVibrationEnabled { get { return isVibrationEnabled; } }


	// ================================================================
	//	Initialize
	// ================================================================
	public VibrationManager () {
		// Load my biscuits!
		isVibrationEnabled = SaveStorage.GetBool(SaveKeys.IsVibrationEnabled, true);
	}
	
	
	// ================================================================
	//	Doers
	// ================================================================
	public void ToggleVibration () {
		// Toggle the value
		isVibrationEnabled = !isVibrationEnabled;
		// Save it up!
		SaveStorage.SetBool (SaveKeys.IsVibrationEnabled, isVibrationEnabled);
	}
	public void ToggleVibrationWithImmediateFeedback () {
		// Do the toggle shuffle.
		ToggleVibration ();
		// If enabled, let the player know their controller totally vibrates!
		if (isVibrationEnabled) {
			Vibrate (MEDIUM_TAP);
		}
	}

	
	
	// ================================================================
	//	Vibration!
	// ================================================================
	public void Vibrate (float duration, float amplitude=0.5f, OVRInput.Controller handType=OVRInput.Controller.RHand) {
		if (!isVibrationEnabled) { // No vibration enabled? Get outta here, Chez.
			return;
		}
		ActionBasedController controllerToVibrate = handType == OVRInput.Controller.RHand ? HandsTransformKnower.ControllerHandR : HandsTransformKnower.ControllerHandL;
		controllerToVibrate.SendHapticImpulse(amplitude, duration);
	}


}

		//OVRInput.SetControllerVibration(0.1f, 0.4f, OVRInput.Controller.RHand);



