using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unpages.Network;
using Fusion;

public class PlayerInteraction : NetworkBehaviour
{
    public NetworkObject pObject;
    //public NetworkObject testObject;
    private bool _isInActivationDelay;

    public void PlayerGrabAndDropItem(GameObject gameobject = null)
    {
        if (_isInActivationDelay) return;
        if (pObject == null)
        {           
            PlayerGrabItem(GameService.Instance.networkItems.GetNetworkItem(gameobject.name));
            StartActivationDelay();
            Destroy(gameobject);
        }
        else 
        {
            PlayerDropItem();
            StartActivationDelay();
        }
    }

    public void PlayerGrabItem(NetworkObject networkObject = null)
    {
        if (pObject == null)
        {
            NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(networkObject, new Vector3(transform.position.x,transform.position.y+1.5f,transform.position.z),this.transform.rotation,Object.StateAuthority);
            item.transform.SetParent(transform);
            item.gameObject.GetComponent<Rigidbody>().useGravity = false;
            item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            item.name = networkObject.name;
            pObject = item;   
            GameService.Instance.playerAction.isGrabbable = true;
        }
    }

    public void PlayerDropItem()
    {
        if (pObject != null)
        {
            Vector3 spawnPosition = transform.position + transform.forward * 1f;
            NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(pObject, spawnPosition, this.transform.rotation, Object.StateAuthority);
            item.name = pObject.name;
            item.gameObject.GetComponent<Rigidbody>().useGravity = true;
            item.gameObject.GetComponent<Rigidbody>().isKinematic = false ;
            Runner.Despawn(pObject);
            pObject.transform.parent = null;
            pObject = null;
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
