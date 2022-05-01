using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulCore : MonoBehaviour
{
    // Components
    [SerializeField] private Ghoul myGhoul;


    // ----------------------------------------------------------------
    //  Events
    // ----------------------------------------------------------------
    public void OnMittEnterMe() {
        myGhoul.SetIsFoundTrue();
        myGhoul.IsCoreTouched = true;
    }
    public void OnMittExitMe() {
        myGhoul.IsCoreTouched = false;
    }

}
