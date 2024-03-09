using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] PlayerAction playerAction; 
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Test");
       // playerAction.isTriggered = true;
        playerAction.grabbableObject = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        //playerAction.isTriggered = false;
        playerAction.grabbableObject = null;
    }
}
