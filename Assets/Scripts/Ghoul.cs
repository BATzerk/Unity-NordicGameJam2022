using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoul : MonoBehaviour
{
    // Properties
    private Vector2 boundsX = new Vector2(-0.5f, 0.5f);
    private Vector2 boundsY = new Vector2(0.8f, 1.5f);
    private Vector2 boundsZ = new Vector2(0.3f, 0.8f);
    [SerializeField] private readonly float DriftSpeed = 0.03f; // applied to PerlinNoise
    private float seed0;
    private float seed1;
    private float seed2;
    private float seed3;
    private float seed4;
    private float seed5;


    // ----------------------------------------------------------------
    //  Start
    // ----------------------------------------------------------------
    void Start() {
        seed0 = Random.Range(-1000, 1000);
        seed1 = Random.Range(-1000, 1000);
        seed2 = Random.Range(-1000, 1000);
        seed3 = Random.Range(-1000, 1000);
        seed4 = Random.Range(-1000, 1000);
        seed5 = Random.Range(-1000, 1000);
    }


    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    void Update() {
        float xLoc = Mathf.PerlinNoise((Time.time+seed0)*DriftSpeed, (Time.time+seed1)*DriftSpeed);
        float yLoc = Mathf.PerlinNoise((Time.time+seed2)*DriftSpeed, (Time.time+seed3)*DriftSpeed);
        float zLoc = Mathf.PerlinNoise((Time.time+seed4)*DriftSpeed, (Time.time+seed5)*DriftSpeed);
        // Move around via PerlinNoise!
        this.transform.localPosition = new Vector3(
            Mathf.Lerp(boundsX.x,boundsX.y, xLoc),
            Mathf.Lerp(boundsY.x,boundsY.y, yLoc),
            Mathf.Lerp(boundsZ.x,boundsZ.y, zLoc)
        );
    }


}
