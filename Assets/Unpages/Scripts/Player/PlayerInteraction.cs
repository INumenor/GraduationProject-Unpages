using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using Unpages.Network;

public class PlayerInteraction : NetworkBehaviour
{
    private bool _isInActivationDelay;

    //public void PlayerGrabAndDropItem(ItemType itemType, GameObject interactableObject)
    //{
    //    if (_isInActivationDelay) return;
    //    Debug.Log(interactableObject);
    //    if (interactableObject != null && interactionObject == null)
    //    {
    //        interactableObject.GetComponent<NetworkObject>().ReleaseStateAuthority();
    //        if (!interactableObject.GetComponent<FoodItem>().isSliced)
    //        {
    //            PlayerGrabItem(GameService.Instance.networkItems.GetNetworkFoodItem(itemType));
    //        }
    //        else
    //        {
    //            PlayerGrabItem(GameService.Instance.networkItems.GetNetworkItemSlice(itemType));
    //        }
    //        StartActivationDelay();
    //        //interactableObject.GetComponent<Item>().RPC_Despawn();
    //        RPC_Despawn(interactableObject.GetComponent<NetworkObject>());
    //    }
    //    else
    //    {
    //        PlayerDropItem();
    //        StartActivationDelay();
    //    }
    //}

    public void PlayerGrabObject(ItemType itemType, NetworkObject interactionObjcet)
    {
        if (_isInActivationDelay) return;

        NetworkObject networkObject=null;
        if (interactionObjcet)
        {
            switch (itemType)
            {
                case ItemType.Food:
                    //Deðiþicek
                    if(!interactionObjcet.GetComponent<FoodItem>().isSliced) networkObject = GameService.Instance.networkItems.GetNetworkFoodItem(interactionObjcet.GetComponent<FoodItem>().foodType);
                    else if(interactionObjcet.GetComponent<FoodItem>().isSliced) networkObject = GameService.Instance.networkItems.GetNetworkItemSlice(interactionObjcet.GetComponent<FoodItem>().foodType);
                    GameService.Instance.spawnObject.PlayerGrabItem(networkObject,GameService.Instance.playerAction.playerAnchorPoint,false,interactionObjcet);
                    break;
                case ItemType.Plate:
                    networkObject = GameService.Instance.networkItems.GetNetworkItemPlate(ItemType.Plate);
                    GameService.Instance.spawnObject.PlayerPlateGrab(networkObject, GameService.Instance.playerAction.playerAnchorPoint,false,interactionObjcet);
                    break;
                case ItemType.Other:
                    break;
                default:
                    break;
            }
        }
        StartActivationDelay();
    }

    public void PlayerDrobObject(NetworkObject keepObject, ItemType itemType , NetworkObject interactionObjcet)
    {
        if (_isInActivationDelay) return;

        NetworkObject networkObject;
        if (keepObject)
        {
            if(itemType == ItemType.Plate)
            {
                interactionObjcet.GetComponent<PlateItem>().DropItem(keepObject);
            }
            else
            {
                GameService.Instance.spawnObject.PlayerDropItem(keepObject, true);
            }
            
        }
        StartActivationDelay();
    }

    //public void PlayerStorageGrab(NetworkObject interactableObject)
    //{
    //    PlayerGrabItem(interactableObject);
    //    StartActivationDelay();
    //}
    //public void PlayerChoppingGrab(NetworkObject interactableObject)
    //{
    //    PlayerGrabItem(interactableObject);
    //    StartActivationDelay();
    //}
    //public void PlayerPlateGrab(NetworkObject interactableObject)
    //{
    //    PlayerGrabItem(interactableObject);
    //    StartActivationDelay();
    //}


    //public void PlayerGrabItem(NetworkObject networkObject = null)
    //{
    //    if (interactionObject == null)
    //    {   
    //        NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(networkObject, itemAnchorPoint.position, this.transform.rotation, Object.InputAuthority);
    //        item.transform.SetParent(transform);
    //        item.gameObject.GetComponent<Rigidbody>().useGravity = false;
    //        item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    //        item.name = networkObject.name;
    //        interactionObject = item;
    //        GameService.Instance.playerAction.isGrabbable = true;
    //        GameService.Instance.playerAction.keepObject = item.gameObject;
    //    }
    //}

    //public void PlayerDropItem()
    //{
    //    if (interactionObject != null)
    //    {
    //        Vector3 playerPosition = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
    //        Vector3 spawnPosition = playerPosition + transform.forward * 1f;
    //        NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(interactionObject, spawnPosition, this.transform.rotation, Object.StateAuthority);
    //        item.name = interactionObject.name;
    //        item.gameObject.GetComponent<Rigidbody>().useGravity = true;
    //        item.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    //        RPC_Despawn(interactionObject);
    //        //Runner.Despawn(interactionObject);
    //        interactionObject.transform.parent = null;
    //        interactionObject = null;
    //        GameService.Instance.playerAction.isGrabbable = false;
    //        GameService.Instance.playerAction.keepObject = null;
    //    }
    //}
    public async void StartActivationDelay()
    {
        _isInActivationDelay = true;
        await UniTask.WaitForSeconds(.1f, cancellationToken: destroyCancellationToken);
        _isInActivationDelay = false;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]//------> Change
    public void RPC_Despawn(NetworkObject networkObject)
    {
        NetworkManager.Instance.SessionRunner.Despawn(networkObject);
    }
}
