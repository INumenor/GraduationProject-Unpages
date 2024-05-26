using Cysharp.Threading.Tasks;
using Fusion;
using Sirenix.OdinInspector;
using System;
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
    public TaskSystem taskSystem;

    public Action onServiceStatus;

    public AngryBar angryBar;
    
    public  void Start()
    {  
       GameService.Instance.playerTask = this;
    }
    public void Task(int recipeRandomNumber)
    {
        foodRecipes = GameService.Instance.networkItems.networkFoodRecipes[recipeRandomNumber];
        if (/*taskRecipes.Count<4 && */!taskRecipes.ContainsKey(foodRecipes)) 
        {
            GameObject task= Instantiate(taskPrefab,taskScrollViewContent);
            taskRecipes.Add(foodRecipes,task);
            task.GetComponent<TaskUI>().taskTimeAction += DestroyTask;
            task.GetComponent<TaskUI>().Init(foodRecipes);          
        }
    }
    public void DestroyTask(FoodRecipes taskRecipe,bool isDone)
    {       
        Destroy(taskRecipes[taskRecipe]);
        taskRecipes.Remove(taskRecipe);
        if (!isDone) angryBar.playerScore -= taskRecipe.recipeScore/2;
        else angryBar.playerScore = taskRecipe.recipeScore;
        if (taskRecipes.Count < 1)
        {
            taskSystem.NewRecipe();
        }
    }
    
    public bool ChecktaskRecipes(NetworkObject plateFoodRecipes)
    {
        if(plateFoodRecipes == null) return false;
        foreach (FoodRecipes foodRecipes in taskRecipes.Keys)
        {
            if(foodRecipes.name == plateFoodRecipes.name)
            {
                angryBar.playerScore += foodRecipes.recipeScore;
                angryBar.UpdateAngryBar(foodRecipes.recipeScore/2);
                DestroyTask(foodRecipes, true);
                taskSystem.NewRecipe();
                return true;
            }
        }
        return false;
    }
    [Button]
    public void SetScore()
    {
        angryBar.playerScore += 10;
        angryBar.UpdateAngryBar(10/2);
    }

    [Button]
    public void Test()
    {
        DestroyTask(foodRecipes, true);
        taskSystem.NewRecipe();
    }
}
