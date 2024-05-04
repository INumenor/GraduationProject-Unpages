using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    public StateManager stateManager { get; set; }

    public void EnterState()
    {
        Debug.Log("sela");
        //RPC_CharacterIdle();
    }

    public void ExitState()
    {
        Debug.Log("Exit");
    }

    public void UpdateState()
    {
        if(stateManager.isRunning)
        {
            stateManager.ChangeState(new RunState());
        }
        else if (stateManager.isGrabbing)
        {
            stateManager.ChangeState(new GrabState());
        }
        else if (stateManager.isJumping 
            && !stateManager.isGrabbing)
        {
            stateManager.ChangeState(new JumpState());
        }
        else if (stateManager.isKitchenAction)
        {
            stateManager.ChangeState(new KitchenState());
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterIdle()
    {
        stateManager.characterAnimator.SetBool("isRunning", false);
        stateManager.characterAnimator.SetBool("isGrabbing", false);
        stateManager.characterAnimator.SetBool("isJumping", false);
    }
}
