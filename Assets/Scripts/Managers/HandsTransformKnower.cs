using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.EventSystems;

/** A bit hacky. A static class that always has an idea where the VR hands are. */
public class HandsTransformKnower : MonoBehaviour {
    // Properties
    [SerializeField] private ActionBasedController controllerHandL;
    [SerializeField] private ActionBasedController controllerHandR;
    //[SerializeField] private Transform tf_handR;
    public static Transform HandL;
    public static Transform HandR;
    public static ActionBasedController ControllerHandL;
    public static ActionBasedController ControllerHandR;

    // Awake
    void Awake() {
        ControllerHandL = controllerHandL;
        ControllerHandR = controllerHandR;
        HandL = controllerHandL.transform;
        HandR = controllerHandR.transform;
    }

    //private Collider prevColliderOver;
    //private void Update() {
    //    if (prevColliderOver != InputManager.Instance.CurrCursorCollider) {
    //        GameManagers.Instance.VibrationManager.Vibrate(0.02f, 0.15f);
    //    }
    //    prevColliderOver = InputManager.Instance.CurrCursorCollider;
    //}

}
