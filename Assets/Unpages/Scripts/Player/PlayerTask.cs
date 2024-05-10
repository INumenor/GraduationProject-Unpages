using Cysharp.Threading.Tasks;
using Fusion;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unpages.Network;

public class PlayerTask : SerializedMonoBehaviour
{
    public FoodRecipes foodRecipes;

    public Dictionary<FoodRecipes,GameObject> taskRecipes =new Dictionary<FoodRecipes, GameObject>();
    public int taskRandomNumber;

    public Transform taskScrollViewContent;
    public GameObject taskPrefab;

    //public override void Spawned()
    //{
    //    if (Object.HasStateAuthority)
    //    {
    //        GameService.Instance.playerTask = this;
           
    //    }
    //}
    public  void Start()
    {     
            GameService.Instance.playerTask = this;
    }
    public void Task(int recipeRandomNumber)
    {
        foodRecipes = GameService.Instance.networkItems.networkFoodRecipes[recipeRandomNumber];
        if (taskRecipes.Count<4 && !taskRecipes.ContainsKey(foodRecipes)) 
        {
            GameObject task= Instantiate(taskPrefab,taskScrollViewContent);
            taskRecipes.Add(foodRecipes,task);
            task.GetComponent<TaskUI>().taskTimeAction += DestroyTask;
            task.GetComponent<TaskUI>().Init(foodRecipes);          
        }
    }
    public void DestroyTask(FoodRecipes taskRecipe)
    {       
        Destroy(taskRecipes[taskRecipe]);
        taskRecipes.Remove(taskRecipe);
    }
}
