using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {
    // Constants
    public const float TargetFrameRate = 60;
    // Properties
    static public float FrameTimeScale { get; private set; }
    static public float FrameTimeScaleUnscaled { get; private set; }


    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    private void Update () {
		FrameTimeScale = Application.targetFrameRate * Time.deltaTime;
		FrameTimeScaleUnscaled = Application.targetFrameRate * Time.unscaledDeltaTime;
	}



}
