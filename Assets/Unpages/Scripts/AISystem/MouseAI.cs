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
        // The mouse AI only runs on the shared-mode master client (mouseStateManager is null elsewhere).
        if (GameService.Instance == null || GameService.Instance.mouseStateManager == null) return;

        // Plates are tagged "Item" too, but only food can be stolen.
        if (other.CompareTag("Item") && !grabbleNetworkObject && other.TryGetComponent(out FoodItem foodItem))
        {
            if (!foodItem.isProcessed) GameService.Instance.mouseStateManager.mouseGrabbleObject = GameService.Instance.networkItems.GetNetworkFoodItem(foodItem.foodType);
            //GameService.Instance.mouseStateManager.expiredFood.Remove(other.gameObject.GetComponent<NetworkObject>());
            other.gameObject.GetComponent<Item>().RPC_Despawn();
            //GameService.Instance.playerAction.RPC_Despawn(other.GetComponent<NetworkObject>());
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
