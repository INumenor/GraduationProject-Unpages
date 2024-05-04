using Fusion;
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

    #region RPC_System
    [Rpc(RpcSources.All, RpcTargets.All)]//------> Change
    public void RPC_Despawn(NetworkObject networkObject)
    {
        NetworkManager.Instance.SessionRunner.Despawn(networkObject);
    }
    #endregion
}
