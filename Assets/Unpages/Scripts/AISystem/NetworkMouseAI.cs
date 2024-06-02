using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class NetworkMouseAI : NetworkBehaviour
{
    public Animator MouseAnimatorController;

    [Networked,OnChangedRender(nameof(AnimationChange))] public NetworkBool isIdle { get; set; } = false;
    [Networked, OnChangedRender(nameof(AnimationChange))] public NetworkBool isRunning { get; set; } = false;
    [Networked, OnChangedRender(nameof(AnimationChange))] public NetworkBool isJumping { get; set; } = false;
    public void DropItem(NetworkObject grabbleNetworkObject)
    {
        if (NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient && grabbleNetworkObject)
        {
            Vector3 spawnPosition = transform.position + transform.forward * -1f;
            NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(grabbleNetworkObject, spawnPosition, this.transform.rotation, Object.StateAuthority);
            item.name = grabbleNetworkObject.name;
            item.gameObject.GetComponent<Rigidbody>().useGravity = true;
            grabbleNetworkObject = null;
            item.GetComponent<Item>().AddComponentInteract();
           
        }
    }

    public void AnimationChange()
    {
        if (isIdle)
        {
            MouseAnimatorController.SetBool("isIdle", true);
        }
        else if (isRunning)
        {
            MouseAnimatorController.SetBool("isRunning", true);
        }
        else if (isJumping)
        {
            MouseAnimatorController.SetBool("isJumping", false);
        }
        else
        {
            MouseAnimatorController.SetBool("isIdle", true);
            MouseAnimatorController.SetBool("isRunning", false);
            MouseAnimatorController.SetBool("isJumping", false);
        }
    }
    //[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    //public void RPC_Despawn(NetworkObject)
    //{
    //    Runner.Despawn(NetworkObject);
    //}
}
