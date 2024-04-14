using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Unpages.Network;

public class MouseAI : NetworkBehaviour
{
    public NavMeshAgent mouseAgent;
    public NetworkObject grabbleNetworkObject;
    public GameObject grabbleCheck;
    [Networked, OnChangedRender(nameof(MouseGrabCheck))] public NetworkBool IsMouseGrab { get; set; } = false;

    public void MouseAgentDestination(Vector3 targetPosition)
    {
        mouseAgent.SetDestination(targetPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item") && !grabbleNetworkObject)
        {
            //NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(other.GetComponent<NetworkObject>(), new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), this.transform.rotation , Object.InputAuthority);
            //item.transform.SetParent(grabbleGameObjcetParent);
            //item.gameObject.GetComponent<Rigidbody>().useGravity = false;
            //grabbleNetworkObject = item;
            grabbleNetworkObject = GameService.Instance.networkItems.GetNetworkItem(other.GetComponent<Item>().foodType);
            IsMouseGrab = true;
            GameService.Instance.playerAction.RPC_Trigger(other.GetComponent<NetworkObject>());
            GameService.Instance.aiManagerSystem.ReturnBase();
        }
        else if (other.CompareTag("MouseBase"))
        {
            if (grabbleNetworkObject) 
            {
                IsMouseGrab = false;
                grabbleNetworkObject = null;
            }
            mouseAgent.speed = 5;
        }
    }

    public void DropItem()
    {
        if (NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient && grabbleNetworkObject)
        {
            Vector3 spawnPosition = transform.position + -transform.forward * 2f;
            NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(grabbleNetworkObject, spawnPosition, this.transform.rotation, Object.StateAuthority);
            item.name = grabbleNetworkObject.name;
            item.gameObject.GetComponent<Rigidbody>().useGravity = true;
            IsMouseGrab=false;
            grabbleNetworkObject = null;
            mouseAgent.speed = 50;
        }
    }
    public void MouseGrabCheck()
    {
        if(IsMouseGrab)
        {
            grabbleCheck.SetActive(true);
        }
        else
        {
            grabbleCheck.SetActive(false);
        }
    }
}
