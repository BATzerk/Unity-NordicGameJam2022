using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** For a chain of CubeLines. */
public class CubeLines : MonoBehaviour {
	// Components
	private List<CubeLine> lines;
	// Properties
	private Color lineColor;
	private float lineDepth;
	private float lineThickness;
	private List<Vector2> points;

	// Getters
	//	public List<Vector2> Points { get { return points; } }
	public int NumPoints { get { return points.Count; } }

	// ----------------------------------------------------------------
	//  Initialize
	// ----------------------------------------------------------------
	public void Initialize (Transform tf_parent) {
		GameUtils.ParentAndReset(this.gameObject, tf_parent);
		this.gameObject.name = "CubeLines";

		lines = new List<CubeLine>();
		points = new List<Vector2> ();
	}


	// ----------------------------------------------------------------
	//  Add/Remove/Set
	// ----------------------------------------------------------------
	public void AddPoint (Vector2 _point) {
		points.Add (_point);
		// If we have at least 2 points (to make a line)!...
		if (points.Count > 1) {
			AddLine (points[NumPoints-2], points[NumPoints-1]);
		}
	}
	public void RemovePoint () {
		points.RemoveAt (NumPoints-1);
		// If we at least have any lines left...
		if (lines.Count > 0) {
			RemoveLine ();
		}
	}
	private void AddLine (Vector2 startPos, Vector2 endPos) {
		CubeLine newLine = Instantiate(ResourcesHandler.Instance.go_CubeLine).GetComponent<CubeLine>();
		newLine.gameObject.name = "CubeLine" + lines.Count;
		newLine.transform.SetParent (this.transform);
		newLine.Initialize (startPos, endPos);
		newLine.SetColor (lineColor);
		newLine.SetThicknessAndDepth (lineThickness, lineDepth);
		lines.Add (newLine);
	}
	private void RemoveLine () {
		CubeLine sl = lines[lines.Count-1];
		lines.Remove (sl);
		Destroy (sl.gameObject);
	}

	public void SetPoint (int index, Vector2 _point) {
		points[index] = _point;
		// Update the corresponding lines!
		if (index>0) {
			lines[index-1].EndPos = _point;
		}
		if (index<NumPoints-1) {
			lines[index].StartPos = _point;
		}
	}

	// ----------------------------------------------------------------
	//  Visuals
	// ----------------------------------------------------------------
	public void SetActive (bool _isActive) {
		this.gameObject.SetActive (_isActive);
	}
	public void SetColor (Color _color) {
		lineColor = _color;
		foreach (CubeLine sl in lines) {
			sl.SetColor (lineColor);
		}
	}
	public void SetThicknessAndDepth (float _thickness, float _depth) {
		lineThickness = _thickness;
		lineDepth = _depth;
		foreach (CubeLine sl in lines) {
			sl.SetThicknessAndDepth (lineThickness, lineDepth);
		}
	}

}
