using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenState : IState
{
    public StateManager stateManager { get; set; }

    public void EnterState()
    {
        RPC_CharacterKichenAction();
    }

    public void ExitState()
    {
        RPC_CharacterDontKichenAction();
    }

    public void UpdateState()
    {
        if (!stateManager.isKitchenAction)
        {
            stateManager.ChangeState(new IdleState());
        }
    }

    public void RPC_CharacterKichenAction()
    {
        stateManager.characterAnimator.SetBool("isKitchenAction", true);
    }

    public void RPC_CharacterDontKichenAction()
    {
        stateManager.characterAnimator.SetBool("isKitchenAction", false);
    }
}
