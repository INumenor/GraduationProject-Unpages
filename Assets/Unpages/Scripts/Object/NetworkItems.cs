using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkItems : MonoBehaviour
{
    public List<ItemInfo> networkItems;

    public NetworkObject GetNetworkItem(ItemType itemType)
    {
        foreach (ItemInfo itemObject in networkItems)
        {
            if(itemObject.itemType == itemType)
            {
                return itemObject.item;
            }
        }
        return null;
    }
}
