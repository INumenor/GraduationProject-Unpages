using DG.Tweening;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkItems : MonoBehaviour
{
    public List<ItemInfo> networkFoodItems;
    public List<FoodInfo> networkItemsSlice;
    public List<FoodRecipes> networkFoodRecipes;
    public List<FoodRecipes> networkTaskFoodRecipes;
    public List<FoodInfo> networkItems;
    public NetworkObject networkPlate;


    public NetworkObject GetNetworkFoodItem(FoodType foodType)
    {
        foreach (FoodInfo itemObject in networkFoodItems)
        {
            if(itemObject.foodtype == foodType)
            {
                return itemObject.item;
            }
        }
        return null;
    }
    public NetworkObject GetNetworkItemSlice(FoodType foodType)
    {
        foreach (FoodInfo itemSliceObject in networkItemsSlice)
        {
            if (itemSliceObject.foodtype == foodType)
            {
                return itemSliceObject.item;
            }
        }
        return null;
    }

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

    public NetworkObject GetNetworkFoodRecipes(List<FoodType> foodTypes)
    {
        foreach(FoodRecipes foodRecipe in networkFoodRecipes)
        {
            if(foodTypes.Count == foodRecipe.foodTypes.Count)
            {
                int matchFood = 0;
                for (int i = 0; i < foodTypes.Count; i++)
                {
                    if (foodRecipe.foodTypes.Contains(foodTypes[i]))
                    {
                        matchFood++;
                    }
                }
                if(matchFood == foodTypes.Count)
                {
                    return foodRecipe.foodRecipe;
                }
            }
        }
        return null;
    }

    public NetworkObject GetNetworkRecipes(NetworkObject respawnRecipe)
    {
        foreach (FoodRecipes foodRecipe in networkFoodRecipes)
        {
            if (foodRecipe.foodRecipe == respawnRecipe)
            {
                return foodRecipe.foodRecipe;
            }
        }
        return null;
    }
    public Sprite GetImageFoodItem(FoodType foodType)
    {
        foreach (FoodInfo itemObject in networkFoodItems)
        {
            if (itemObject.foodtype == foodType)
            {
                return itemObject.itemImage;
            }
        }
        return null;
    }
}
