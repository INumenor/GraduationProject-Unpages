using Cysharp.Threading.Tasks;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class PlayerTask : NetworkBehaviour
{
    public FoodRecipes foodRecipes;
    public TaskUI taskUI;

    public List<FoodRecipes> taskRecipes = new List<FoodRecipes>();
    public int taskRandomNumber;

    public Transform taskScrollViewContent;
    public GameObject taskPrefab;

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            GameService.Instance.playerTask = this;
           
        }
    }
    public void Task()
    {
        foodRecipes = GameService.Instance.networkItems.networkFoodRecipes[taskRandomNumber];
        if (taskRecipes.Count<4) 
        {
           GameObject task= Instantiate(taskPrefab,taskScrollViewContent);
            //task.transform.SetParent(taskScrollViewContent);
            task.GetComponent<TaskUI>().Init(foodRecipes.recipeImages,foodRecipes.foodTypes);
            taskRecipes.Add(foodRecipes);
        }
    }
}
