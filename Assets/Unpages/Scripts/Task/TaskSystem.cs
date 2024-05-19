using Cysharp.Threading.Tasks;
using Fusion;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSystem : NetworkBehaviour
{
    //[Networked,OnChangedRender(nameof(SetTask))] public int randomRecipeNumber { get; set; }
    //[Networked, OnChangedRender(nameof(NewRecipe))] public NetworkBool isRecipeDone { get; set; }


    private async UniTask Start()
    {
        GameService.Instance.playerTask.onServiceStatus += RandomRecipe;
        GameService.Instance.playerTask.taskSystem = this;
        await UniTask.WaitForSeconds(5f);
        /*if (Runner.IsSharedModeMasterClient)*/ RandomRecipe();
    }
    //public override FixedUpdateNetwork()
    //{
    //    if (Runner.IsSharedModeMasterClient && GameService.Instance.playerTask.taskRecipes.Count < 1)
    //    {
    //        RandomRecipe();
    //    } 
    //}
    public void NewRecipe()
    {
        /*if (Runner.IsSharedModeMasterClient && isRecipeDone)*/
        RPC_NewTask(Random.Range(0, GameService.Instance.networkItems.networkTaskFoodRecipes.Count));
    }

    public void RandomRecipe()
    {
        RPC_StartTask(Random.Range(0, GameService.Instance.networkItems.networkTaskFoodRecipes.Count));
    }

    [Rpc(RpcSources.StateAuthority,RpcTargets.All)]
    public void RPC_StartTask(int randomRecipeNumber)
    {
        GameService.Instance.playerTask.Task(randomRecipeNumber);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_NewTask(int randomRecipeNumber)
    {
        GameService.Instance.playerTask.Task(randomRecipeNumber);
    } 
}
