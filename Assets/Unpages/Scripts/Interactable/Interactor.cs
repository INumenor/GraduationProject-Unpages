using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InteractorData
{
    public GameObject InteractorObject;

    public Transform AnchorPoint;
}
public class Interactor : MonoBehaviour
{
    [SerializeField] LayerMask interactionMask;

    [SerializeField] InteractorData interactorData = new InteractorData();


    private void OnTriggerEnter(Collider other)
    {
        if (interactionMask == (interactionMask | (1 << other.gameObject.layer)))
        {
            if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact(interactorData);
            }
            else
            {
                Debug.LogError("There is no IInteractable on this object: " + other.name);
            }
        }
    }
}
