using Fusion;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class PlateItem : Item
{
    //tabak yerinden tabak al�nabilecek. Tabak bo� alanlara konulabilecek. Taba��n i�erisine do�ranm�� sebzeler konulabilecek di�erleri konulamayacak.taba��n oldu�u yerdeki bir yere yemekler konulabilecek ve taba��n i�erisinde spawn edilecek ya da haz�r spawn edilmi� halleri a��lacak.

    //public NetworkObject platePrefab;

    public PlateInteract plateInteract;

    public Transform anchorPoint;

    public NetworkObject networkFoodRecipe = null;

    public List<FoodType> foodTypes = new List<FoodType>();

    public override void RPCTrigger()
    {
        if(isInteractable && !plateInteract)
        {
            plateInteract = gameObject.AddComponent<PlateInteract>();
        } 
        
    }

    public void DropItem(NetworkObject keepObject)
    {
        if (ItemType.Food == keepObject.GetComponent<Item>().itemType && keepObject.GetComponent<FoodItem>().isPlateHolder)
        {
            bool isThere = false;
            foreach (FoodType foodType in foodTypes)
            {
                if(foodType == keepObject.GetComponent<FoodItem>().foodType)
                {
                    isThere = true;
                }
            }
            if (!isThere)
            {
                if(networkFoodRecipe != null)GameService.Instance.spawnObject.Despawn(networkFoodRecipe);
                foodTypes.Add(keepObject.GetComponent<FoodItem>().foodType);
                networkFoodRecipe = GameService.Instance.spawnObject.SpawnFoodRecipe(GameService.Instance.networkItems.GetNetworkFoodRecipes(foodTypes),anchorPoint,keepObject);
                RPC_SetParent(networkFoodRecipe);
            }
        } 
    }

    public void GrabItem(NetworkObject networkObject)
    {
        networkFoodRecipe = GameService.Instance.spawnObject.ReSpawnFoodRecipe(networkObject,anchorPoint);
        RPC_SetParent(networkFoodRecipe);
    }


    ////-----Bu kısımda Recipe olayı RPC yapılacak ......
    ///
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SetParent(NetworkObject networkObject)
    {
        networkObject.transform.SetParent(anchorPoint);
        networkObject.gameObject.transform.localScale = new Vector3(1,1,1);
    }

    //[Button]
    //public void SetFoodOnPlate(GameObject gameObject)
    //{
    //    for (int i = 0; i < anchorPointsPlate.Count; i++)
    //    {
    //        if (anchorPointsPlate[i].transform.childCount == 0)
    //        {
    //            plateSpawn(gameObject, GameService.Instance.networkItems.GetNetworkItemPlate(gameObject.GetComponent<FoodItem>().foodType));
    //        }
    //    }
    //}

    //public void plateSpawn(GameObject gameObject, NetworkObject networkObject)
    //{
    //    NetworkObject item = NetworkManager.Instance.SessionRunner.Spawn(networkObject, transform.position, this.transform.rotation);
    //    item.transform.SetParent(gameObject.transform);
    //    item.gameObject.GetComponent<Rigidbody>().useGravity = false;
    //    item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    //    item.name = networkObject.name;
    //}

    //public void GrabAndDropPlate(GameObject gameObject)//tabak yerine gidi� bast���nda tabak almas� ancak tabak olan bir yere bast���nda eli bo�sa �n�ndeki taba�� almas� laz�m ve e�er elinde varsa da taba�� koymas� gerekiyor.
    //{
    //    if (!gameObject)
    //    {
    //        GetPlate();
    //    }
    //    else
    //    {
    //        DropPlate();
    //        Debug.Log("taba�� b�rak");
    //    }
    //}
    //public void DropPlate()
    //{
    //    DespawnPlate(platePrefab);
    //    SpawnPlate(platePrefab);
    //    Debug.Log("tabak yok olup olu�tu");
    //}
    //public void GetPlate()
    //{
    //        GameService.Instance.playerAction.playerInteraction.PlayerPlateGrab(platePrefab);               
    //}
    //public void SpawnPlate(NetworkObject networkObject)
    //{
    //    platePrefab = NetworkManager.Instance.SessionRunner.Spawn(networkObject, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), this.transform.rotation, Object.InputAuthority);
    //    platePrefab.transform.SetParent(transform);
    //    platePrefab.gameObject.GetComponent<Rigidbody>().useGravity = false;
    //    platePrefab.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    //    platePrefab.name = networkObject.name;
    //}
    //public void DespawnPlate(NetworkObject networkObject)
    //{
    //    GameService.Instance.playerAction.RPC_Trigger(networkObject);
    //}
    //public void InteractPlateArea()
    //{
    //    GameService.Instance.playerAction.playerInteractionKitchenObject = this.gameObject;
    //}
}
