using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MouseIdleState : IMouseState
{
    public MouseStateManager mouseStateManager { get ; set ; }

    public void EnterState()
    {
        CharacterIdle();
        mouseStateManager.isCatch = false;         
        mouseStateManager.mouseGrabbleObject = null;
        mouseStateManager.mouseAgent.speed = 10;
    }

    public void ExitState()
    {
        CharacterDontIdle();
    }

    public void UpdateState()
    {
      if(mouseStateManager.expiredFood.Count>0 && !mouseStateManager.mouseGrabbleObject)
        {
            Debug.Log("burasý 2");
            mouseStateManager.ChangeState(new MouseStealFoodState());
        }
    }
    public void CharacterIdle()
    {
        mouseStateManager.MouseAnimatorController.SetBool("isIdle", true);
        mouseStateManager.MouseAnimatorController.SetBool("isRunning", false);
        mouseStateManager.MouseAnimatorController.SetBool("isJumping", false);
    }
    public void CharacterDontIdle()
    {
        mouseStateManager.MouseAnimatorController.SetBool("isIdle",false); 
    }
}
