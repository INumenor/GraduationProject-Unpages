using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class Item : MonoBehaviour, IInteractable
{
    public void Interact(InteractorData interactorData)
    {
        //GameService.Instance.playerAction.grabbableObject =this.gameObject;
        GameService.Instance.playerAction.grabbableObject =interactorData.InteractorObject;
    }
}
