using Fusion;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class SpawnObject : NetworkBehaviour
{
    private void Start()
    {
        GameService.Instance.spawnObject = this;
    }
    #region PlayerGroundGrabandDropItem
    public void PlayerGrabItem(NetworkObject networkObject, Transform anchorPoint ,bool isIntetactor,NetworkObject interactionObjcet)
    {
        //if (interactionObject == null)
        //{
        NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(networkObject, anchorPoint.position , this.transform.rotation,Object.StateAuthority);
        item.transform.SetParent(anchorPoint);
        item.gameObject.GetComponent<Rigidbody>().useGravity = false;
        item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        item.name = networkObject.name;
        //interactionObject = item;
        GameService.Instance.playerAction.isGrabbable = true;
        GameService.Instance.playerAction.keepObject = item;
        GameService.Instance.playerAction.interactionObjcet = null;
        GameService.Instance.playerAction.interactionObjcetType = ItemType.Null;

        if (isIntetactor) item.GetComponent<Item>().AddComponentInteract();

        if (interactionObjcet)
        {
            interactionObjcet.ReleaseStateAuthority();
            RPC_Despawn(interactionObjcet);
        }
        //}
    }

    public void PlayerDropItem(NetworkObject interactionObjcet , bool isInteractor)
    {
        //if (interactionObject != null)
        //{
        Vector3 playerPosition = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
        Vector3 spawnPosition = playerPosition + transform.forward * 1f;
        NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(interactionObjcet, spawnPosition, this.transform.rotation, Object.StateAuthority);
        item.name = interactionObjcet.name;
        item.gameObject.GetComponent<Rigidbody>().useGravity = true;
        item.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        RPC_Despawn(interactionObjcet);
        //Runner.Despawn(interactionObject); //--> Change
        GameService.Instance.playerAction.isGrabbable = false;
        GameService.Instance.playerAction.keepObject = null;
        if (isInteractor) item.GetComponent<Item>().AddComponentInteract();

    }
    #endregion
    #region CupboardGrabandDropItem
    public void PlayerGrabCupboardItem(NetworkObject networkObject, Transform anchorPoint, bool isIntetactor/* NetworkObject interactionObjcet*/)
    {
        //if (interactionObject == null)
        //{
        NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(networkObject, anchorPoint.position, this.transform.rotation, Object.StateAuthority);
        item.transform.SetParent(anchorPoint);
        item.gameObject.GetComponent<Rigidbody>().useGravity = false;
        item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        item.name = networkObject.name;
        //interactionObject = item;
        GameService.Instance.playerAction.isGrabbable = true;
        GameService.Instance.playerAction.keepObject = item;

        if (isIntetactor) item.GetComponent<Item>().AddComponentInteract();
        networkObject.ReleaseStateAuthority();
        if (networkObject) RPC_Despawn(networkObject);

        //}
    }

    public NetworkObject PlayerDropCupboardItem(NetworkObject interactionObjcet,Transform anchorPoint ,bool isInteractor)
    {
        //if (interactionObject != null)
        //{
        NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(interactionObjcet, anchorPoint.position, this.transform.rotation, Object.StateAuthority);
        item.name = interactionObjcet.name;
        item.transform.SetParent(anchorPoint);
        item.gameObject.GetComponent<Rigidbody>().useGravity = false;
        item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        RPC_Despawn(interactionObjcet);
        //Runner.Despawn(interactionObject); //--> Change
        GameService.Instance.playerAction.isGrabbable = false;
        GameService.Instance.playerAction.keepObject = null;
        if (isInteractor) item.GetComponent<Item>().AddComponentInteract();
        return item;
    }
    #endregion

    #region Destroy of Object Drop into Storage

    public void DestroyDropItemStorage(NetworkObject interactionObjcet)
    {
        RPC_Despawn(interactionObjcet);
        GameService.Instance.playerAction.isGrabbable = false;
        GameService.Instance.playerAction.keepObject = null;
    }

    #endregion

    #region Spawn of Object Drop into ChoppingBoard

    public NetworkObject SpawnChoppedItem(NetworkObject defautObjcet,NetworkObject choppedObject,Transform anchorPoint,bool isInteractor)
    {
        NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(choppedObject, anchorPoint.position, this.transform.rotation, Object.StateAuthority);
        item.name = choppedObject.name;
        item.transform.SetParent(anchorPoint);
        item.gameObject.GetComponent<Rigidbody>().useGravity = false;
        item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        RPC_Despawn(defautObjcet);
        GameService.Instance.playerAction.isGrabbable = false;
        GameService.Instance.playerAction.keepObject = null;
        if (isInteractor) item.GetComponent<Item>().AddComponentInteract();
        return item;
    }

    #endregion

    #region Spawn of Food Recipe into Plate

    public NetworkObject SpawnFoodRecipe(NetworkObject interactionObjcet, Transform anchorPoint , NetworkObject keepObject)
    {
        NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(interactionObjcet, anchorPoint.position, this.transform.rotation, Object.StateAuthority);
        item.name = interactionObjcet.name;
        item.transform.SetParent(anchorPoint);
        RPC_Despawn(GameService.Instance.playerAction.keepObject);
        GameService.Instance.playerAction.isGrabbable = false;
        GameService.Instance.playerAction.keepObject = null;
        return item;
    }

    public NetworkObject ReSpawnFoodRecipe(NetworkObject interactionObjcet, Transform anchorPoint)
    {
        NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(interactionObjcet, anchorPoint.position, this.transform.rotation, Object.StateAuthority);
        item.name = interactionObjcet.name;
        item.transform.SetParent(anchorPoint);
        return item;
    }

    public void PlayerPlateGrab(NetworkObject networkObject, Transform anchorPoint, bool isIntetactor, NetworkObject interactionObjcet)
    {
        
        NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(networkObject, anchorPoint.position, this.transform.rotation, Object.StateAuthority);
        item.transform.SetParent(anchorPoint);
        item.gameObject.GetComponent<Rigidbody>().useGravity = false;
        item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        item.name = networkObject.name;
        //interactionObject = item;
        GameService.Instance.playerAction.isGrabbable = true;
        GameService.Instance.playerAction.keepObject = item;
        GameService.Instance.playerAction.interactionObjcet = null;
        GameService.Instance.playerAction.interactionObjcetType = ItemType.Null;

        if (interactionObjcet.GetComponent<PlateItem>().networkFoodRecipe)
        {
            PlateItem plateItem = interactionObjcet.GetComponent<PlateItem>();
            item.GetComponent<PlateItem>().GrabItem(plateItem.networkFoodRecipe);
        }

        if (isIntetactor) item.GetComponent<Item>().AddComponentInteract();

        if (interactionObjcet)
        {
            interactionObjcet.ReleaseStateAuthority();
            RPC_Despawn(interactionObjcet);
        }
        //}
    }

    #endregion

    #region Plate Drop

    public void PlateDrop(NetworkObject plateObject)
    {

    }

    #endregion

    #region RPC_System
    [Rpc(RpcSources.All, RpcTargets.All)]//------> Change
    public void RPC_Despawn(NetworkObject networkObject)
    {
        NetworkManager.Instance.SessionRunner.Despawn(networkObject);
    }
    #endregion
}
