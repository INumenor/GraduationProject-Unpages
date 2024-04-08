using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Unpages.Network;
using Unity.VisualScripting;

public enum ItemType {Tomato,Bread,Chess,Lettuce,Null}
public class Item : MonoBehaviour, IInteractable
{
    public ItemType foodType;
    int ItemTime = 0;

    public void Interact(InteractorData interactorData)
    {
        //GameService.Instance.playerAction.grabbableObject =this.gameObject;
        GameService.Instance.playerAction.grabbableObject = interactorData.InteractorObject;
        GameService.Instance.playerAction.grabbableObjectType = foodType;
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {           
            ItemTime++;
            Debug.Log(ItemTime);
            if (ItemTime > 1000)
            {
                GameService.Instance.aiManagerSystem.Init(transform.position);
             
            }
        }
    }
    private void OnCollisionExit(Collision col)
    {
            ItemTime = 0;
            Debug.Log("gelmeeeeeeeeeeeeeeeeeeeeeeeeeeeeeek");
            GameService.Instance.aiManagerSystem.ReturnBase();
            Debug.Log("çalýþmaaaaaaaaaaaaaaaaaaaaaaaaaaaaaak");               
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Despawn()
    {
        //Destroy(gameObject);
        NetworkManager.Instance.SessionRunner.Despawn(GetComponent<NetworkObject>());
    }
}
