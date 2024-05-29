using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public enum ItemType { Food , Other ,Plate , Attributes , Null}
public class Item : NetworkBehaviour
{
    public ItemType itemType;
    public override void Spawned()
    {
        RPCTrigger();
    }

    [Networked, OnChangedRender(nameof(RPCTrigger))] public NetworkBool isInteractable { get; set; } = false;

    public void AddComponentInteract()
    {
        isInteractable = true;
    }

    public virtual void RPCTrigger()
    {
        
    }

    public void Despawn()
    {
       Runner.Despawn(Object);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Despawn()
    {
        Despawn();
    }

}
