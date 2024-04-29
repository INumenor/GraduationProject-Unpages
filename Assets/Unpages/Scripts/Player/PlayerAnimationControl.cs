using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : NetworkBehaviour
{
    public Animator characterAnimator;

    private void Start()
    {
        if (HasStateAuthority == false) return;

        GameService.Instance.playerAnimationControl = this;
    }

    [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
    public void RPC_CharacterIdle()
    {
        characterAnimator.SetBool("isRunning", false);
        characterAnimator.SetBool("isGrabbing", false);
        characterAnimator.SetBool("isJumping", false);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterGrabbing()
    {
        characterAnimator.SetBool("isGrabbing", true);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterRunning() 
    {
        characterAnimator.SetBool("isRunning", true);
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterJumping()
    {
        characterAnimator.SetBool("isJumping", true);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterDontJumping()
    {
        characterAnimator.SetBool("isJumping", false);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterKichenAction()
    {
        characterAnimator.SetBool("isKitchenAction", true);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterDontKichenAction()
    {
        characterAnimator.SetBool("isKitchenAction", false);
    }
}
