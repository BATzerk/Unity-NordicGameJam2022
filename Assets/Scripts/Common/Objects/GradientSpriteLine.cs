using UnityEngine;
using System.Collections;


/** Like a SpriteLine, but gradients from one color to another! */

public class GradientSpriteLine : MonoBehaviour {
	// Components
	private SpriteRenderer spriteA;
	private SpriteRenderer spriteB;
	// Properties
	private Color colorA, colorB;
	private float angle;
	private float alphaA, alphaB;
	private float length;
	private float thickness = 1f;
	// References
	private Vector2 startPos;
	private Vector2 endPos;
	
	// Getters
	public float Length { get { return length; } }
	public Vector2 StartPos {
		get { return startPos; }
		set {
			if (startPos == value) { return; }
			startPos = value;
			UpdateAngleLengthPosition ();
		}
	}
	public Vector2 EndPos {
		get { return endPos; }
		set {
			if (endPos == value) { return; }
			endPos = value;
			UpdateAngleLengthPosition ();
		}
	}
	
	
	
	// ================================================================
	//  Initialize
	// ================================================================
	private void Awake() {
		// Make my SpriteRenderer!
		spriteA = new GameObject().AddComponent<SpriteRenderer> ();
		spriteB = new GameObject().AddComponent<SpriteRenderer> ();
		spriteA.gameObject.name = "GradientSpriteA";
		spriteB.gameObject.name = "GradientSpriteB";
		spriteA.gameObject.transform.SetParent (this.transform);
		spriteB.gameObject.transform.SetParent (this.transform);
		spriteA.transform.localPosition = Vector2.zero;
		spriteB.transform.localPosition = Vector2.zero;
		Sprite gradientSprite = Resources.Load<Sprite> (CommonPaths.WhiteGradientSprite);
		spriteA.sprite = gradientSprite;
		spriteB.sprite = gradientSprite;
		spriteB.transform.localEulerAngles = new Vector3 (0, 0, 180);
	}
	public void Initialize () {
		Initialize (Vector2.zero, Vector2.zero);
	}
	public void Initialize (Vector2 _startPos, Vector2 _endPos) {
		startPos = _startPos;
		endPos = _endPos;

		this.transform.localPosition = Vector3.zero;
		this.transform.localScale = Vector3.one;
		
		UpdateAngleLengthPosition ();
	}
	
	
	// ================================================================
	//  Update Things
	// ================================================================
	private void UpdateAngleLengthPosition() {
		// Update values
		angle = LineUtils.GetAngle_Degrees (startPos, endPos);
		length = LineUtils.GetLength (startPos, endPos);
		// Transform sprite!
		if (float.IsNaN (endPos.x)) {
			Debug.LogError ("Ahem! A SpriteLine's endPos is NaN! (Its startPos is " + startPos + ".)");
		}
		this.transform.localPosition = LineUtils.GetCenterPos(startPos, endPos);
		this.transform.localEulerAngles = new Vector3 (0, 0, angle);
		GameUtils.SizeSpriteRenderer (spriteA, length, thickness);
		GameUtils.SizeSpriteRenderer (spriteB, length, thickness);
	}
	
	public bool IsVisible {
		get { return spriteA.enabled; }
		set {
			spriteA.enabled = value;
			spriteB.enabled = value;
		}
	}
	public void SetAlphas (float _alphaA, float _alphaB) {
		alphaA = _alphaA;
		alphaB = _alphaB;
		ApplyColors ();
	}
	public void SetColors(Color _colorA, Color _colorB) {
		colorA = _colorA;
		colorB = _colorB;
		ApplyColors ();
	}
	private void ApplyColors () {
		spriteA.color = new Color (colorA.r,colorA.g,colorA.b, colorA.a * alphaA);
		spriteB.color = new Color (colorB.r,colorB.g,colorB.b, colorB.a * alphaB);
	}
	public void SetSortingOrder(int sortingOrder) {
		spriteA.sortingOrder = sortingOrder;
		spriteB.sortingOrder = sortingOrder;
	}
	public void SetThickness(float _thickness) {
		thickness = _thickness;
		GameUtils.SizeSpriteRenderer(spriteA, length, thickness);
		GameUtils.SizeSpriteRenderer(spriteB, length, thickness);
	}
	
	
}




