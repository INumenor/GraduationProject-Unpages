public enum FoodType { Tomato, Bread, Cheese, Lettuce, Null }
public class FoodItem : Item
{
    public FoodType foodType;
    public int choppingCount;
    public bool isProcessed;
    public bool isPlateHolder;
    public FoodInteract foodInteract;
    public override void AddComponentInteract()
    {
        base.AddComponentInteract();
        foodInteract = gameObject.AddComponent<FoodInteract>();
    }

    //public void Interact(InteractorData interactorData)
    //{
    //    //GameService.Instance.playerAction.grabbableObject =this.gameObject;
    //    GameService.Instance.playerAction.grabbableObject = interactorData.InteractorObject;
    //    GameService.Instance.playerAction.grabbableObjectType = foodType;
    //}
}
