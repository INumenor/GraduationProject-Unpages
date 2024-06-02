using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenInteract : MonoBehaviour, IInteractable
{
    public void Interact(InteractorData interactorData)
    {
        if(GameService.Instance.playerAction.playerInteractionKitchenObject == null)
        {
            GameService.Instance.playerAction.playerInteractionKitchenObject = interactorData.InteractorObject.GetComponent<NetworkObject>();
            if (interactorData.InteractorObject.GetComponent<Outline>() != null)
            {
                Debug.Log("kitchen outline " + interactorData.InteractorObject.name);
                interactorData.InteractorObject.GetComponent<Outline>().enabled = true;
            }
        } 
    }

    public void UnInteract()
    {
        if (GameService.Instance.playerAction.playerInteractionKitchenObject.GetComponent<Outline>() != null)
        {
            GameService.Instance.playerAction.playerInteractionKitchenObject.GetComponent<Outline>().enabled = false;
        }
        GameService.Instance.playerAction.playerInteractionKitchenObject = null;
    }
}
