using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class Bullet : MonoBehaviour
{
    // Properties
    [SerializeField] private float Speed = 0.1f;
    // References
    private GameController gameController;



    // ----------------------------------------------------------------
    // Initialize / Destroy
    // ----------------------------------------------------------------
    public void Initialize(GameController gameController, Transform spawnTF) {
        this.gameController = gameController;
        this.transform.position = spawnTF.position;
        this.transform.rotation = spawnTF.rotation;
        this.transform.position += this.transform.forward * 0.5f;
        Invoke("DestroySelf", 10f);
    }

    private void DestroySelf() {
        Destroy(this.gameObject);
    }



    // ----------------------------------------------------------------
    // FixedUpdate
    // ----------------------------------------------------------------
    private void FixedUpdate() {
        this.transform.position += this.transform.forward * Speed;

        // Raycast backwards-- did I just hit a ghoul?
    }



    // ----------------------------------------------------------------
    // Events
    // ----------------------------------------------------------------
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "GhoulBodyTrigger") {
            // Capture the ghoul!
            Ghoul ghoul = other.GetComponentInParent<Ghoul>();
            if (ghoul != null) {
                gameController.OnBulletHitGhoul(this, ghoul);
            }
        }
    }

}
*/
