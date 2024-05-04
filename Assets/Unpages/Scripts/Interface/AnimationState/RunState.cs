using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : IState
{
    public StateManager stateManager { get; set; }

    public void EnterState()
    {
        RPC_CharacterRunning();
    }

    public void ExitState()
    {
        RPC_CharacterDontRunning();
    }

    public void UpdateState()
    {
        if (stateManager.isJumping)
        {
           stateManager.ChangeState(new JumpState());
        }
        else if (!stateManager.isRunning)
        {
           stateManager.ChangeState(new IdleState());
        }
        
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterRunning()
    {
        stateManager.characterAnimator.SetBool("isRunning", true);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterDontRunning()
    {
        stateManager.characterAnimator.SetBool("isRunning", false);
    }
}
