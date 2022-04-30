using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerUtils {
    public static bool IsLayerInLayermask(int layer, LayerMask layermask) {
        return layermask == (layermask | (1 << layer));
    }
    public static bool IsLayer(GameObject go, string layerName) {
        return go.layer == LayerMask.NameToLayer(layerName);
    }
}
