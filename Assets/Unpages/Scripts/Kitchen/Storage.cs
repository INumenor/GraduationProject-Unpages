using Cysharp.Threading.Tasks;
using DG.Tweening;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class Storage : MonoBehaviour
{
    PlayerInteraction playerInteraction;
    public ItemType itemType;
    public int storageFoodCount=0;


    public void DropItemStorage(GameObject gameObject)
    {
        Item item = gameObject.GetComponent<Item>();
        if (item.foodType == itemType && storageFoodCount<5 && !item.isSliced)
        {
            storageFoodCount++;
            GameService.Instance.playerAction.RPC_Trigger(gameObject.GetComponent<NetworkObject>());
            GameService.Instance.playerAction.isGrabbable = false;
            GameService.Instance.playerAction.keepObject = null;
        }
    }
    public void GetItemStorage()
    {
        if (storageFoodCount > 0)
        {
            GameService.Instance.playerAction.playerInteraction.PlayerStorageGrab(GameService.Instance.networkItems.GetNetworkItem(itemType));
            storageFoodCount--;
        }     
    }

    public void InteractStorage()
    {

        GameService.Instance.playerAction.playerInteractionKitchenObject = this.gameObject;

    }
   
}
