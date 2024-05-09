using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MouseIdleState : IMouseState
{
    public MouseStateManager mouseStateManager { get ; set ; }

    public void EnterState()
    {
        mouseStateManager.isCatch = false;         
        mouseStateManager.mouseGrabbleObject = null;
        mouseStateManager.mouseAgent.speed = 10;
    }

    public void ExitState()
    {
      
    }

    public void UpdateState()
    {
      if(mouseStateManager.expiredFood.Count>0 && !mouseStateManager.mouseGrabbleObject)
        {
            mouseStateManager.ChangeState(new MouseStealFoodState());
        }
    }

}
