using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class Chopping :NetworkBehaviour
{
    public ItemType itemType;
    public int choppingCount=0;
    public bool isFull =false;
    public NetworkObject item;
    public NetworkObject itemSlice;
    public void GrabAndDropChoppingBoard(GameObject gameObject)// objeyi doðradýktan sonra ele alýnca eski halindeki objeden baþka bir tane spawn oluyo sanýrým hata var
    {
        if (!gameObject && isFull && itemType!=null)
        {        
            GrabFood();
            itemType =ItemType.Null;
            DespawnFood(item);
            isFull = false;
        }     
        else
        {
            itemType = GameService.Instance.playerAction.keepObject.GetComponent<Item>().foodType;
            DespawnFood(gameObject.GetComponent<NetworkObject>());
            SpawnFood();
            isFull = true;
        }
    }
    public void ChoppingFood(GameObject gameObject)
    {
        if (!gameObject && isFull && choppingCount==0)
        {
            choppingCount++;
            DespawnFood(item);
            SpawnSliceFood();
        }
        else if(!gameObject && isFull && choppingCount>0 && choppingCount < 5)
        {
            choppingCount++;
        }
    }
    public void SpawnFood()
    {
        NetworkObject networkObject=GameService.Instance.networkItems.GetNetworkItem(itemType);
        item = NetworkManager.Instance.SessionRunner.Spawn(networkObject, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), this.transform.rotation, Object.InputAuthority);
        item.transform.SetParent(transform);
        item.gameObject.GetComponent<Rigidbody>().useGravity = false;
        item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        item.name = networkObject.name;
    }
    public void SpawnSliceFood()
    {
        NetworkObject networkObject = GameService.Instance.networkItems.GetNetworkItemSlice(itemType);
        itemSlice = NetworkManager.Instance.SessionRunner.Spawn(networkObject, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), this.transform.rotation, Object.InputAuthority);
        itemSlice.transform.SetParent(transform);
        itemSlice.gameObject.GetComponent<Rigidbody>().useGravity = false;
        itemSlice.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        itemSlice.name = networkObject.name;
    }
    public void DespawnFood(NetworkObject networkObject)
    {
        GameService.Instance.playerAction.RPC_Trigger(networkObject);
    }
    public void GrabFood()
    {
        GameService.Instance.playerAction.playerInteraction.PlayerChoppingGrab(GameService.Instance.networkItems.GetNetworkItem(itemType));
    }
    public void InteractChoppingBoard()
    {

        GameService.Instance.playerAction.playerInteractionKitchenObject = this.gameObject;
        Debug.Log("kdfokoef");

    }
}
