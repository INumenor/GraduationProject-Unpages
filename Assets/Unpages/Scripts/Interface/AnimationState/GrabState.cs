using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabState : IState
{
    public StateManager stateManager { get; set; }

    public void EnterState()
    {
        RPC_CharacterGrabbing();
    }

    public void ExitState()
    {
        RPC_CharacterDontGrabbing();
    }

    public void UpdateState()
    {
        if (!stateManager.isGrabbing)
        {
            stateManager.ChangeState(new IdleState());
        }
        else if(stateManager.isJumping)
        {
            stateManager.ChangeState(new JumpState());
        }
            
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterGrabbing()
    {
       stateManager.characterAnimator.SetBool("isGrabbing", true);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterDontGrabbing()
    {
        stateManager.characterAnimator.SetBool("isGrabbing", false);
    }
}
