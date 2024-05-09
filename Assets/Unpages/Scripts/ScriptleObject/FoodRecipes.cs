using Fusion;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FoodRecipeType {Default,Sandwich,Burger,Pizza,Soup,Salad}

[CreateAssetMenu(fileName = "InfoScriptableObject", menuName = "Unpages/ScriptableObject/FoodRecipes")]
public class FoodRecipes : SerializedScriptableObject
{
    public NetworkObject foodRecipe;
    public FoodRecipeType foodRecipeType;
    public List<FoodType> foodTypes = new List<FoodType>();
    public Sprite recipeImages;
    public float recipeTime;
}
