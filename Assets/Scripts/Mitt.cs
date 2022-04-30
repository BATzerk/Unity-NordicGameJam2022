using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mitt : MonoBehaviour
{
    // References
    [SerializeField] private GameController gameController;

    private void OnTriggerEnter(Collider other) {
        //Debug.Log("OnTriggerEnter: " + other.gameObject);
        Gem gem = other.gameObject.GetComponentInParent<Gem>();
        if (gem != null) {
            gem.GetCollected();
            //gameController.OnCollectedAGem();
        }
    }
}
