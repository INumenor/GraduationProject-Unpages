using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Unpages.Network;

public class MouseAI : NetworkBehaviour,IInteractable
{
    public NavMeshAgent mouseAgent;
    public NetworkObject grabbleNetworkObject;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item") && !grabbleNetworkObject)
        { 
            if (!other.GetComponent<FoodItem>().isProcessed) GameService.Instance.mouseStateManager.mouseGrabbleObject = GameService.Instance.networkItems.GetNetworkFoodItem(other.GetComponent<FoodItem>().foodType);
            GameService.Instance.mouseStateManager.expiredFood.Remove(other.gameObject.GetComponent<NetworkObject>());
            GameService.Instance.playerAction.RPC_Despawn(other.GetComponent<NetworkObject>());
        }
        if (other.CompareTag("Player"))
        {
            GameService.Instance.mouseStateManager.isCatch = true;
        }

    }

    public void Interact(InteractorData interactorData)
    {
        //if(Runner.IsSharedModeMasterClient)GameService.Instance.mouseStateManager.isCatch = true;
        //else RPC_SetCatch();
    }

    public void UnInteract()
    {
        
    }

    //[Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    //public void RPC_SetCatch()
    //{
    //    GameService.Instance.mouseStateManager.isCatch = true;
    //}
}
