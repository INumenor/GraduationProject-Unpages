using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCatchState : IMouseState
{
    public MouseStateManager mouseStateManager { get; set; }

    public void EnterState()
    {
        Debug.Log("enter'da");
        if (mouseStateManager.mouseGrabbleObject)
        {
            mouseStateManager.networkMouseAI.DropItem(mouseStateManager.mouseGrabbleObject);
            mouseStateManager.isCatch = false;
            mouseStateManager.mouseAgent.speed = 50;
            mouseStateManager.mouseGrabbleObject = null;
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
