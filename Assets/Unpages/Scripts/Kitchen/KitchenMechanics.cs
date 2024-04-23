using Cysharp.Threading.Tasks;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum KitchenObjectTag { Storage,Plate,ChoppingBoard,Trash}
public class KitchenMechanics : MonoBehaviour
{
    private bool _isInActivationDelay;
    public void SetKitchenObject(GameObject gameObject)
    {
        Debug.Log("sela"+gameObject.name);
        StartActivationDelay();
        switch (gameObject.tag)
        {
            case "Storage":
                FoodStorage(gameObject);
                break;
            case "Trash":
                Trash();
                break;
            case "Plate":
                FoodPreparation();
                break;
            case "ChoppingBoard":
                FoodChopping();
                break;
        }
    }
   public void FoodStorage(GameObject gameObject)
    {
        if (!GameService.Instance.playerAction.keepObject) gameObject.GetComponent<Storage>().GetItemStorage();
        else gameObject.GetComponent<Storage>().DropItemStorage(GameService.Instance.playerAction.keepObject);

    }
   public void FoodChopping()
    {

    }
    public void FoodPreparation()
    {

    }
    public void Trash()
    {

    }
    public async void StartActivationDelay()
    {
        _isInActivationDelay = true;
        await UniTask.WaitForSeconds(15f, cancellationToken: destroyCancellationToken);
        _isInActivationDelay = false;
    }
}
