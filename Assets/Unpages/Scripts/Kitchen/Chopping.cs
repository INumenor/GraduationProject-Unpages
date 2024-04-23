using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class Chopping :NetworkBehaviour
{
    public ItemType itemType;
    public int choppingCount;
    public bool isFull =false;
    public NetworkObject item;
    public void GrabAndDropChoppingBoard(GameObject gameObject)// sonra bu itema bir kez e ile vurulursa o objede despawn olup yerine kesilmiþ hali gelicek ve bu vurma sayýlarýný tutmasý gerekecek 5'ten fazla vurulamayacak ve eðer alýnýp tekrar konulursa kaldýðý yerden devam etmesi gerekecek eðer tabaða konmaya çalýþýlýnýrsa koyulmamasý gerekli.
    {
        if (!gameObject && isFull && itemType!=null)
        {        
            GrabFood();
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
    public void SpawnFood()
    {
        NetworkObject networkObject=GameService.Instance.networkItems.GetNetworkItem(itemType);
        item = NetworkManager.Instance.SessionRunner.Spawn(networkObject, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), this.transform.rotation, Object.InputAuthority);
        item.transform.SetParent(transform);
        item.gameObject.GetComponent<Rigidbody>().useGravity = false;
        item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        item.name = networkObject.name;
    }
    public void DespawnFood(NetworkObject gameObject)
    {
        GameService.Instance.playerAction.RPC_Trigger(gameObject);
    }
    public void GrabFood()
    {
        GameService.Instance.playerAction.playerInteraction.PlayerChoppingGrab(GameService.Instance.networkItems.GetNetworkItem(itemType));
    }
    public void InteractChoppingBoard()
    {

        GameService.Instance.playerAction.playerInteractionKitchenObject = this.gameObject;

    }
}
