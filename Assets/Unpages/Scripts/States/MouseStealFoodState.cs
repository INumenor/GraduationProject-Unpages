using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseStealFoodState : IMouseState
{
    public MouseStateManager mouseStateManager { get; set; }

    public void EnterState()
    {
       if(mouseStateManager.expiredFood.Count>0 && !mouseStateManager.mouseGrabbleObject)
        {
            mouseStateManager.targetFood = mouseStateManager.expiredFood[0];
            mouseStateManager.mouseAgent.SetDestination(mouseStateManager.targetFood.transform.position);
           
        }
    }

    public void ExitState()
    {
       
    }

    public void UpdateState()
    {
        float targetDistance = Vector3.Distance(mouseStateManager.mouseAgent.transform.position, mouseStateManager.targetFood.transform.position);
        if (mouseStateManager.mouseGrabbleObject && targetDistance<0.1f)
        {
            mouseStateManager.ChangeState(new MouseReturnBaseState());
        }
        if(mouseStateManager.targetFood.transform != mouseStateManager.expiredFood[0].transform)
        {
            mouseStateManager.ChangeState(new MouseReturnBaseState());
        }
        if (mouseStateManager.isCatch)
        {
            mouseStateManager.ChangeState(new MouseCatchState());
        }
    }
}
