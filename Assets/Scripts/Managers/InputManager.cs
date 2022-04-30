using UnityEngine;
using UnityEngine.XR;
using System;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {
	// ----------------------------------------------------------------
	//  Instance
	// ----------------------------------------------------------------
	public static InputManager Instance; // There can only be one.
	private void Awake() {
		// T... two??
		if (Instance != null) {
			// THERE CAN ONLY BE ONE.
			DestroyImmediate(this.gameObject);
			return;
		}
		// There's no instance already...
		else {
			if (Application.isEditor) { // In the UnityEditor?? Look for ALL InputManagers! We'll get duplicates when we reload our scripts.
				InputManager[] allOthers = FindObjectsOfType<InputManager>();
				for (int i = 0; i < allOthers.Length; i++) {
					if (allOthers[i] == this) { continue; } // Skip ourselves.
					Destroy(allOthers[i].gameObject);
				}
			}
		}

		// There could only be one. :)
		Instance = this;
		//DontDestroyOnLoad(this.gameObject);
	}


	// Enum
	public enum CursorSource { Undefined, Mouse, HandL, HandR }
	// Constants
	private const float DEAD_ZONE_MIN = 0.15f; // any raw axis input less than this value is set to 0. For controllers that "drift".
											   // Properties
	public float handTriggerIndexL, handTriggerIndexR; // right and left hand triggers
	float        phandTriggerIndexL, phandTriggerIndexR; // previous frame's right and left hand triggers
	public float handTriggerPointerL, handTriggerPointerR; // right and left hand triggers
	float        phandTriggerPointerL, phandTriggerPointerR; // previous frame's right and left hand triggers
	private bool isButtonDown_AdvanceLocksteps;
	private bool isButtonDown_PullObedients;
	public Collider CurrCursorCollider { get; private set; }
	public CursorSource CurrCursorSource { get; private set; } = CursorSource.HandR; // so we can swap seamlessly between controlling with mouse, left hand, and right hand.
	public Vector3 handLCursorPosGlobal { get; private set; } // where the VR hand controller's ray-hit is in global space.
	public Vector3 handRCursorPosGlobal { get; private set; } // where the VR hand controller's ray-hit is in global space.
	public Vector3 mouseCursorPosGlobal { get; private set; } // from the camera, where the mouse is in global space.
	public Vector3 ReticlePos { get; private set; } // In global space, from actual Raycasts.
	//private Vector3 phandCursorPosGlobal;
	//private Vector3 pmouseCursorPosGlobal;
	private Vector3 mousePosRelative;
	private Vector2 movementAxis; // I believe this is used by menus and the player.
	public Vector3 CursorPosGlobal { get; private set; } // The location of the mouse, OR hand-pointer, in global world space.
	public Vector3 CursorPosLocal { get; private set; } // Local to the current level.
	// VR
	InputDevice xrHandL;
	InputDevice xrHandR;
	[SerializeField] SpriteRenderer sr_cursorWorldPos;

	// Getters
	// NOTE: When used in game (for level editor), it's relative to the level. When used in MapEditor, it's relative to the whole world.
	public Vector3 MousePosScreen {
		get {
			//Debug.Log(CursorPosGlobal - Camera.main.gameObject.transform.position);
			//return CursorPosGlobal - Camera.main.gameObject.transform.position;
			return (Input.mousePosition - new Vector3(Screen.width,Screen.height,0)*0.5f);
		}
	}
	public Vector3 MousePosRelative { get { return mousePosRelative; } }
	public Vector2 MovementAxis { get { return movementAxis; } }

	private bool IsAnyButtonDownOnHandL() {
		return OVRInput.GetDown(OVRInput.Button.Any, OVRInput.Controller.LHand)
			|| handTriggerIndexL > 0.1f || handTriggerPointerL > 0.1f;
		//(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch)
		//	|| false;
	}
	private bool IsAnyButtonDownOnHandR() {
		return OVRInput.GetDown(OVRInput.Button.Any, OVRInput.Controller.RHand)
			|| handTriggerIndexR > 0.1f || handTriggerPointerR > 0.1f;
	}
	public bool GetButtonDown_Cancel () {
		if (Input.GetKeyDown(KeyCode.Escape)) return true;
		if (OVRInput.GetDown(OVRInput.Button.Back)) return true;
		return false;
	}
	public bool GetButtonDown_Submit () {
		if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) return true;
		if (OVRInput.GetDown(OVRInput.Button.One)) return true;
		return false;
	}
	public bool GetButtonDown_Pause () {
		if (Input.GetKeyDown(KeyCode.Escape)) return true;
		if (OVRInput.GetDown(OVRInput.Button.Start)) return true;
		return false;
    }
    public bool GetButtonUp_ResetLevel() {
        if (Input.GetKeyUp(KeyCode.R)) return true;
        if (OVRInput.GetUp(OVRInput.Button.Four)) return true;
        return false;
    }
    public bool GetButtonDown_ResetLevel() {
        if (Input.GetKeyDown(KeyCode.R)) return true;
        if (OVRInput.GetDown(OVRInput.Button.Four)) return true;
        return false;
    }
    public bool GetButtonDown_ResetOrientation() {
		if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.D)) return true;
		if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick)) return true; // left controller thumbstick click
		if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick)) return true; // right controller thumbstick click
		return false;
    }

	public bool GetButtonDown_FireHeadBullet() {
		// Controller Buttons
		if (OVRInput.GetDown(OVRInput.Button.One)
		 || OVRInput.GetDown(OVRInput.Button.Two)
		 || OVRInput.GetDown(OVRInput.Button.Three)
		 || OVRInput.GetDown(OVRInput.Button.Four))
			return true;
		// Controller Triggers
		if (phandTriggerIndexL<0.1f && handTriggerIndexL>=0.1f) return true;
		if (phandTriggerIndexR<0.1f && handTriggerIndexR>=0.1f) return true;
		if (phandTriggerPointerL<0.1f && handTriggerPointerL>=0.1f) return true;
		if (phandTriggerPointerR<0.1f && handTriggerPointerR>=0.1f) return true;
		// Keyboard
		return Input.GetKeyDown(KeyCode.Space);
	}
	//private bool GetButton_PullObedients ()	{
	//	// Hand Controllers
	//	if (handTriggerIndexL > 0.1f) return true;
	//	if (handTriggerIndexR > 0.1f) return true;
	//	xrHandL.TryGetFeatureValue(CommonUsages.trigger, out float triggerValueL);
	//	xrHandR.TryGetFeatureValue(CommonUsages.trigger, out float triggerValueR);
	//	if (triggerValueL>0.1f || triggerValueR>0.1f) { return true; }
	//	// Keyboard
	//	return Input.GetKey (KeyCode.Space);
	//}

	public int GetMouseButtonDown() {
		if (Input.GetMouseButtonDown(0)) return 0;
		if (Input.GetMouseButtonDown(1)) return 1;
		if (Input.GetMouseButtonDown(2)) return 2;
		if (phandTriggerIndexL<0.1f && handTriggerIndexL>=0.1f) return 0;
		if (phandTriggerIndexR<0.1f && handTriggerIndexR>=0.1f) return 0;
		if (phandTriggerPointerL<0.1f && handTriggerPointerL>=0.1f) return 1;
		if (phandTriggerPointerR<0.1f && handTriggerPointerR>=0.1f) return 1;
		return -1;
	}
	public int GetMouseButtonUp() {
		if (Input.GetMouseButtonUp(0)) return 0;
		if (Input.GetMouseButtonUp(1)) return 1;
		if (Input.GetMouseButtonUp(2)) return 2;
		if (phandTriggerIndexL>=0.1f && handTriggerIndexL<0.1f) return 0;
		if (phandTriggerIndexR>=0.1f && handTriggerIndexR<0.1f) return 0;
		if (phandTriggerPointerL>=0.1f && handTriggerPointerL<0.1f) return 1;
		if (phandTriggerPointerR>=0.1f && handTriggerPointerR<0.1f) return 1;
		return -1;
	}
	public int GetMouseButton() {
		if (Input.GetMouseButton(0)) return 0;
		if (Input.GetMouseButton(1)) return 1;
		if (Input.GetMouseButton(2)) return 2;
		if (handTriggerIndexL>0.1f) return 0;
		if (handTriggerIndexR>0.1f) return 0;
		if (handTriggerPointerL>0.1f) return 1;
		if (handTriggerPointerR>0.1f) return 1;
		return -1;
	}



	// ================================================================
	//  Start
	// ================================================================
	void Start() {
		// Set up XR controllers!
		List<InputDevice> leftHands = new List<InputDevice>();
		List<InputDevice> rightHands = new List<InputDevice>();
		InputDeviceCharacteristics leftHandChars = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
		InputDeviceCharacteristics rightHandChars = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
		InputDevices.GetDevicesWithCharacteristics(leftHandChars, leftHands);
		InputDevices.GetDevicesWithCharacteristics(rightHandChars, rightHands);
		if (leftHands.Count > 0) { xrHandL = leftHands[0]; }
		else { Debug.LogWarning("No XR left-hand controller found."); }
		if (rightHands.Count > 0) { xrHandR = rightHands[0]; }
		else { Debug.LogWarning("No XR right-hand controller found."); }

		List<InputDevice> allDevices = new List<InputDevice>();
        InputDevices.GetDevices(allDevices);
        foreach (var item in allDevices) {
            Debug.Log(item.name + "   " + item.characteristics);
        }
    }


	// ================================================================
	//  Update
	// ================================================================
	void Update() {
		UpdateButtons();
		UpdateMovementAxis();
		UpdateCursorPosGlobal();
	}

	private void UpdateButtons () {
		// Update OVR controller triggers
		phandTriggerIndexL = handTriggerIndexL;
		phandTriggerIndexR = handTriggerIndexR;
		phandTriggerPointerL = handTriggerPointerL;
		phandTriggerPointerR = handTriggerPointerR;
		handTriggerIndexL = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
		handTriggerIndexR = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
		handTriggerPointerL = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
		handTriggerPointerR = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);
	}
	private void UpdateMovementAxis () {
		movementAxis = Vector2.zero;
		movementAxis += new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		movementAxis += OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

		// Apply dead zone to the raw input.
		if (Mathf.Abs (movementAxis.x) < DEAD_ZONE_MIN) { movementAxis.x = 0; }
		if (Mathf.Abs (movementAxis.y) < DEAD_ZONE_MIN) { movementAxis.y = 0; }
		// Clamp them sh**s! (Aka don't let diagonal pushing let us go like twice as fast.)
		movementAxis = Vector2.ClampMagnitude (movementAxis, 1);
	}

	public void UpdateMousePosRelative (Vector3 cameraPosOffset, float cameraZoomAmount) {
		mousePosRelative = MousePosScreen;
		mousePosRelative /= cameraZoomAmount;
		mousePosRelative += cameraPosOffset;
	}
	public void UpdateCursorPosLocal(Transform tf_currLevel) {
		CursorPosLocal = tf_currLevel.InverseTransformPoint(CursorPosGlobal);
	}


	private void SetCurrCursorSource(CursorSource val) {
		CurrCursorSource = val;
		GameManagers.Instance.EventManager.OnChangeCurrCursorSource();
    }

	private void UpdateCursorPosGlobal() {
		// Update the known values.
		{
			handLCursorPosGlobal = GetCursorPosGlobal_HandL();
			handRCursorPosGlobal = GetCursorPosGlobal_HandR();
			mouseCursorPosGlobal = GetCursorPosGlobal_Mouse();
		}

		// Maybe CHANGE currCursorSource!
		{
			if (IsAnyButtonDownOnHandL()) SetCurrCursorSource(CursorSource.HandL);
			else if (IsAnyButtonDownOnHandR()) SetCurrCursorSource(CursorSource.HandR);
			else if (Input.GetMouseButtonDown(0)) SetCurrCursorSource(CursorSource.Mouse);
			//if (Vector3.Distance(handCursorPosGlobal, phandCursorPosGlobal) > 0.05f) doUseMouseAsCursor = false;
			//else if (Vector3.Distance(mouseCursorPosGlobal, pmouseCursorPosGlobal) > 0.2f) currCursorSource = CursorSource.Mouse;
		}

		// Update CursorPosGlobal.
		switch (CurrCursorSource) {
			case CursorSource.HandL:
				CursorPosGlobal = handLCursorPosGlobal;
				ReticlePos = GetRaycastHitPos(HandsTransformKnower.HandL);
				break;
			case CursorSource.HandR:
				CursorPosGlobal = handRCursorPosGlobal;
				ReticlePos = GetRaycastHitPos(HandsTransformKnower.HandR);
				break;
			default:
				CursorPosGlobal = mouseCursorPosGlobal;
				ReticlePos = mouseCursorPosGlobal;
				break;
		}

		// Update debug sprite pos.
		sr_cursorWorldPos.transform.position = ReticlePos;
	}
	private Vector3 GetCursorPosGlobal_Mouse() {
		// TODO: This.
		//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//Vector3 mouseCursorPos;
		//if (LineUtils.RayPlaneIntersection(ray, GameWorld.Position, new Vector3(0,0,-1), out mouseCursorPos)) {
		//	return mouseCursorPos;
		//}
		return Vector3.zero;
	}
	private Vector3 GetCursorPosGlobal_HandL() { return GetHandCursorPosGlobal(HandsTransformKnower.HandL); }
	private Vector3 GetCursorPosGlobal_HandR() { return GetHandCursorPosGlobal(HandsTransformKnower.HandR); }
	private Vector3 GetHandCursorPosGlobal(Transform handTF) {
		if (handTF == null) { Debug.LogError("Hey, InputManager is missing the Transform for a hand controller."); return Vector3.zero; } // Safety check.
		return GetRaycastHitPos(handTF);
		//Ray ray = new Ray(handTF.position, handTF.forward);
		//Vector3 handCursorPos;
		//if (LineUtils.RayPlaneIntersection(ray, GameWorld.Position, new Vector3(0,0,-1), out handCursorPos)) {
		//	return handCursorPos;
		//}
		//return Vector3.zero;
	}

	private Vector3 GetRaycastHitPos(Transform tf_origin) {
		RaycastHit hit;
		if (Physics.Raycast(tf_origin.position, tf_origin.forward, out hit)) {
			CurrCursorCollider = hit.collider;
			return hit.point;
        }
		return Vector3.zero;
    }





}




