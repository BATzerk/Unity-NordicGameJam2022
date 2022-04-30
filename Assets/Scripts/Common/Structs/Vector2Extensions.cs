using Unity.XR.CoreUtils;
using UnityEngine;

public static class Vector2Extensions {
    public static readonly Vector2 NaN = new Vector2(float.NaN, float.NaN); // Note: Inappropriate to be in this class.
    public static bool IsNaN(Vector2 v) { return float.IsNaN(v.x); } // Note: Inappropriate to be in this class.

    public static Vector2Int ToVector2Int(this Vector2 vector) {
        return new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    }
    
    public static Vector2 xy(this Vector3 v) => new Vector2(v.x, v.y);
    public static Vector2 xz(this Vector3 v) => new Vector2(v.x, v.z);
    public static Vector2 yx(this Vector3 v) => new Vector2(v.y, v.x);
    public static Vector2 yz(this Vector3 v) => new Vector2(v.y, v.z);
    public static Vector2 zx(this Vector3 v) => new Vector2(v.z, v.y);
    public static Vector2 zy(this Vector3 v) => new Vector2(v.z, v.x);
    public static Vector2 Abs(this Vector2 v) => new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
}