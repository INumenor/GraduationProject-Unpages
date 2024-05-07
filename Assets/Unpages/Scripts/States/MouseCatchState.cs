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
            mouseStateManager.isCatch = false;
            mouseStateManager.mouseAgent.speed = 50;
        }
    }

    public void ExitState()
    {
        
    }

    public void UpdateState()
    {
        if (!mouseStateManager.mouseGrabbleObject)
        {
            mouseStateManager.ChangeState(new MouseReturnBaseState());
        }
    }
}
