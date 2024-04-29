using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
   public void TrashObject(GameObject gameObject)
    {
        GameService.Instance.playerAction.RPC_Trigger(gameObject.GetComponent<NetworkObject>());
        GameService.Instance.playerAction.isGrabbable = false;
        GameService.Instance.playerAction.keepObject = null;
    }
    public void InteractTrash()
    {

        GameService.Instance.playerAction.playerInteractionKitchenObject = this.gameObject;

    }
}
