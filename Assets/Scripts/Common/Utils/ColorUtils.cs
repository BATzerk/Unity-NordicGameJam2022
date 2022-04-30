using UnityEngine;

public static class ColorUtils {

    // ----------------------------------------------------------------
    //  Converting
    // ----------------------------------------------------------------
	static public string ColorToHex(Color32 color) {
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}

	static public Color HexToColor(int hexInt) { return HexToColor(hexInt.ToString()); }
	static public Color HexToColor(string hex) {
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}
    
    
    // ----------------------------------------------------------------
    //  Easing
    // ----------------------------------------------------------------
    /** This function simulates ONE frame's worth of our standard easing, but from one COLOR to another. :)
     easing: Higher is slower. */
    public static Color EaseColorOneStep (Color appliedColor, Color targetColor, float easing) {
        return new Color (appliedColor.r+(targetColor.r-appliedColor.r)/easing, appliedColor.g+(targetColor.g-appliedColor.g)/easing, appliedColor.b+(targetColor.b-appliedColor.b)/easing);
    }
    
}

