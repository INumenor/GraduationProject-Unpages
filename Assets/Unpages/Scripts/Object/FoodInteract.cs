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
        if (interactorData.InteractorObject.GetComponent<Outline>() != null)
        {
            interactorData.InteractorObject.GetComponent<Outline>().enabled = true;
        }
    }
    public void UnInteract()
    {
        // Interaction.OnTriggerExit may already have cleared interactionObjcet.
        NetworkObject interactionObjcet = GameService.Instance.playerAction.interactionObjcet;
        if (interactionObjcet != null && interactionObjcet.TryGetComponent(out Outline outline))
        {
            outline.enabled = false;
        }
        else if (TryGetComponent(out Outline ownOutline))
        {
            ownOutline.enabled = false;
        }
        GameService.Instance.playerAction.interactionObjcet = null;
        GameService.Instance.playerAction.interactionObjcetType = ItemType.Null;
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            if (ItemTime < 101) ItemTime++;
            // mouseStateManager only exists on the shared-mode master client.
            if (ItemTime == 100 /*1000*/ && GameService.Instance.mouseStateManager != null)
            {
                GameService.Instance.mouseStateManager.expiredFood.Add(this.gameObject.GetComponent<NetworkObject>());
            }
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("KitchenInteractable") && GameService.Instance.playerAction != null)
        {
            //Vector3 kitchenObjectPosition = new Vector3(Mathf.RoundToInt(other.transform.position.x), Mathf.RoundToInt(other.transform.position.y), Mathf.RoundToInt(other.transform.position.z));
            //Vector3 spawnPosition = kitchenObjectPosition + transform.forward * 1f;
            gameObject.transform.position = GameService.Instance.playerAction.foodSpawnPoint.position;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        ItemTime = 0;
        if (other.gameObject.CompareTag("Floor") && GameService.Instance.mouseStateManager != null)
        {
            //Debug.Log("Yerden Yüksek");
            //RPC_RemoveFoodList();
            GameService.Instance.mouseStateManager.expiredFood.Clear();
            //if (GameService.Instance.mouseStateManager.expiredFood.Contains(other.gameObject.GetComponent<NetworkObject>()))
            //{
            //    GameService.Instance.mouseStateManager.expiredFood.Remove(other.gameObject.GetComponent<NetworkObject>());
            //}
        }
    }
    //public void RemoveFoodItem()
    //{
    //    if (GameService.Instance.mouseStateManager.expiredFood.Contains(gameObject.GetComponent<NetworkObject>()))
    //    {
    //        GameService.Instance.mouseStateManager.expiredFood.Remove(gameObject.GetComponent<NetworkObject>());
    //       // GameService.Instance.mouseStateManager.isCatch = true;
    //    }
    //}

    //[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    //public void RPC_RemoveFoodList()
    //{
    //    RemoveFoodItem();
    //    //if (GameService.Instance.mouseStateManager.expiredFood.Contains(gameObject.GetComponent<NetworkObject>()))
    //    //{
    //    //Debug.Log(gameObject.name + " sNAME");
    //    //GameService.Instance.mouseStateManager.expiredFood.Remove(gameObject.GetComponent<NetworkObject>());
    //    //GameService.Instance.mouseStateManager.isCatch = true;
    //    //}
    //}
}
