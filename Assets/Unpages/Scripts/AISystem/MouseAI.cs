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
    public GameObject grabbleCheck;
    //[Networked, OnChangedRender(nameof(MouseGrabCheck))] public NetworkBool IsMouseGrab { get; set; } = false;

    //public void MouseAgentDestination(Vector3 targetPosition)
    //{
    //    mouseAgent.SetDestination(targetPosition);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item") && !grabbleNetworkObject)
        {
            //NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(other.GetComponent<NetworkObject>(), new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), this.transform.rotation , Object.InputAuthority);
            //item.transform.SetParent(grabbleGameObjcetParent);
            //item.gameObject.GetComponent<Rigidbody>().useGravity = false;
            //grabbleNetworkObject = item;
            //grabbleNetworkObject = GameService.Instance.networkItems.GetNetworkFoodItem(other.GetComponent<FoodItem>().foodType);
            //IsMouseGrab = true;
            if (!other.GetComponent<FoodItem>().isProcessed) GameService.Instance.mouseStateManager.mouseGrabbleObject = GameService.Instance.networkItems.GetNetworkFoodItem(other.GetComponent<FoodItem>().foodType);
            //GameService.Instance.mouseStateManager.mouseGrabbleObject = GameService.Instance.networkItems
            GameService.Instance.mouseStateManager.expiredFood.Remove(other.gameObject.GetComponent<NetworkObject>());
            GameService.Instance.playerAction.RPC_Despawn(other.GetComponent<NetworkObject>());

        }
        //else if (other.CompareTag("MouseBase"))
        //{
        //    if (grabbleNetworkObject) 
        //    {
        //        IsMouseGrab = false;
        //        grabbleNetworkObject = null;
        //    }
        //}
    }

    //public void DropItem()
    //{
    //    if (NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient && grabbleNetworkObject)
    //    {
    //        Vector3 spawnPosition = transform.position + -transform.forward * 2f;
    //        NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(grabbleNetworkObject, spawnPosition, this.transform.rotation, Object.StateAuthority);
    //        item.name = grabbleNetworkObject.name;
    //        item.gameObject.GetComponent<Rigidbody>().useGravity = true;
    //        IsMouseGrab=false;
    //        grabbleNetworkObject = null;
    //        mouseAgent.speed = 50;
    //    }
    //}
    //public void MouseGrabCheck()
    //{
    //    if(IsMouseGrab)
    //    {
    //        grabbleCheck.SetActive(true);
    //    }
    //    else
    //    {
    //        grabbleCheck.SetActive(false);
    //    }
    //}

    public void Interact(InteractorData interactorData)
    {
        Debug.Log("deðmesi lazým");
        GameService.Instance.mouseStateManager.isCatch = true;
    }

    public void UnInteract()
    {
        
    }
}
