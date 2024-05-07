using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseIdleState : IMouseState
{
    public MouseStateManager mouseStateManager { get ; set ; }

    public void EnterState()
    {
        mouseStateManager.isCatch = false;
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
