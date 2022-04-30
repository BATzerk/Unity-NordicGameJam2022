using UnityEngine;
using System.Collections;

/// This class is MOOT in VR!
public class ScreenHandler : MonoBehaviour {
	/*
	// Constants
	static private Vector2 originalScreenSize { get {
			return IsLandscape() ? new Vector2(800,600) : new Vector2(1080,1920); // Instead of asking if we're on mobile, ask if we're landscape/portrait, which is more flexible.
	} }
	// Statics!
	static private float screenScale;
	static private Vector2 relativeScreenSize; // to-do:? Set this based on the status of the Canvas
	// Properties
	private Vector2 knownScreenSize; // when this changes, we'll dispatch an event.

	// Getters
	static public float ScreenScale { get { return screenScale; } }
	static public bool IsLandscape () { return Screen.width>Screen.height; }
	static public bool IsPortrait () { return !IsLandscape(); }
	static public Vector2 OriginalScreenSize { get { return originalScreenSize; } }
	static public Vector2 RelativeScreenSize { get { return relativeScreenSize; } }


	// ----------------------------------------------------------------
	//  Awake
	// ----------------------------------------------------------------
	private void Awake () {
		// Set application values
		Application.targetFrameRate = GameVisualProperties.TargetFrameRate;

		// Update the screen size right away! If you know what I mean ;)
		OnScreenSizeChanged ();
	}


	// ----------------------------------------------------------------
	//  Update
	// ----------------------------------------------------------------
	private void Update () {
		if (Screen.width!=knownScreenSize.x || Screen.height!=knownScreenSize.y) {
			OnScreenSizeChanged ();
		}
	}

	private void OnScreenSizeChanged () {
		knownScreenSize = new Vector2 (Screen.width, Screen.height);
		// Update screenScale and stuff!
		screenScale = Mathf.Min (((float)Screen.width)/originalScreenSize.x, ((float)Screen.height)/originalScreenSize.y);
		relativeScreenSize = new Vector2 (Screen.width/screenScale+1f, Screen.height/screenScale+1f); // NOTE: Add bloat!! I added this when I discovered mobile resolution was returning 638.5 instead of 640 width (for 640x1136). The simple, cheap fix is to bloat the screen size a tad.
		// Dispatch!
		GameManagers.Instance.EventManager.OnScreenSizeChanged ();
	}


	*/
}





