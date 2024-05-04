using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState
{
    public StateManager stateManager { get; set; }

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
        if (!stateManager.isJumping && GameService.Instance.playerAction.playerMovement._controller.isGrounded)
        {
            stateManager.ChangeState(new IdleState());
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterJumping()
    {
        stateManager.characterAnimator.SetBool("isJumping", true);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterDontJumping()
    {
        stateManager.characterAnimator.SetBool("isJumping", false);
    }
}
