using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    // Constants
    private const float RotationSpeed = 1f;
    // Components
    [SerializeField] private GameObject go_body;
    [SerializeField] private ParticleSystem ps_collected;
    // References
    private GameController myGC;
    // Properties
    private bool isCollected;
    private float TopSpeed;


    // ----------------------------------------------------------------
    // Initialize
    // ----------------------------------------------------------------
    public void Initialize(GameController _myGC, Transform tf_spawnSource) {
        this.myGC = _myGC;
        GameUtils.ParentAndReset(this.gameObject, tf_spawnSource);
        // Position!
        this.transform.localPosition = new Vector3(
            Random.Range(-0.4f, 0.4f),
            Random.Range(0.6f, 1.3f),
            10);
        this.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);

        // Define TopSpeed.
        TopSpeed = 0.026f * Random.Range(0.8f, 1.2f);


        GameUtils.SetParticleSystemEmissionEnabled(ps_collected, false);
    }

    private void DestroySelf() {
        Destroy(gameObject);
    }


    // ----------------------------------------------------------------
    // Update
    // ----------------------------------------------------------------
    void Update() {
        if (!isCollected) {
            // Move!
            this.transform.localPosition += new Vector3(0, 0, -TopSpeed) * Time.timeScale;
            go_body.transform.localEulerAngles += new Vector3(0, RotationSpeed, 0) * Time.timeScale;

            // Maybe destroy me if I've gone too far.
            if (this.transform.localPosition.z < -10) {
                DestroySelf();
            }
        }
    }


    // ----------------------------------------------------------------
    // Doers
    // ----------------------------------------------------------------
    public void GetCollected() {
        isCollected = true;
        go_body.SetActive(false);
        ps_collected.Emit(20);
        //GameUtils.SetParticleSystemEmissionEnabled(ps_collected, true);
        Invoke("DestroySelf", 20);
    }


}
