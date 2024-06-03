using Fusion;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class GameControl : MonoBehaviour
{
    public System.Action<GameObject> BoxDestroy;
    public System.Action BoxSpawn;
    public SpawnFoodBox spawnFoodBox;
    public List<ItemInfo> spawnNetworkItem = new List<ItemInfo>();
    public int BoxDestroyCount;

    private void Start()
    {
        if (NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient)
        {
           spawnNetworkItem = GameService.Instance.networkItems.GetNetworkFoodItem();
        }
    }
    public void ObjectSpawn(GameObject spawnObject)
    {
        if (NetworkManager.Instance.SessionRunner.IsSharedModeMasterClient)
        {
            int randomNumber = Random.Range(0, spawnNetworkItem.Count);
            Debug.Log(randomNumber + " random numbeerrr");
            //foreach (FoodInfo itemInfo in spawnNetworkItem)
            //{
            //if (itemInfo.isSpawnable && itemInfo.minRandomGameSpawn < randomNumber && randomNumber < itemInfo.maxRandomGameSpawn)
            //{
            BoxDestroyCount++;
            //BoxDestroy(spawnObject);
            spawnFoodBox.RemoveListBox(spawnObject);
            if (BoxDestroyCount >= 10)
            {
                spawnFoodBox.SpawnMissingBox();
                //BoxSpawn();
                BoxDestroyCount = 0;
            }
            //Debug.Log(itemInfo.name + " " + randomNumber);
            //GameService.Instance.mouseStateManager.AreaBake();
            NetworkObject networkObject = NetworkManager.Instance.SessionRunner.Spawn(spawnNetworkItem[randomNumber].item,
                 new Vector3(spawnObject.transform.position.x, spawnObject.transform.position.y - 0.3f, spawnObject.transform.position.z),
                 spawnObject.transform.rotation);
            networkObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
            networkObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            networkObject.name = spawnNetworkItem[randomNumber].name;
            networkObject.GetComponent<Item>().AddComponentInteract();
            spawnNetworkItem.RemoveAt(randomNumber);
            Debug.Log(spawnNetworkItem.Count);
            if (spawnNetworkItem.Count < 1)
            {
               spawnNetworkItem = GameService.Instance.networkItems.GetNetworkFoodItem();
            }
            //}
            //}
        }
    }
}
