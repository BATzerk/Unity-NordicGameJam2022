using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum VRMotionPermissionType : int {
	Undefined,
	NoMotion,
	Some,
	Full
}

public class DataManager {


	// ================================================================
	//  Initialize
	// ================================================================
	public DataManager() {
		Initialize ();
	}
	private void Initialize () {
	}

	/*
	public void ClearAllSaveData() {
		// What data do we wanna retain??
		int controllerType = InputManager.Instance.ControllerType;

		// KNUKE IT
		SaveStorage.DeleteAll ();
		Reset ();
		Debug.Log ("All SaveStorage CLEARED!");

		// Pump back the data we retained!
		InputManager.Instance.SetControllerType (controllerType);
	}
	*/

}




















