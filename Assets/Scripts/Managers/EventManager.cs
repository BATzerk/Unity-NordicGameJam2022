using UnityEngine;
using System.Collections;

public class EventManager {
	// Events
	public delegate void NoParamAction ();
	public delegate void BoolAction (bool _bool);
	public delegate void FloatAction (float _float);
	public delegate void IntAction (int _int);
	public delegate void Vector2Action (Vector2 _vector);

	// Common
	public delegate void ColorFloatBoolAction(Color c, float f, bool b);
	public delegate void ColorColorFloatBoolAction(Color cA, Color cB, float f, bool b);
	public event NoParamAction FullScrim_HideEvent;
	public event ColorFloatBoolAction FullScrim_ShowEvent;
	public event ColorColorFloatBoolAction FullScrim_FadeFromAToBEvent;
	public event NoParamAction ChangeCurrCursorSourceEvent;

	// Game-Specific
	//public event NoParamAction PauseMenuOpenedEvent;
	//public event NoParamAction PauseMenuClosedEvent;
	//public event NoParamAction PauseScreenClosedEvent;
	//public event NoParamAction PauseScreenOpenedEvent;


	public void FullScrim_Hide() { FullScrim_HideEvent?.Invoke(); }
	public void FullScrim_Show(Color color, float alpha, bool useHighestAlpha=true) { FullScrim_ShowEvent?.Invoke(color, alpha, useHighestAlpha); }
	public void FullScrim_FadeFromAtoB(Color startColor, Color endColor, float duration, bool useUnscaledTime) { FullScrim_FadeFromAToBEvent?.Invoke(startColor,endColor,duration,useUnscaledTime); }
	public void OnChangeCurrCursorSource() { ChangeCurrCursorSourceEvent?.Invoke(); }


}




