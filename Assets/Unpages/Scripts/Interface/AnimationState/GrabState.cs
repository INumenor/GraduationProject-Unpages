using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabState : IState
{
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
        if (!GameService.Instance.playerAction.isGrabbing)
        {
            GameService.Instance.playerAction.ChangeState(new IdleState());
        }
        else if(GameService.Instance.playerAction.isJumping)
        {
            GameService.Instance.playerAction.ChangeState(new JumpState());
        }
            
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterGrabbing()
    {
        GameService.Instance.playerAnimationControl.characterAnimator.SetBool("isGrabbing", true);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterDontGrabbing()
    {
        GameService.Instance.playerAnimationControl.characterAnimator.SetBool("isGrabbing", false);
    }
}
