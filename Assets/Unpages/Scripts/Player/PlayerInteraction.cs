using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unpages.Network;
using Fusion;

public class PlayerInteraction : NetworkBehaviour
{
    public NetworkObject pObject;
    public NetworkObject testObject;
    private bool _isInActivationDelay;

    public void PlayerGrabAndDropItem(GameObject cube = null)
    {
        if (_isInActivationDelay) return;
        if (pObject == null)
        {
            PlayerGrabItem(cube);
            StartActivationDelay();
        }
        else 
        {
            PlayerDropItem();
            StartActivationDelay();
        }
    }

    public void PlayerGrabItem(GameObject cube = null)
    {
        if (pObject == null)
        {
            NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(testObject, new Vector3(transform.position.x,transform.position.y+1.5f,transform.position.z),this.transform.rotation,Object.StateAuthority);
            //Runner.Despawn(cube.GetComponent<NetworkObject>());
            item.transform.SetParent(transform);
            item.gameObject.GetComponent<Rigidbody>().useGravity = false;
            pObject = item;   
            GameService.Instance.playerAction.isGrabbable = true;
        }
    }

    public void PlayerDropItem()
    {
        if (pObject != null)
        {
            Runner.Despawn(pObject);
            pObject.transform.parent = null;
            pObject = null;
            Vector3 spawnPosition = transform.position + transform.forward * 1f;
            NetworkManager.Instance.SessionRunner.Spawn(testObject, spawnPosition, this.transform.rotation, Object.StateAuthority);
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
