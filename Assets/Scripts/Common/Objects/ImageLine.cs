using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/** A lightweight alternative to LineRenderer. */
public class ImageLine : MonoBehaviour {
	// Components
	[SerializeField] private Image image=null; // the actual line
	// Properties
	private float angle; // in DEGREES.
	private float length;
	private float thickness = 1f;
	// References
	private Vector2 startPos;
	private Vector2 endPos;

	// Getters
    public RectTransform MyRectTransform { get { return image.rectTransform; } }
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

    public void SetStartAndEndPos (Line _line) {
        SetStartAndEndPos(_line.start, _line.end);
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
		Initialize (this.transform.parent, _startPos,_endPos);
	}
    public void Initialize (Transform _parentTransform) {
        Initialize(_parentTransform, Vector2.zero, Vector2.zero);
    }
	public void Initialize (Transform _parentTransform, Vector2 _startPos, Vector2 _endPos) {
		startPos = _startPos;
		endPos = _endPos;
        
        GameUtils.ParentAndReset(this.gameObject, _parentTransform);

		UpdateAngleLengthPosition ();
	}


	// ----------------------------------------------------------------
	//  Update Things
	// ----------------------------------------------------------------
	private void UpdateAngleLengthPosition() {
		// Update values
		angle = LineUtils.GetAngle_Degrees (startPos, endPos);
		length = LineUtils.GetLength (startPos, endPos);
		// Transform image!
		if (float.IsNaN (endPos.x)) {
			Debug.LogError ("Ahem! An ImageLine's endPos is NaN! (Its startPos is " + startPos + ".)");
		}
		this.GetComponent<RectTransform>().anchoredPosition = LineUtils.GetCenterPos(startPos, endPos); //.transform.localPosition
		this.transform.localEulerAngles = new Vector3 (0, 0, angle);
		SetThickness (thickness);
	}

	public bool IsVisible {
		get { return image.enabled; }
		set { image.enabled = value; }
	}
    public void SetAnchors(Vector2 anchorMin,Vector2 anchorMax) {
        MyRectTransform.anchorMin = anchorMin;
        MyRectTransform.anchorMax = anchorMax;
    }
	public void SetAlpha(float alpha) {
		GameUtils.SetUIGraphicAlpha(image, alpha);
	}
	public void SetColor(Color color) {
		image.color = color;
	}
	public void SetThickness(float _thickness) {
		thickness = _thickness;
		GameUtils.SizeUIGraphic(image, length, thickness);
	}


}




