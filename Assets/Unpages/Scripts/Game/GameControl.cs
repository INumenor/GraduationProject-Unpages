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
                    //GameService.Instance.mouseStateManager.AreaBake();
                    NetworkObject networkObject = NetworkManager.Instance.SessionRunner.Spawn(itemInfo.item,
                         new Vector3(spawnObject.transform.position.x, spawnObject.transform.position.y - 0.3f, spawnObject.transform.position.z),
                         spawnObject.transform.rotation);
                    networkObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    networkObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    networkObject.name = itemInfo.name;
                    networkObject.GetComponent<Item>().AddComponentInteract();
                }
            }
        }
    }
}
