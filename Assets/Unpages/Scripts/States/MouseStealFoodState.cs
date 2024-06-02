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
                    mouseStateManager.targetFood = networkObject;
                    break;
                }
            }
            if (mouseStateManager.targetFood != null)
            {
                mouseStateManager.mouseAgent.SetDestination(mouseStateManager.targetFood.transform.position);
            }

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
        else
        {
            if (mouseStateManager.targetFood == null)
            {
                foreach (var networkObject in mouseStateManager.expiredFood)
                {
                    if (networkObject != null)
                    {
                        mouseStateManager.targetFood = networkObject;
                        mouseStateManager.mouseAgent.SetDestination(mouseStateManager.targetFood.transform.position);
                        break;
                    }
                }
                if (mouseStateManager.targetFood != null)
                {
                    mouseStateManager.mouseAgent.SetDestination(mouseStateManager.targetFood.transform.position);
                }
                else
                {
                    mouseStateManager.ChangeState(new MouseReturnBaseState());
                }
            }
        }
        
        //if (mouseStateManager.expiredFood.Count > 0)
        //{
        //    if (!mouseStateManager.expiredFood[0])
        //    {
        //        mouseStateManager.ChangeState(new MouseReturnBaseState());
        //    }
        //}     
        if (mouseStateManager.isCatch)
        {
            //foreach (var networkObject in mouseStateManager.expiredFood)
            //{
            //    if (networkObject != null)
            //    {
            //        mouseStateManager.targetFood = networkObject;
            //        mouseStateManager.mouseAgent.SetDestination(mouseStateManager.targetFood.transform.position);
            //        break;
            //    }
            //}
            //if (mouseStateManager.targetFood != null)
            //{
            //    mouseStateManager.mouseAgent.SetDestination(mouseStateManager.targetFood.transform.position);
            //}
            //else
            //{
            //    mouseStateManager.ChangeState(new MouseCatchState());
            //}

            mouseStateManager.ChangeState(new MouseCatchState());
        }
    }
    public void CharacterRunning()
    {   
        mouseStateManager.networkMouseAI.isRunning = true;
        mouseStateManager.networkMouseAI.MouseAnimatorController.SetBool("isRunning", true);
    }

    public void CharacterDontRunning()
    {
        mouseStateManager.networkMouseAI.isRunning = false;
        mouseStateManager.networkMouseAI.MouseAnimatorController.SetBool("isRunning", false);
    }
}
