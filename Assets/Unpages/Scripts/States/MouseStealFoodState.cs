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
        Debug.Log("S�ra : 1");
        if (mouseStateManager.mouseGrabbleObject)
        {
            Debug.Log("S�ra : 6");
            mouseStateManager.ChangeState(new MouseReturnBaseState());
        }
        
        if(mouseStateManager.targetFood == null)
        {
            //Debug.Log("S�ra : 2");
            //foreach (var networkObject in mouseStateManager.expiredFood)
            //{
            //    if (networkObject != null)
            //    {
            //        mouseStateManager.targetFood = networkObject;
            //        mouseStateManager.mouseAgent.SetDestination(mouseStateManager.targetFood.transform.position);
            //        break;
            //    }
            //}
            //Debug.Log("S�ra : 3");
            //if (mouseStateManager.targetFood != null)
            //{
            //    Debug.Log("S�ra : 4");
            //    mouseStateManager.mouseAgent.SetDestination(mouseStateManager.targetFood.transform.position);
            //}
            //else
            //{
                //Debug.Log("S�ra : 5");
                mouseStateManager.ChangeState(new MouseReturnBaseState());
            //}
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
                mouseStateManager.ChangeState(new MouseCatchState());
            }
            
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
