using Fusion;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class GameControl : MonoBehaviour
{
    public void ObjectSpawn(Transform spawnPoint)
    {
        if (NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient)
        {
            List<ItemInfo> spawnNetworkItem = GameService.Instance.networkItems.networkFoodItems;
            int randomNumber = Random.Range(0, 101);
            foreach (FoodInfo itemInfo in spawnNetworkItem)
            {
                if (itemInfo.isSpawnable && itemInfo.minRandomGameSpawn < randomNumber && randomNumber < itemInfo.maxRandomGameSpawn)
                {
                    Debug.Log(itemInfo.name + " " + randomNumber);
                    //Instantiate(itemInfo.item, spawnPoint);
                    NetworkObject networkObject = NetworkManager.Instance.SessionRunner.Spawn(itemInfo.item, spawnPoint.position, spawnPoint.transform.rotation);
                    networkObject.name = itemInfo.name;
                    networkObject.GetComponent<Item>().AddComponentInteract();
                    //Runner.Spawn(itemInfo.item,position: spawnPoint.position,rotation : spawnPoint.rotation,Object.StateAuthority);
                }
            }
        }
    }
}
