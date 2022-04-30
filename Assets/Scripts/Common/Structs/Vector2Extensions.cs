using UnityEngine;

public static class Vector2Extensions {
    public static readonly Vector2 NaN = new Vector2(float.NaN, float.NaN); // Note: Inappropriate to be in this class.
    public static bool IsNaN(Vector2 v) { return float.IsNaN(v.x); } // Note: Inappropriate to be in this class.

    public static Vector2Int ToVector2Int(this Vector2 vector) {
        return new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    }
}