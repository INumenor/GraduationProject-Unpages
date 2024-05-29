using Fusion;
using Unpages.Network;

public enum FoodType { Tomato, Bread, Cheese, Lettuce, Null }
public class FoodItem : Item
{
    public FoodType foodType;
    public int choppingCount;
    public bool isProcessed;
    public bool isPlateHolder;
    public FoodInteract foodInteract;

    
    public override void RPCTrigger()
    {
        if(isInteractable && !foodInteract)
        {
            foodInteract = gameObject.AddComponent<FoodInteract>();
        }
    }

    public async void Despawn(NetworkObject networkObject)
    {
        networkObject.GetComponent<NetworkObject>().RequestStateAuthority();
        NetworkManager.Instance.SessionRunner.Despawn(networkObject);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Despawn(NetworkObject networkObject)
    {
        Despawn(networkObject);
    }

    //public void Interact(InteractorData interactorData)
    //{
    //    //GameService.Instance.playerAction.grabbableObject =this.gameObject;
    //    GameService.Instance.playerAction.grabbableObject = interactorData.InteractorObject;
    //    GameService.Instance.playerAction.grabbableObjectType = foodType;
    //}
}
