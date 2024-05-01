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
                FoodPreparation(gameObject);
                break;
            case "ChoppingBoard":
                GrabFoodChopping(gameObject);
                break;
        }
        StartActivationDelay();

    }
    public void ActionKitchenObject(GameObject gameObject)
    {
        if (_isInActivationDelay) return;
        switch (gameObject.tag)
        {
            //case "Storage":         
            //    break;
            //case "Trash":       
            //    break;
            //case "Plate":
            //    FoodPreparation(gameObject);
            //    break;
            case "ChoppingBoard":
                ChoppingFood(gameObject);
                break;
        }
        GameService.Instance.playerAction.isKitchenAction = true;
        StartActivationDelay();

    }
    public void FoodStorage(GameObject gameObject)
    {
        if (GameService.Instance.playerAction.keepObject !=null) { gameObject.GetComponent<Storage>().DropItemStorage(GameService.Instance.playerAction.keepObject); }
        else { gameObject.GetComponent<Storage>().GetItemStorage(); }

    }
   public void GrabFoodChopping(GameObject gameObject)
    {
        gameObject.GetComponent<Chopping>().GrabAndDropChoppingBoard(GameService.Instance.playerAction.keepObject); 
    }
    public void ChoppingFood(GameObject gameObject)
    {
        gameObject.GetComponent<Chopping>().ChoppingFood(GameService.Instance.playerAction.keepObject);
    }
    public void FoodPreparation(GameObject gameObject)
    {
        gameObject.GetComponent<PlateKitchen>().GrabAndDropPlate(GameService.Instance.playerAction.keepObject);
    }
    public void Trash(GameObject gameObject)
    {
        gameObject.GetComponent<Trash>().TrashObject(GameService.Instance.playerAction.keepObject);
    }
    public async void StartActivationDelay()
    {
        _isInActivationDelay = true;
        await UniTask.WaitForSeconds(1f, cancellationToken: destroyCancellationToken);
        _isInActivationDelay = false;
    }
}
