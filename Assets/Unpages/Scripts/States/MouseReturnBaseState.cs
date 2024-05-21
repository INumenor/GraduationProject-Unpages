using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseReturnBaseState : IMouseState
{
    public MouseStateManager mouseStateManager { get; set; }

    public void EnterState()
    {
        CharacterRunning();
        mouseStateManager.mouseAgent.SetDestination(mouseStateManager.mouseAgentBase.position);
    }

    public void ExitState()
    {
        CharacterDontRunning();
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
    public void CharacterRunning()
    {
        mouseStateManager.MouseAnimatorController.SetBool("isRunning", true);
    }

    public void CharacterDontRunning()
    {
        mouseStateManager.MouseAnimatorController.SetBool("isRunning", false);
    }
}
