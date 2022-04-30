using UnityEngine;
using System.Collections;


/** All dashed lines, rendered with a MeshRenderer! */
public class DashedLine : MonoBehaviour {
	// Components
	[SerializeField] private MeshRenderer meshRenderer=null;
	// Properties
	private float angle; // in DEGREES.
	private float dashSize; // how many pixels long each dash is. Pretty simple!
	private float length;
	private float thickness = 1f;
	// References
	private Vector2 startPos;
	private Vector2 endPos;

	// Getters / Setters
	public float Angle { get { return angle; } }
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
	/** Use this to scroll OneWayStreets. */
	public float TextureOffsetX {
		get { return meshRenderer.material.mainTextureOffset.x; }
		set { meshRenderer.material.mainTextureOffset = new Vector2 (value, meshRenderer.material.mainTextureOffset.y); }
	}

	public void SetStartAndEndPos (Vector2 _startPos, Vector2 _endPos) {
		startPos = _startPos;
		endPos = _endPos;
		UpdateAngleLengthPosition ();
	}



	// ================================================================
	//  Initialize
	// ================================================================
	public void Initialize (float _dashSize) {
		Initialize (Vector2.zero,Vector2.zero, _dashSize);
	}
	public void Initialize (Vector2 _startPos, Vector2 _endPos, float _dashSize) {
		startPos = _startPos;
		endPos = _endPos;
		dashSize = _dashSize;

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
			Debug.LogError ("Ahem! A DashedLine's endPos is NaN! (Its startPos is " + startPos + ".)");
		}
		Vector2 centerPos = LineUtils.GetCenterPos (startPos, endPos);
		this.transform.localPosition = new Vector3 (centerPos.x, centerPos.y, this.transform.localPosition.z);
		this.transform.localEulerAngles = new Vector3 (0, 0, angle);
		ApplyLengthAndThickness ();
	}
	private void ApplyLengthAndThickness () {
		this.transform.localScale = new Vector3 (length, thickness, this.transform.localScale.z);
		// Set textureScale properly!
		float textureScaleX = 0.5f/dashSize; // I did the math: With a 16-pixel-wide dash (8px filled, 8px transparent), the pixel-perfect factor is 0.5f.
		meshRenderer.material.mainTextureScale = new Vector2 (textureScaleX*length, meshRenderer.material.mainTextureScale.y);
	}

	public bool IsVisible {
		get { return meshRenderer.enabled; }
		set { meshRenderer.enabled = value; }
	}
//	public void SetAlpha(float alpha) {
////		Color color = meshRenderer.material.GetColor ("_MainColor");
//		Color color = meshRenderer.material.color;
//		SetColor (new Color (color.r,color.g,color.b, alpha));
//	}
	public void SetColor(Color color) {
		meshRenderer.material.color = color;
//		meshRenderer.material.SetColor ("_MainColor", color);
	}
	public void SetColor(Color color, float alpha) {
		SetColor (new Color (color.r,color.g,color.b, alpha));
	}
	public void SetThickness(float _thickness) {
		thickness = _thickness;
		ApplyLengthAndThickness ();
	}


}




