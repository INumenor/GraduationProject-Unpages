using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseReturnBaseState : IMouseState
{
    public MouseStateManager mouseStateManager { get; set; }

    public void EnterState()
    {
        CharacterRunning();
        mouseStateManager.targetFood = null;
        mouseStateManager.mouseAgent.SetDestination(mouseStateManager.mouseAgentBase.position);
    }

    public void ExitState()
    {
        CharacterDontRunning();
    }

    public void UpdateState()
    {
        if ((int)mouseStateManager.mouseAgentBase.position.x == (int)mouseStateManager.mouseAgent.transform.position.x &&
            (int)mouseStateManager.mouseAgentBase.position.z == (int)mouseStateManager.mouseAgent.transform.position.z)
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
