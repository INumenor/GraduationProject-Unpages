using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class Storage : MonoBehaviour
{
    PlayerInteraction playerInteraction;
    public ItemType itemTpye;
    int tomatoCount = 0;
    int cheeseCount = 0;
    int lettuceCount = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.name == other.gameObject.name)
        {
            DropItemStorage(other);
        }
        if (other.CompareTag("Player"))
        {
            if(GameService.Instance.playerAction.grabbableObject == null)
            {
                GetItemStorage();
            }          
        }        
    }
    public void DropItemStorage(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "TomatoFood":
                if (tomatoCount < 5)
                {
                    tomatoCount++;
                    itemTpye = ItemType.Tomato;
                    Destroy(other.gameObject);
                }
                break;
            case "CheeseFood":
                if (cheeseCount < 5)
                {
                    cheeseCount++;
                    itemTpye = ItemType.Cheese;
                    Destroy(other.gameObject);
                }
                break;
            case "LettuceFood":
                if (lettuceCount < 5)
                {
                    lettuceCount++;
                    itemTpye = ItemType.Lettuce;
                    Destroy(other.gameObject);
                }
                break;
        }
    }
    public void GetItemStorage()
    {
        Debug.Log(GameService.Instance.networkItems.GetNetworkItem(itemTpye) + "olmak");
        playerInteraction.PlayerGrabItem(GameService.Instance.networkItems.GetNetworkItem(itemTpye));
    }
}
