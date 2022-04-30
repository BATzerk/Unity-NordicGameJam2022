using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityVibrator : MonoBehaviour
{
    // Components
    [SerializeField] private SphereCollider myTrigger; // the trigger the Mitt overlaps with.
    // Properties
    [SerializeField] public float DistMax { get; private set; } = 0.4f;
    [SerializeField] public float DistMin { get; private set; } = 0.08f;

    // ----------------------------------------------------------------
    //  Start
    // ----------------------------------------------------------------
    void Start() {
        myTrigger.radius = DistMax;
    }

}
