using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
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
        if(GameService.Instance.playerAction.isRunning)
        {
            GameService.Instance.playerAction.ChangeState(new RunState());
        }
        else if (GameService.Instance.playerAction.isGrabbing)
        {
            GameService.Instance.playerAction.ChangeState(new GrabState());
        }
        else if (GameService.Instance.playerAction.isJumping && !GameService.Instance.playerAction.isGrabbing)
        {
            GameService.Instance.playerAction.ChangeState(new JumpState());
        }
        else if (GameService.Instance.playerAction.isKitchenAction)
        {
            GameService.Instance.playerAction.ChangeState(new KitchenState());
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_CharacterIdle()
    {
        GameService.Instance.playerAnimationControl.characterAnimator.SetBool("isRunning", false);
        GameService.Instance.playerAnimationControl.characterAnimator.SetBool("isGrabbing", false);
        GameService.Instance.playerAnimationControl.characterAnimator.SetBool("isJumping", false);
    }
}
