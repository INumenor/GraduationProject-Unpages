using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unpages.Network;
using Fusion;

public class PlayerInteraction : NetworkBehaviour
{
    public NetworkObject interactionObject;
    //public NetworkObject testObject;
    private bool _isInActivationDelay;

    public void PlayerGrabAndDropItem(GameObject interactableObject)
    {
        if (_isInActivationDelay) return;
        Debug.Log(interactableObject);
        if (interactableObject != null && interactionObject == null)
        {
            interactableObject.GetComponent<NetworkObject>().ReleaseStateAuthority();

            PlayerGrabItem(GameService.Instance.networkItems.GetNetworkItem(interactableObject.name));
            StartActivationDelay();
            Destroy(interactableObject);
        }
        else 
        {
            PlayerDropItem();
            StartActivationDelay();
        }
    }

    public void PlayerGrabItem(NetworkObject networkObject = null)
    {
        if (interactionObject == null)
        {
            NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(networkObject, new Vector3(transform.position.x,transform.position.y+1.5f,transform.position.z),this.transform.rotation,Object.InputAuthority);
            item.transform.SetParent(transform);
            item.gameObject.GetComponent<Rigidbody>().useGravity = false;
            item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            item.name = networkObject.name;
            interactionObject = item;   
            GameService.Instance.playerAction.isGrabbable = true;
        }
    }

    public void PlayerDropItem()
    {
        if (interactionObject != null)
        {
            Vector3 spawnPosition = transform.position + transform.forward * 1f;
            NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(interactionObject, spawnPosition, this.transform.rotation, Object.StateAuthority);
            item.name = interactionObject.name;
            item.gameObject.GetComponent<Rigidbody>().useGravity = true;
            item.gameObject.GetComponent<Rigidbody>().isKinematic = false ;
            Runner.Despawn(interactionObject);
            interactionObject.transform.parent = null;
            interactionObject = null;
            GameService.Instance.playerAction.isGrabbable = false;
        }
    }
    public async void StartActivationDelay()
    {
        _isInActivationDelay = true;
        await UniTask.WaitForSeconds(.1f, cancellationToken: destroyCancellationToken);
        _isInActivationDelay = false;
    }
}
