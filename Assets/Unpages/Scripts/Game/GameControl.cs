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
            //foreach (FoodInfo itemInfo in spawnNetworkItem)
            //{
            //if (itemInfo.isSpawnable && itemInfo.minRandomGameSpawn < randomNumber && randomNumber < itemInfo.maxRandomGameSpawn)
            //{
            // Read the transform before the box is despawned.
            Vector3 foodPosition = spawnObject.transform.position + Vector3.down * 0.3f;
            Quaternion foodRotation = spawnObject.transform.rotation;

            BoxDestroyCount++;
            if (BoxDestroyCount >= 10)
            {
                // Refill before freeing this box's cell, so no new box lands on the dropped food.
                spawnFoodBox.SpawnMissingBox();
                //BoxSpawn();
                BoxDestroyCount = 0;
            }
            //BoxDestroy(spawnObject);
            spawnFoodBox.RemoveListBox(spawnObject);
            //Debug.Log(itemInfo.name + " " + randomNumber);
            //GameService.Instance.mouseStateManager.AreaBake();
            NetworkObject networkObject = NetworkManager.Instance.SessionRunner.Spawn(spawnNetworkItem[randomNumber].item,
                 foodPosition,
                 foodRotation);
            networkObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
            networkObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            networkObject.name = spawnNetworkItem[randomNumber].name;
            networkObject.GetComponent<Item>().AddComponentInteract();
            spawnNetworkItem.RemoveAt(randomNumber);
            if (spawnNetworkItem.Count < 1)
            {
               spawnNetworkItem = GameService.Instance.networkItems.GetNetworkFoodItem();
            }
            //}
            //}
        }
    }
}
