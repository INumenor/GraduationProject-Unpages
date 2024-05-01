using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenState : IState
{
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
        if (!GameService.Instance.playerAction.isKitchenAction)
        {
            GameService.Instance.playerAction.ChangeState(new IdleState());
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterKichenAction()
    {
        GameService.Instance.playerAnimationControl.characterAnimator.SetBool("isKitchenAction", true);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterDontKichenAction()
    {
        GameService.Instance.playerAnimationControl.characterAnimator.SetBool("isKitchenAction", false);
    }
}
