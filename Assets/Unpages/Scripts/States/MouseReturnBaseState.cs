using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseReturnBaseState : IMouseState
{
    public MouseStateManager mouseStateManager { get; set; }

    public void EnterState()
    {
        Debug.Log("returnbase'da");
        mouseStateManager.mouseAgent.SetDestination(mouseStateManager.mouseAgentBase.position);
    }

    public void ExitState()
    {

    }

    public void UpdateState()
    {
        if (mouseStateManager.mouseAgentBase.position.x == mouseStateManager.mouseAgent.transform.position.x &&
            mouseStateManager.mouseAgentBase.position.z == mouseStateManager.mouseAgent.transform.position.z)
        {
            //bunlar network olucak !!!!!!!!!!!        
            mouseStateManager.ChangeState(new MouseIdleState());
        }
        if (mouseStateManager.isCatch)
        {
            mouseStateManager.ChangeState(new MouseCatchState());
        }
    }
}
