using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenInteract : MonoBehaviour, IInteractable
{
    public void Interact(InteractorData interactorData)
    {
        GameService.Instance.playerAction.playerInteractionKitchenObject = interactorData.InteractorObject.GetComponent<NetworkObject>();
    }

    public void UnInteract()
    {
        GameService.Instance.playerAction.playerInteractionKitchenObject = null;
    }
}
