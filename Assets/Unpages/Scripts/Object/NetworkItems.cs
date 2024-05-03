using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkItems : MonoBehaviour
{
    public List<ItemInfo> networkFoodItems;
    public List<ItemInfo> networkItemsSlice;
    public List<ItemInfo> networkItems;
    public List<ItemInfo> networkItemsPlates;

    public NetworkObject GetNetworkFoodItem(ItemType itemType)
    {
        foreach (ItemInfo itemObject in networkFoodItems)
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

    public NetworkObject GetNetworkItemPlate(ItemType itemType)
    {
        foreach (ItemInfo itemSliceObject in networkItemsPlates)
        {
            if (itemSliceObject.itemType == itemType)
            {
                return itemSliceObject.item;
            }
        }
        return null;
    }

    //public NetworkObject GetNetworkItem(OtherItemType itemType)
    //{     
    //    foreach(ItemInfo item in networkItems)
    //    {
    //        if(item.itemType== itemType)
    //        {
    //            return item.item;
    //        }
    //    }
    //    return null;
    //}
}
