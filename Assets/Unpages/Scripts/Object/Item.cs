using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Unpages.Network;

public enum ItemType {Tomato,Bread,Chess,Lettuce,Null}
public class Item : MonoBehaviour, IInteractable
{
    public ItemType foodType;
    public void Interact(InteractorData interactorData)
    {
        //GameService.Instance.playerAction.grabbableObject =this.gameObject;
        GameService.Instance.playerAction.grabbableObject = interactorData.InteractorObject;
        GameService.Instance.playerAction.grabbableObjectType = foodType;
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Despawn()
    {
        //Destroy(gameObject);
        NetworkManager.Instance.SessionRunner.Despawn(GetComponent<NetworkObject>());
    }
}
