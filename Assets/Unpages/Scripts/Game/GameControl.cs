using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class GameControl : MonoBehaviour
{
    public void ObjectSpawn(Transform spawnPoint)
    {
        List<ItemInfo> spawnNetworkItem = GameService.Instance.networkItems.networkItems;
        int randomNumber = Random.Range(0, 101);
        foreach (ItemInfo itemInfo in spawnNetworkItem)
        {
            if (itemInfo.isSpawnable && itemInfo.minRandomGameSpawn < randomNumber && randomNumber < itemInfo.maxRandomGameSpawn)
            {
                Debug.Log(itemInfo.name + " " + randomNumber);
                //Instantiate(itemInfo.item, spawnPoint);
                NetworkManager.Instance.SessionRunner.Spawn(itemInfo.item, spawnPoint.position, spawnPoint.transform.rotation);
                //Runner.Spawn(itemInfo.item,position: spawnPoint.position,rotation : spawnPoint.rotation,Object.StateAuthority);
            }
        }
    }
}
