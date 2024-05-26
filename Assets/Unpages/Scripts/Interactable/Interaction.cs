using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] PlayerAction playerAction; 
    private void OnTriggerEnter(Collider other)
    {
        playerAction.TriggerObject(other);
    }
    private void OnTriggerExit(Collider other)
    {
        //playerAction.isTriggered = false;
        playerAction.interactionObjcet = null;
        playerAction.playerInteractionKitchenObject = null;
        
    }
}
