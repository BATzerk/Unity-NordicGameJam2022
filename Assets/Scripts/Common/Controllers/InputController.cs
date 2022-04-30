using UnityEngine;
using System.Collections;
/*
public class InputController : MonoBehaviour {
	// Constants
	private const float QUICK_CLICK_TIME_WINDOW = 0.3f; // quick-click is used for double-clicks (but could be extended to 3 or more).
    // Instance
    static public InputController Instance { get; private set; }
    // Properties
    private int numQuickClicks=0; // for double-clicks (or maybe even more).
    private bool isDoubleClick; // reset every frame.
    private float timeWhenNullifyDoubleClick;

    // Getters
    public bool IsDoubleClick { get { return isDoubleClick; } }
    public Vector2 MousePosWorld { get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); } }

    static public bool IsTouchHold() { return Input.GetMouseButton(0); } // TODO: Remove this
    static public bool IsTouchDown() { return Input.GetMouseButtonDown(0); }
    static public bool IsTouchUp() { return Input.GetMouseButtonUp(0); }
    static public int GetMouseButtonDown() {
		if (Input.GetMouseButtonDown(0)) return 0;
		if (Input.GetMouseButtonDown(1)) return 1;
		if (Input.GetMouseButtonDown(2)) return 2;
		return -1;
	}
	static public int GetMouseButtonUp() {
		if (Input.GetMouseButtonUp(0)) return 0;
		if (Input.GetMouseButtonUp(1)) return 1;
		if (Input.GetMouseButtonUp(2)) return 2;
		return -1;
	}
	static public bool IsMouseButtonDown () {
		return GetMouseButtonDown() != -1;
	}
	static public bool IsMouseButtonUp () {
		return GetMouseButtonUp() != -1;
	}

    static public bool IsKey_alt { get { return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt); } }
    static public bool IsKey_control { get { return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl); } }
    static public bool IsKeyUp_control { get { return Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl); } }
    static public bool IsKeyDown_control { get { return Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl); } }
    static public bool IsKey_shift { get { return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift); } }
    static public bool IsKeyUp_shift { get { return Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift); } }
    static public bool IsKeyDown_shift { get { return Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift); } }


    // ----------------------------------------------------------------
    //  Awake
    // ----------------------------------------------------------------
    private void Awake () {
		// There can only be one (instance)!!
		if (Instance != null) {
			Destroy (this.gameObject);
			return;
		}
		Instance = this;
	}

	// ----------------------------------------------------------------
	//  Update
	// ----------------------------------------------------------------
	private void Update () {
        RegisterMouse();
	}

    private void RegisterMouse() {
        isDoubleClick = false; // I'll say otherwise in a moment.

        if (Input.GetMouseButtonDown(0)) {
            // Up how many clicks we got.
            numQuickClicks ++;
            // This is the SECOND click??
            if (numQuickClicks == 2) { // to-do: Discount if mouse pos is too far from first down pos.
                isDoubleClick = true;
            }
            // Reset the timer to count another quick-click!
            timeWhenNullifyDoubleClick = Time.time + QUICK_CLICK_TIME_WINDOW;
        }
        // Have we nullified our double-click by waiting too long?
        if (Time.time >= timeWhenNullifyDoubleClick) {
            numQuickClicks = 0;
        }
    }
    

}

*/
