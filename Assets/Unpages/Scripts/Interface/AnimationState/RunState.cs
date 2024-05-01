using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : IState
{
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
        if (GameService.Instance.playerAction.isJumping)
        {
            GameService.Instance.playerAction.ChangeState(new JumpState());
        }
        else if (!GameService.Instance.playerAction.isRunning)
        {
            GameService.Instance.playerAction.ChangeState(new IdleState());
        }
        
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterRunning()
    {
        GameService.Instance.playerAnimationControl.characterAnimator.SetBool("isRunning", true);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterDontRunning()
    {
        GameService.Instance.playerAnimationControl.characterAnimator.SetBool("isRunning", false);
    }
}
