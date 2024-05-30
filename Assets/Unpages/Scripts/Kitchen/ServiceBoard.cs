using DG.Tweening;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceBoard : KitchenObject
{
    public AudioSource audioSource;
    public override void DropItem(NetworkObject networkObject)
    {
        if (onTheCupboardObject == null && networkObject.GetComponent<Item>().itemType == ItemType.Plate)
        {
           onTheCupboardObject = GameService.Instance.spawnObject.PlayerDropCupboardItem(networkObject, anchorPoints, false);
        }
    }


    //public override void GrabItem(NetworkObject networkObject, Transform anchorPoint)
    //{
    //    if (onTheCupboardObject != null)
    //    {
    //        GameService.Instance.spawnObject.PlayerGrabCupboardItem(onTheCupboardObject, anchorPoint, false);
    //        onTheCupboardObject = null;
    //    }
    //    else if (onTheCupboardObject == null)
    //    {
    //        NetworkObject foodNetworkObject = GameService.Instance.networkItems.GetNetworkFoodItem(foodType);
    //        GameService.Instance.spawnObject.PlayerGrabItem(foodNetworkObject, GameService.Instance.playerAction.playerAnchorPoint, false, null);
    //    }

    //}

    private void Update()
    {
        bool isThere = false;
        if (onTheCupboardObject && !isThere)
        {
            if (onTheCupboardObject.GetComponent<Item>().itemType == ItemType.Plate /*&& onTheCupboardObject.GetComponent<PlateItem>().networkFoodRecipe*/)
            {
                isThere = GameService.Instance.playerTask.ChecktaskRecipes(onTheCupboardObject.GetComponent<PlateItem>().networkFoodRecipe);
                audioSource.Play();
                GameService.Instance.spawnObject.CheckServisSystem(onTheCupboardObject);
                if (isThere) Debug.Log("aaaaattttttt");
                else Debug.Log("aaaaaaalllllllllll");
                isThere = false;
            }
        }
    }
}
