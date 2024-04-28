using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    public Animator characterAnimator;


    public void CharacterIdle()
    {
        characterAnimator.SetBool("isRunning", false);
        characterAnimator.SetBool("isGrabbing", false);
    }

    public void CharacterGrabbing()
    {

    }

    public void CharacterRunning() 
    {

    }
}
