using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : KitchenObject
{
    //public void TrashObject(GameObject gameObject)
    // {
    //     GameService.Instance.playerAction.RPC_Despawn(gameObject.GetComponent<NetworkObject>());
    //     GameService.Instance.playerAction.isGrabbable = false;
    //     GameService.Instance.playerAction.keepObject = null;
    // }
    public override void DropItem(NetworkObject networkObject)
    {
        GameService.Instance.spawnObject.PlayerDropTrash(networkObject);
    }
    public void InteractTrash()
    {

        //GameService.Instance.playerAction.playerInteractionKitchenObject = this.gameObject;

    }
}
