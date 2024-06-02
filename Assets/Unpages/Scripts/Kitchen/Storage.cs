using Cysharp.Threading.Tasks;
using DG.Tweening;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class Storage : KitchenObject
{
    public FoodType foodType;
    public int storageFoodCount=0;

    public override void DropItem(NetworkObject networkObject)
    {
        if(networkObject.GetComponent<Item>().itemType == ItemType.Food)
        {
           if(networkObject.GetComponent<FoodItem>().foodType == foodType && networkObject.GetComponent<FoodItem>().isPlateHolder==false)
            {
                storageFoodCount += 1;
                GameService.Instance.spawnObject.DestroyDropItemStorage(networkObject);
            }
            else onTheCupboardObject = GameService.Instance.spawnObject.PlayerDropCupboardItem(networkObject, anchorPoints, false);
        }
        else if (onTheCupboardObject == null)
            onTheCupboardObject = GameService.Instance.spawnObject.PlayerDropCupboardItem(networkObject, anchorPoints, false);
    }

    public override void GrabItem(NetworkObject networkObject, Transform anchorPoint)
    {
        if (onTheCupboardObject != null)
        {
            GameService.Instance.spawnObject.PlayerGrabCupboardItem(onTheCupboardObject, anchorPoint, false);
            onTheCupboardObject = null;
        }
        else if(onTheCupboardObject == null && storageFoodCount > 0)
        {
            NetworkObject foodNetworkObject = GameService.Instance.networkItems.GetNetworkFoodItem(foodType);
            GameService.Instance.spawnObject.PlayerGrabItem(foodNetworkObject,GameService.Instance.playerAction.playerAnchorPoint,false,null);
            storageFoodCount--;
        }

    }

    //public void DropItemStorage(GameObject gameObject)
    //{
    //    FoodItem item = gameObject.GetComponent<FoodItem>();
    //    //if (item.foodType == itemType && storageFoodCount<5 && !item.isSliced)
    //    //{
    //    //    storageFoodCount++;
    //    //    GameService.Instance.playerAction.RPC_Trigger(gameObject.GetComponent<NetworkObject>());
    //    //    GameService.Instance.playerAction.isGrabbable = false;
    //    //    GameService.Instance.playerAction.keepObject = null;
    //    //}
    //}
    //public void GetItemStorage()
    //{
    //    if (storageFoodCount > 0)
    //    {
    //        //GameService.Instance.playerAction.playerInteraction.PlayerStorageGrab(GameService.Instance.networkItems.GetNetworkFoodItem(itemType));
    //        storageFoodCount--;
    //    }     
    //}

    //public void InteractStorage()
    //{

    // //   GameService.Instance.playerAction.playerInteractionKitchenObject = this.gameObject;

    //}
   
}
