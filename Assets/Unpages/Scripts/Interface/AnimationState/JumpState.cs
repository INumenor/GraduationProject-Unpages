using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState
{
    public void EnterState()
    {
        RPC_CharacterJumping();
        Debug.Log("içerde");
    }

    public void ExitState()
    {
        RPC_CharacterDontJumping();
    }

    public void UpdateState()
    {
        if (!GameService.Instance.playerAction.isJumping && GameService.Instance.playerAction.playerMovement._controller.isGrounded)
        {
            Debug.Log("eee");
            GameService.Instance.playerAction.ChangeState(new IdleState());
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterJumping()
    {
        GameService.Instance.playerAnimationControl.characterAnimator.SetBool("isJumping", true);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterDontJumping()
    {
        GameService.Instance.playerAnimationControl.characterAnimator.SetBool("isJumping", false);
    }
}
