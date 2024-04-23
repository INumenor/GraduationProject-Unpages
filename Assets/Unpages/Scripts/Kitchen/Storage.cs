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
     int storageFoodCount=0;


    public void DropItemStorage(GameObject gameObject)
    {
        if (gameObject.GetComponent<Item>().foodType == itemType && storageFoodCount<5)
        {
            storageFoodCount++;
            GameService.Instance.playerAction.RPC_Trigger(gameObject.GetComponent<NetworkObject>());
            GameService.Instance.playerAction.isGrabbable = false;
            GameService.Instance.playerAction.keepObject = null;

        }
    }
    public void GetItemStorage()
    {
        Debug.Log("aaaa");
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
