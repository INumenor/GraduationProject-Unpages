using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseStealFoodState : IMouseState
{
    public MouseStateManager mouseStateManager { get; set; }

    public void EnterState()
    {
    
        if (mouseStateManager.expiredFood.Count>0 && !mouseStateManager.mouseGrabbleObject)
        {
            CharacterRunning();
            foreach (var networkObject in mouseStateManager.expiredFood)
            {
                if(networkObject != null)
                {
                    Debug.Log("Easy");
                    mouseStateManager.targetFood = networkObject;
                    break;
                }
            }
            mouseStateManager.mouseAgent.SetDestination(mouseStateManager.targetFood.transform.position);
           
        }
    }

    public void ExitState()
    {
        CharacterDontRunning();
    }

    public void UpdateState()
    {
        if (mouseStateManager.mouseGrabbleObject)
        {
            mouseStateManager.ChangeState(new MouseReturnBaseState());
        }
        if(mouseStateManager.targetFood == null)
        {
            mouseStateManager.ChangeState(new MouseReturnBaseState());
        }
        if (mouseStateManager.expiredFood.Count > 0)
        {
            if (!mouseStateManager.expiredFood[0])
            {
                mouseStateManager.ChangeState(new MouseReturnBaseState());
            }
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
