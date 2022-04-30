using UnityEngine;
using System.Collections;


/** For when you need a 2D line that's actually a cube. */
public class CubeLine : MonoBehaviour {
	// Components
	[SerializeField] private MeshRenderer meshRenderer=null; // the actual line cube thingy
	// Properties
	private float angle; // in DEGREES.
	private float length; // x coords
	private float thickness = 1f; // y coords
	private float depth = 1f; // z coords
	// References
	private Vector2 startPos;
	private Vector2 endPos;

	// Getters
	public float Angle { get { return angle; } }
	public float Length { get { return length; } }
	public Material Material { get { return meshRenderer.material; } }
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

	public void SetStartAndEndPos (Vector2 _startPos, Vector2 _endPos) {
		startPos = _startPos;
		endPos = _endPos;
		UpdateAngleLengthPosition ();
	}



	// ----------------------------------------------------------------
	//  Initialize
	// ----------------------------------------------------------------
    public void Initialize () {
		Initialize (Vector2.zero, Vector2.zero);
	}
	public void Initialize (Vector2 _startPos, Vector2 _endPos) {
		startPos = _startPos;
		endPos = _endPos;

		UpdateAngleLengthPosition ();
	}


	// ----------------------------------------------------------------
	//  Update Things
	// ----------------------------------------------------------------
	private void UpdateAngleLengthPosition() {
		// Update values
		angle = LineUtils.GetAngle_Degrees(startPos, endPos);
		length = LineUtils.GetLength(startPos, endPos);
		// Transform sprite!
		if (float.IsNaN(endPos.x)) {
			Debug.LogError("Ahem! A SpriteLine's endPos is NaN! (Its startPos is " + startPos + ".)");
		}
		this.transform.localPosition = LineUtils.GetCenterPos(startPos, endPos);
		this.transform.localEulerAngles = new Vector3(0, 0, angle);
		ApplyScale();
	}
	private void ApplyScale() {
		// If one of our values would make us invisible, then make us COMPLETELY invisible. So we don't get weird, black, flat squares.
		if (length == 0 || thickness == 0 || depth == 0) {
			meshRenderer.transform.localScale = Vector3.zero;
		}
		else {
			meshRenderer.transform.localScale = new Vector3(length, thickness, depth);
		}
	}


	public bool IsVisible {
		get { return meshRenderer.enabled; }
		set { meshRenderer.enabled = value; }
	}
	public void SetAlpha(float alpha) {
        //		Color color = meshRenderer.material.GetColor ("_MainColor");
        Color color = meshRenderer.material.color;
        SetColor(new Color(color.r, color.g, color.b, alpha));
    }
    public void SetColor(Color color) {
		Color emissionColor = new Color(color.r*0.3f, color.g*0.3f, color.b*0.3f);
		meshRenderer.material.color = color;
		meshRenderer.material.SetColor("_Color", color);
		meshRenderer.material.SetColor("_EmissionColor", emissionColor);
	}
	public void SetColor(Color color, float alpha) {
		SetColor(new Color(color.r, color.g, color.b, alpha));
	}
	public void SetThicknessAndDepth(float _thickness, float _depth) {
		thickness = _thickness;
		depth = _depth;
		ApplyScale();
	}


	/** Replace my default, solid square sprite with a different sprite! Basically used for gradient lines. */
	public void SetMaterial(Material _mat) {
		meshRenderer.material = _mat;
		//UpdateAngleLengthPosition();
	}
	public void MakeMaterialGradient() {
		Material mat = Resources.Load<Material>(CommonPaths.mat_WhiteGradient);
		if (mat == null) {
			Debug.LogError("No gradient material found at path: " + CommonPaths.mat_WhiteGradient);
			return;
		}
		SetMaterial(mat);
	}


}




