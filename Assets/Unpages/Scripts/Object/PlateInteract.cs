using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateInteract : MonoBehaviour , IInteractable
{
    public void Interact(InteractorData interactorData)
    {
        GameService.Instance.playerAction.interactionObjcet = interactorData.InteractorObject.GetComponent<NetworkObject>();
        GameService.Instance.playerAction.interactionObjcetType = this.GetComponent<PlateItem>().itemType;
    }

    public void UnInteract()
    {
        GameService.Instance.playerAction.interactionObjcet = null;
        GameService.Instance.playerAction.interactionObjcetType = ItemType.Null;
    }
}
