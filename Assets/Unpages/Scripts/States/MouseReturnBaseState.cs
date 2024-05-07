using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseReturnBaseState : IMouseState
{
    public MouseStateManager mouseStateManager { get; set; }

    public void EnterState()
    {
        mouseStateManager.mouseAgent.SetDestination(mouseStateManager.mouseAgentBase.position);
    }

    public void ExitState()
    {

    }

    public void UpdateState()
    {
        if (mouseStateManager.mouseAgentBase.position == mouseStateManager.mouseAgent.transform.position)
        {
            //bunlar network olucak !!!!!!!!!!!
            mouseStateManager.mouseGrabbleObject = null;
            mouseStateManager.targetFood = null;
            mouseStateManager.expiredFood.RemoveAt(0);
        }
        if (mouseStateManager.isCatch)
        {
            mouseStateManager.ChangeState(new MouseCatchState());
        }
    }
}
