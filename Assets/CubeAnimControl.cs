using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAnimControl : MonoBehaviour
{
    public Animator animator;

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayDeathAnimation();
        }
    }

    public void PlayDeathAnimation()
    {
        animator.SetBool("isDead", true);
    }
    
    
}
