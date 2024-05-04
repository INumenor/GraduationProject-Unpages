using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkItems : MonoBehaviour
{
    public List<FoodInfo> networkFoodItems;
    public List<FoodInfo> networkItemsSlice;
    public List<FoodInfo> networkItems;
    public NetworkObject networkPlate;


    public NetworkObject GetNetworkFoodItem(FoodType itemType)
    {
        foreach (FoodInfo itemObject in networkFoodItems)
        {
            if(itemObject.foodtype == itemType)
            {
                return itemObject.item;
            }
        }
        return null;
    }
    //public NetworkObject GetNetworkItemSlice(ItemType itemType)
    //{
    //    foreach(FoodInfo itemSliceObject in networkItemsSlice)
    //    {
    //        if (itemSliceObject.foodtype == itemType)
    //        {
    //            return itemSliceObject.item;
    //        }
    //    }
    //    return null;
    //}

    public NetworkObject GetNetworkItemPlate(ItemType itemType)
    {
        if(itemType == ItemType.Plate)
        {
            return networkPlate;
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
