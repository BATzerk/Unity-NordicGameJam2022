using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoul : MonoBehaviour
{
    // Components
    [SerializeField] private GameObject go_bodyVisuals;
    [SerializeField] private MeshRenderer mr_body;
    // Properties
    [SerializeField] private readonly float DriftSpeed = 0.04f; // applied to PerlinNoise
    [SerializeField] private readonly float SpeedUpMultiplier = 3f; // applied to PerlinNoise
    private Vector2 boundsX = new Vector2(-0.5f, 0.5f);
    private Vector2 boundsY = new Vector2(0.5f, 1.5f);
    private Vector2 boundsZ = new Vector2(0.3f, 1.8f);
    private bool isSlayed = false;
    public bool IsFound { get; private set; } = false;
    public bool IsCoreTouched=false;// { get; private set; } = false; // currently touched.
    private float currDriftTime; // incremented every frame. Can be slowed or sped up.
    private float seed0, seed1, seed2, seed3, seed4, seed5;

    public float SpeedUpTimeLeft = 0f;
    // References
    [SerializeField] private Material m_slayed;


    // ----------------------------------------------------------------
    //  Start
    // ----------------------------------------------------------------
    void Start() {
        go_bodyVisuals.SetActive(false);
        currDriftTime = Random.Range(0, 1000); // random.
        // Set my random seedz.
        seed0 = Random.Range(-1000, 1000);
        seed1 = Random.Range(-1000, 1000);
        seed2 = Random.Range(-1000, 1000);
        seed3 = Random.Range(-1000, 1000);
        seed4 = Random.Range(-1000, 1000);
        seed5 = Random.Range(-1000, 1000);
    }
    private void DestroySelf() {
        Destroy(this.gameObject);
    }


    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    void Update() {
        // Update position.
        if (isSlayed) {
            this.transform.position += new Vector3(0, 0.006f, 0);
        }
        else {
            SpeedUpTimeLeft = Mathf.Max(SpeedUpTimeLeft - Time.deltaTime, 0);
            float driftSpeedScale = SpeedUpTimeLeft > 0 ? SpeedUpMultiplier : (IsCoreTouched ? 0 : 1);
            float currDriftTimeDelta = Time.deltaTime * driftSpeedScale;
            currDriftTime += Time.deltaTime * currDriftTimeDelta;
            float xLoc = Mathf.PerlinNoise((currDriftTime+seed0)*DriftSpeed, (currDriftTime+seed1)*DriftSpeed);
            float yLoc = Mathf.PerlinNoise((currDriftTime+seed2)*DriftSpeed, (currDriftTime+seed3)*DriftSpeed);
            float zLoc = Mathf.PerlinNoise((currDriftTime+seed4)*DriftSpeed, (currDriftTime+seed5)*DriftSpeed);
            // Move around via PerlinNoise!
            this.transform.localPosition = new Vector3(
                Mathf.Lerp(boundsX.x, boundsX.y, xLoc),
                Mathf.Lerp(boundsY.x, boundsY.y, yLoc),
                Mathf.Lerp(boundsZ.x, boundsZ.y, zLoc)
            );
        }
    }



    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    public void SlayMe() {
        isSlayed = true;
        // Disable all triggers and everything!
        foreach (Collider col in GetComponentsInChildren<Collider>()) {
            col.enabled = false;
        }
        // Change my material and gtfo!
        mr_body.enabled = true;
        mr_body.material = m_slayed;
        Invoke("DestroySelf", 1.5f);
    }
    public void SetIsFoundTrue() {
        if (IsFound) return; // Safety check; don't do it again.
        IsFound = true;
        go_bodyVisuals.SetActive(true);
    }


}
