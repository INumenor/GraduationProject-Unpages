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
                interactorData.InteractorObject.GetComponent<Outline>().enabled = true;
            }
        }
    }

    public void UnInteract()
    {
        // Interaction.OnTriggerExit may already have cleared playerInteractionKitchenObject.
        NetworkObject kitchenObject = GameService.Instance.playerAction.playerInteractionKitchenObject;
        if (kitchenObject != null && kitchenObject.TryGetComponent(out Outline outline))
        {
            outline.enabled = false;
        }
        else if (TryGetComponent(out Outline ownOutline))
        {
            ownOutline.enabled = false;
        }
        GameService.Instance.playerAction.playerInteractionKitchenObject = null;
    }
}
