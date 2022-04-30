using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FullScrim : MonoBehaviour {
	// Components
	[SerializeField] private Image scrimImage=null;
	// Properties
	private bool useUnscaledTime; // if this is true, I'll fade IRRELEVANT of Time.timeScale
	private Color startColor;
	private Color endColor;
	private float fadeDuration; // in SECONDS, how long it'll take to get from startColor to endColor.
	private float timeUntilFinishFade = -1; // fadeDuration, but counts down.

	// Getters
	public bool IsFading { get { return timeUntilFinishFade >= 0; } }


	// ================================================================
	//	Awake / Destroy
	// ================================================================
	private void Awake() {
		Hide();
		// Add event listeners!
		//GameManagers.Instance.EventManager.FullScrim_FadeFromAToBEvent += FadeFromAtoB;
		//GameManagers.Instance.EventManager.FullScrim_HideEvent += Hide;
		//GameManagers.Instance.EventManager.FullScrim_ShowEvent += Show;
	}
    private void OnDestroy() {
		// Remove event listeners!
		//GameManagers.Instance.EventManager.FullScrim_FadeFromAToBEvent -= FadeFromAtoB;
		//GameManagers.Instance.EventManager.FullScrim_HideEvent -= Hide;
		//GameManagers.Instance.EventManager.FullScrim_ShowEvent -= Show;
	}


    // ================================================================
    //	Update
    // ================================================================
    private void Update () {
		// If I'm visible AND fading colors!...
		if (scrimImage.enabled && timeUntilFinishFade>0) {
			// Update timeUntilFinishFade!
			timeUntilFinishFade -= useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
			// Update color!
			scrimImage.color = Color.Lerp (startColor, endColor, 1-timeUntilFinishFade/fadeDuration);
			// DONE fading??
			if (timeUntilFinishFade <= 0) {
				timeUntilFinishFade = -1;
				// Faded to totally clear?? Then go ahead and HIDE the image completely. :)
				if (endColor.a == 0) {
					scrimImage.enabled = false;
				}
			}
		}
	}


	// ================================================================
	//	Doers
	// ================================================================
	public void Show (float blackAlpha) {
        Show(Color.black, blackAlpha, false);
    }
    /** useHighestAlpha: If TRUE, then I'll only set my alpha to something HIGHER than it already is. */
    public void Show (Color color, float alpha, bool useHighestAlpha=true) {
		scrimImage.enabled = true;
		if (useHighestAlpha) {
			alpha = Mathf.Max (scrimImage.color.a, alpha);
		}
		scrimImage.color = new Color (color.r,color.g,color.b, alpha);
		//		GameUtils.SetUIGraphicAlpha (scrimImage, alpha);
	}
	public void Hide () {
		scrimImage.enabled = false;
		timeUntilFinishFade = -1;
	}

	public void FadeFromAtoB (Color _startColor, Color _endColor, float _fadeDuration, bool _useUnscaledTime) {
		startColor = _startColor;
		endColor = _endColor;
		fadeDuration = timeUntilFinishFade = _fadeDuration;
		useUnscaledTime = _useUnscaledTime;
		// Prep scrimImage!
		scrimImage.color = startColor;
		scrimImage.enabled = true;
	}
	/** This fades from exactly where we ARE to a target color. */
	public void FadeToB (Color _endColor, float _fadeDuration, bool _useUnscaledTime) {
		FadeFromAtoB (scrimImage.color, _endColor, _fadeDuration, _useUnscaledTime);
	}



}




