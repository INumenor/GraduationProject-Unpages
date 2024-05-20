using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
public class KitchenMechanics : MonoBehaviour
{
    private bool _isInActivationDelay;
    #region Select Kitchen Player Drop And Grap
    public void SelectKitchenGD(NetworkObject kitchenObject ,NetworkObject keepObject, KitchenObjectType kitchenObjectType, Transform anchorPoint)
    {
        if (_isInActivationDelay) return;

        if (kitchenObject.GetComponent<KitchenObject>().onTheCupboardObject != null &&
           kitchenObject.GetComponent<KitchenObject>().onTheCupboardObject.GetComponent<Item>().itemType == ItemType.Plate
           && keepObject != null)
        {
            KitchenObjectPlateDropItem(kitchenObject, keepObject);
        }
        else if (keepObject == null)
        {
            KitchenObjectGrabItem(kitchenObject, kitchenObjectType, anchorPoint, keepObject);
        }
        else
        {
            KitchenObjectDropItem(kitchenObject, keepObject, kitchenObjectType);
        }
        StartActivationDelay();
    }


    public void KitchenObjectDropItem(NetworkObject kitchenObject,NetworkObject keepObject,KitchenObjectType kitchenObjectType)
    {
        if(_isInActivationDelay) return;

        kitchenObject.GetComponent<KitchenObject>().DropItem(keepObject);

        StartActivationDelay();

        //switch (kitchenObjectType)
        //{
        //    case KitchenObjectType.Cupboard:
        //        kitchenObject.GetComponent<KitchenObject>().DropItem(GameService.Instance.playerAction.keepObject);
        //        break;
        //    default:
        //        break;
        //}

    }

    public void KitchenObjectGrabItem(NetworkObject kitchenObject, KitchenObjectType kitchenObjectType,Transform anchorPoint,NetworkObject keepObjcet)
    {
        if (_isInActivationDelay) return;

         kitchenObject.GetComponent<KitchenObject>().GrabItem(kitchenObject, anchorPoint);

        //switch (kitchenObjectType)
        //{
        //    case KitchenObjectType.Cupboard:
        //        kitchenObject.GetComponent<KitchenObject>().GrabItem(kitchenObject,anchorPoint);
        //        break;
        //    default:
        //        break;
        //}
        StartActivationDelay();
    }

    public void KitchenObjectPlateDropItem(NetworkObject kitchenObject,NetworkObject keepObjcet)
    {
        if (_isInActivationDelay) return;

        kitchenObject.GetComponent<KitchenObject>().PlateDrop(keepObjcet);

        StartActivationDelay();
    }

    #endregion

    #region Kitchen Objcet Player Action
    public void SelectKitchenAction(NetworkObject kitchenObject, NetworkObject keepObject, KitchenObjectType kitchenObjectType)
    {
        if (_isInActivationDelay) return;

        if (keepObject == null)
        {
            switch (kitchenObjectType)
            {
                case KitchenObjectType.ChoppingBoard:
                    kitchenObject.GetComponent<Chopping>().ChoppingFood();
                    break;
                default: break;
            }
        }
        else
        {
            //KitchenObjectDropItem(kitchenObject, keepObject, kitchenObjectType);
        }
        StartActivationDelay();
    }
    #endregion

    //public void SetKitchenObject(GameObject gameObject)
    //{
    //    if (_isInActivationDelay) return;
    //    switch (gameObject.tag)
    //    {
    //        case "Storage":
    //            FoodStorage(gameObject);
    //            break;
    //        case "Trash":
    //            Trash(gameObject);
    //            break;
    //        case "Plate":
    //            FoodPreparation(gameObject);
    //            break;
    //        case "ChoppingBoard":
    //            GrabFoodChopping(gameObject);
    //            break;
    //    }
    //    StartActivationDelay();

    //}
    //public void ActionKitchenObject(GameObject gameObject)
    //{
    //    if (_isInActivationDelay) return;
    //    switch (gameObject.tag)
    //    {
    //        //case "Storage":         
    //        //    break;
    //        //case "Trash":       
    //        //    break;
    //        //case "Plate":
    //        //    FoodPreparation(gameObject);
    //        //    break;
    //        case "ChoppingBoard":
    //            ChoppingFood(gameObject);
    //            break;
    //    }
    //    GameService.Instance.playerAction.isKitchenAction = true;
    //    StartActivationDelay();

    //}
    // public void FoodStorage(GameObject gameObject)
    // {
    //     //if (GameService.Instance.playerAction.keepObject !=null) { gameObject.GetComponent<Storage>().DropItemStorage(GameService.Instance.playerAction.keepObject); }
    //     else { gameObject.GetComponent<Storage>().GetItemStorage(); }

    // }
    //public void GrabFoodChopping(GameObject gameObject)
    // {
    //     gameObject.GetComponent<Chopping>().GrabAndDropChoppingBoard(GameService.Instance.playerAction.keepObject); 
    // }
    // public void ChoppingFood(GameObject gameObject)
    // {
    //     gameObject.GetComponent<Chopping>().ChoppingFood(GameService.Instance.playerAction.keepObject);
    // }
    // public void FoodPreparation(GameObject gameObject)
    // {
    //     gameObject.GetComponent<PlateKitchen>().GrabAndDropPlate(GameService.Instance.playerAction.keepObject);
    // }
    // public void Trash(GameObject gameObject)
    // {
    //     gameObject.GetComponent<Trash>().TrashObject(GameService.Instance.playerAction.keepObject);
    // }
    public async void StartActivationDelay()
    {
        _isInActivationDelay = true;
        await UniTask.WaitForSeconds(.1f, cancellationToken: destroyCancellationToken);
        _isInActivationDelay = false;
    }
}
