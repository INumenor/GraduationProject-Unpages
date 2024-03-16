using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unpages.Network;
using Fusion;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject pObject;
    private bool _isInActivationDelay;

    public void PlayerGrabAndDropItem(GameObject cube = null)
    {
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
            NetworkManager.Instance.SessionRunner.Spawn(cube.GetComponent<NetworkObject>,this.transform,this.transform.rotation,);
            Destroy(cube);
            cube.transform.SetParent(this.transform, true);
            pObject = cube;   
            GameService.Instance.playerAction.isGrabbable = true;
        }
    }

    public void PlayerDropItem()
    {
        if (pObject != null)
        {
            pObject.transform.parent = null;
            pObject = null;
            GameService.Instance.playerAction.isGrabbable = false;
        }
    }
    public async void StartActivationDelay()
    {
        _isInActivationDelay = true;
        await UniTask.WaitForSeconds(5f, cancellationToken: destroyCancellationToken);
        _isInActivationDelay = false;
    }
}
