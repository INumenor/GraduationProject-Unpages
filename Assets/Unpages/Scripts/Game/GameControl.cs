using Fusion;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using Unpages.Network;

public class GameControl : MonoBehaviour
{
    public System.Action<GameObject> BoxDestroy;
    public System.Action BoxSpawn;
    public int BoxDestroyCount;
    public void ObjectSpawn(GameObject spawnObject)
    {
        if (NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient)
        {
            List<ItemInfo> spawnNetworkItem = GameService.Instance.networkItems.networkFoodItems;
            int randomNumber = Random.Range(0, 101);
            foreach (FoodInfo itemInfo in spawnNetworkItem)
            {
                if (itemInfo.isSpawnable && itemInfo.minRandomGameSpawn < randomNumber && randomNumber < itemInfo.maxRandomGameSpawn)
                {
                    BoxDestroyCount++;
                    BoxDestroy(spawnObject);
                    if (BoxDestroyCount >= 10)
                    {
                        BoxSpawn();
                        BoxDestroyCount = 0;
                    }
                    Debug.Log(itemInfo.name + " " + randomNumber);
                    //Instantiate(itemInfo.item, spawnPoint);
                    NetworkObject networkObject = NetworkManager.Instance.SessionRunner.Spawn(itemInfo.item, spawnObject.transform.position, spawnObject.transform.rotation);
                    networkObject.name = itemInfo.name;
                    networkObject.GetComponent<Item>().AddComponentInteract();
                    //Runner.Spawn(itemInfo.item,position: spawnPoint.position,rotation : spawnPoint.rotation,Object.StateAuthority);
                }
            }
        }
    }
}
