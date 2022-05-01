using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityVibrator : MonoBehaviour
{
    // Components
    [SerializeField] private SphereCollider myTrigger; // the trigger the Mitt overlaps with.
    // Properties
    [SerializeField] public float DistMax = 0.6f;
    [SerializeField] public float DistMin = 0.08f;

    // ----------------------------------------------------------------
    //  Start
    // ----------------------------------------------------------------
    void Start() {
        myTrigger.radius = DistMax;
    }

}
