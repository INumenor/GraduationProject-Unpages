using Fusion;
using UnityEngine;

public class FoodInteract : MonoBehaviour, IInteractable
{
    int ItemTime = 0;
    public void Interact(InteractorData interactorData)
    {
        //GameService.Instance.playerAction.grabbableObject = this.GetComponent<NetworkObject>();
        GameService.Instance.playerAction.interactionObjcet = interactorData.InteractorObject.GetComponent<NetworkObject>();
        GameService.Instance.playerAction.interactionObjcetType = this.GetComponent<FoodItem>().itemType;
    }
    public void UnInteract()
    {
        GameService.Instance.playerAction.interactionObjcet = null;
        GameService.Instance.playerAction.interactionObjcetType = ItemType.Null;
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            if (ItemTime < 101) ItemTime++;
            if (ItemTime == 100 /*1000*/)
            {
                GameService.Instance.mouseStateManager.expiredFood.Add(this.gameObject.GetComponent<NetworkObject>());             
            }
        }
    }
    private void OnCollisionExit(Collision col)
    {
        ItemTime = 0;
        Debug.Log("TurnBase :" + ItemTime);
    }

}
