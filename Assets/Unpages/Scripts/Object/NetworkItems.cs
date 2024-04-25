using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkItems : MonoBehaviour
{
    public List<ItemInfo> networkItems;
    public List<ItemInfo> networkItemsSlice;

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
    public NetworkObject GetNetworkItemSlice(ItemType itemType)
    {
        foreach(ItemInfo itemSliceObject in networkItemsSlice)
        {
            if (itemSliceObject.itemType == itemType)
            {
                return itemSliceObject.item;
            }
        }
        return null;
    }
}
