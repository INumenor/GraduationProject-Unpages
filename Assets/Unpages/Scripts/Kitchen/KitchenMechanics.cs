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
        if (_isInActivationDelay) return;
        switch (gameObject.tag)
        {
            case "Storage":
                FoodStorage(gameObject);
                break;
            case "Trash":
                Trash(gameObject);
                break;
            case "Plate":
                FoodPreparation();
                break;
            case "ChoppingBoard":
                FoodChopping(gameObject);
                break;
        }
        StartActivationDelay();

    }
    public void FoodStorage(GameObject gameObject)
    {
        if (GameService.Instance.playerAction.keepObject !=null) { gameObject.GetComponent<Storage>().DropItemStorage(GameService.Instance.playerAction.keepObject); }
        else { gameObject.GetComponent<Storage>().GetItemStorage(); }

    }
   public void FoodChopping(GameObject gameObject)
    {
        gameObject.GetComponent<Chopping>().GrabAndDropChoppingBoard(GameService.Instance.playerAction.keepObject); 
    }
    public void FoodPreparation()
    {

    }
    public void Trash(GameObject gameObject)
    {
        gameObject.GetComponent<Trash>().TrashObject(GameService.Instance.playerAction.keepObject);
    }
    public async void StartActivationDelay()
    {
        _isInActivationDelay = true;
        await UniTask.WaitForSeconds(0.2f, cancellationToken: destroyCancellationToken);
        _isInActivationDelay = false;
    }
}
