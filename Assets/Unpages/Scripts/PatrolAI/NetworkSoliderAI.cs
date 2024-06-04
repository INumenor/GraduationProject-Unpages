using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class NetworkSoliderAI : MonoBehaviour
{
    public Animator MouseAnimatorController;

    [Networked, OnChangedRender(nameof(AnimationChange))] public NetworkBool isRunning { get; set; } = false;

    public void AnimationChange()
    {
        if (isRunning)
        {         
            MouseAnimatorController.SetBool("isRunning", true);
        }
    }
    //[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    //public void RPC_Despawn(NetworkObject)
    //{
    //    Runner.Despawn(NetworkObject);
    //}
}
