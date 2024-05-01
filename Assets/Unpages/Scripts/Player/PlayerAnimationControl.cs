using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    public Animator characterAnimator;

    private void Start()
    {
        GameService.Instance.playerAnimationControl = this;
    }

    public void CharacterIdle()
    {
        characterAnimator.SetBool("isRunning", false);
        characterAnimator.SetBool("isGrabbing", false);
    }

    public void CharacterGrabbing()
    {
        characterAnimator.SetBool("isGrabbing", true);
    }

    public void CharacterRunning() 
    {
        characterAnimator.SetBool("isRunning", true);
    }
}
