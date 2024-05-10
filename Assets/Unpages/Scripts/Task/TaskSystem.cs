using Fusion;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSystem : NetworkBehaviour
{
    public int randomRecipeNumber;

   [Button]
    public void RandomRecipe()
    {
            randomRecipeNumber = Random.RandomRange(0, GameService.Instance.networkItems.networkTaskFoodRecipes.Count);
            GameService.Instance.playerTask.Task(randomRecipeNumber);

    }
}
