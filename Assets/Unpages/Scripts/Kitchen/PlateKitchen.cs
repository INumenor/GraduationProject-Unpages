using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class PlateKitchen : KitchenObject
{
    //public NetworkObject plateNetworkObject;

    public int plateLimit;
    
    [SerializeField] List<NetworkObject> platesObjects = new List<NetworkObject>();
    // Start is called before the first frame update

    public override void DropItem(NetworkObject networkObject)
    {
        if(networkObject.GetComponent<Item>().itemType == ItemType.Plate)
        {
            GameService.Instance.spawnObject.DestroyDropItemKitchenPlate(networkObject);
            plateLimit++;
        }
    }

    public override void GrabItem(NetworkObject networkObject, Transform anchorPoint)
    {
        if (plateLimit > 0)
        {
            NetworkObject plateObjcet = GameService.Instance.networkItems.GetNetworkItemPlate(ItemType.Plate);
            GameService.Instance.spawnObject.PlayerGrabItem(plateObjcet,anchorPoint,false,null);
            plateLimit--;
        }

    }

    //public void GrabAndDropPlate(GameObject gameObject)//tabak yerine gidiğ bastığında tabak alması ancak tabak olan bir yere bastığında eli boşsa önündeki tabağı alması lazım ve eğer elinde varsa da tabağı koyması gerekiyor.
    //{
    //    //Debug.Log("aaaaaa");
    //    //if (!gameObject && plateLimit > 0)
    //    //{
    //    //    GetPlate();
    //    //}
    //    //else if(gameObject && gameObject.GetComponent<FoodItem>().foodType == ItemType.Plate)
    //    //{
    //    //    Debug.Log("tabağı bırak");
    //    //    DropPlate(gameObject.GetComponent<NetworkObject>());
    //    //}
    //}

    //public void DropPlate(NetworkObject networkObject)
    //{
    //    DespawnPlate(networkObject);
    //    GameService.Instance.playerAction.isGrabbable = false;
    //    GameService.Instance.playerAction.keepObject = null;
    //    plateLimit++;
    //    Debug.Log("tabak yok olup oluştu");
    //}
    //public void GetPlate()
    //{
    //    //GameService.Instance.playerAction.playerInteraction.PlayerPlateGrab(plateNetworkObject);
    //    plateLimit --;
    //}
    //public void DespawnPlate(NetworkObject networkObject)
    //{
    //    GameService.Instance.playerAction.RPC_Despawn(networkObject);
    //}
    //public void InteractPlateArea()
    //{
    //    //GameService.Instance.playerAction.playerInteractionKitchenObject = this.gameObject;
    //}
}
