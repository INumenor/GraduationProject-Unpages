using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct InteractorData
{
    public GameObject InteractorObject;

   // public Transform AnchorPoint;
}
public class Interactor : MonoBehaviour
{
    [SerializeField] LayerMask interactionMask;

    [SerializeField] InteractorData interactorData = new InteractorData();
    

    private void OnTriggerEnter(Collider other)
    {
        GameObject targetObject = other.gameObject;
        if (targetObject.layer == LayerMask.NameToLayer("ItemInteractable"))
        {
            if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactorData.InteractorObject = other.gameObject;
                interactable.Interact(interactorData);
            }
        }
        else if (targetObject.layer == LayerMask.NameToLayer("MouseInteractable"))
        {
            if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactorData.InteractorObject = other.gameObject;
                interactable.Interact(interactorData);
            }
         
        }
        else if (targetObject.layer == LayerMask.NameToLayer("KitchenInteractable"))
        {
            if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactorData.InteractorObject = other.gameObject;
                interactable.Interact(interactorData);
            }
            //switch (other.tag)
            //{
            //    case "Storage":
            //        other.gameObject.GetComponent<Storage>().InteractStorage();
            //        break;
            //    case "Trash":
            //        other.gameObject.GetComponent<Trash>().InteractTrash();
            //        break;
            //    case "Plate":
            //        other.gameObject.GetComponent<PlateKitchen>().InteractPlateArea();
            //        break;
            //    case "ChoppingBoard":
            //        other.gameObject.GetComponent<Chopping>().InteractChoppingBoard();
            //        break;
            //}
        }
    }
    private void OnTriggerExit(Collider other)
    {
        GameObject targetObject = other.gameObject;
        if (targetObject.layer == LayerMask.NameToLayer("ItemInteractable"))
        {
            if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.UnInteract();
            }
            else
            {
                Debug.LogError("There is no IInteractable on this object: " + other.name);
            }
        }
        else if (targetObject.layer == LayerMask.NameToLayer("KitchenInteractable"))
        {
            if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactorData.InteractorObject = other.gameObject;
                interactable.UnInteract();
            }
        }
    }
}
