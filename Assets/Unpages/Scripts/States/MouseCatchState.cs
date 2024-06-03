using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCatchState : IMouseState
{
    public MouseStateManager mouseStateManager { get; set; }

    public void EnterState()
    {
        if (mouseStateManager.mouseGrabbleObject)
        {
            CharacterJumping();
            mouseStateManager.networkMouseAI.DropItem(mouseStateManager.mouseGrabbleObject);
            mouseStateManager.mouseAgent.speed = 10;
            mouseStateManager.mouseGrabbleObject = null;
        }
        mouseStateManager.isCatch = false;
    }

    public void ExitState()
    {
        CharacterDontJumping();
    }

    public void UpdateState()
    {
        if (!mouseStateManager.mouseGrabbleObject)
        {
            mouseStateManager.ChangeState(new MouseReturnBaseState());
        }
    }
    public void CharacterJumping()
    {
        mouseStateManager.networkMouseAI.isJumping = true;
        mouseStateManager.networkMouseAI.MouseAnimatorController.SetBool("isJumping", true);
    }

    public void CharacterDontJumping()
    {
        mouseStateManager.networkMouseAI.isRunning = false;
        mouseStateManager.networkMouseAI.MouseAnimatorController.SetBool("isJumping", false);
    }
}
