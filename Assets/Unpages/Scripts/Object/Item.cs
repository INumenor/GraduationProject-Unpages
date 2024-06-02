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
        //if (GameService.Instance.mouseStateManager.expiredFood.Contains(gameObject.GetComponent<NetworkObject>()))
        //{
        //    GameService.Instance.mouseStateManager.expiredFood.Remove(gameObject.GetComponent<NetworkObject>());
        //    GameService.Instance.mouseStateManager.isCatch = true;
        //}
        //UnityEngine.Debug.Log("Bari Buraya GEl Be");
        Runner.Despawn(Object);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Despawn()
    {
        Despawn();
    }

    //public void RemoveFoodList()
    //{
    //    if (GameService.Instance.mouseStateManager.expiredFood.Contains(gameObject.GetComponent<NetworkObject>()))
    //    {
    //        GameService.Instance.mouseStateManager.expiredFood.Remove(gameObject.GetComponent<NetworkObject>());
    //        GameService.Instance.mouseStateManager.isCatch = true;
    //    }
    //    UnityEngine.Debug.Log("Bari Buraya GEl Be");
    //}

    //[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    //public void RPC_RemoveFoodList()
    //{
    //    RemoveFoodList();
    //}


}
