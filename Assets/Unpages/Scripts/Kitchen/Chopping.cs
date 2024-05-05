using Cysharp.Threading.Tasks;
using Fusion;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unpages.Network;

public class Chopping : KitchenObject
{
    //public ItemType itemType;
    public int choppingCount=0;
    private int foodChoppingCount;
    //public bool isFull =false;
    //public bool isSlice = false;
    //public NetworkObject item;
    //public NetworkObject itemSlice;

    public Image circleBarImage;
    public Image circleBarBackground;
    public RectTransform circleBarRectTransform;
    public override void DropItem(NetworkObject networkObject)
    {
        if (onTheCupboardObject == null)
        {
            onTheCupboardObject = GameService.Instance.spawnObject.PlayerDropCupboardItem(networkObject, anchorPoints, false);
            ResetChopping();
        }
    }

    public override void GrabItem(NetworkObject networkObject, Transform anchorPoint)
    {
        if (onTheCupboardObject != null)
        {
            GameService.Instance.spawnObject.PlayerGrabCupboardItem(onTheCupboardObject, anchorPoint, false);
            onTheCupboardObject = null;
        }

    }

    public void ChoppingFood()
    {
        if(onTheCupboardObject != null && onTheCupboardObject.GetComponent<Item>().itemType == ItemType.Food)
        {
            foodChoppingCount = onTheCupboardObject.GetComponent<FoodItem>().choppingCount;
            if(choppingCount < foodChoppingCount)
            {
                choppingCount++;
                CheckUI();
            }
            else if(choppingCount == foodChoppingCount)
            {
                NetworkObject choppedObject = GameService.Instance.networkItems.GetNetworkItemSlice(onTheCupboardObject.GetComponent<FoodItem>().foodType);
                onTheCupboardObject = GameService.Instance.spawnObject.SpawnChoppedItem(onTheCupboardObject, choppedObject ,anchorPoints,false);
            }
        }
        
        //if (!gameObject && isFull && choppingCount < foodChoppingCount)
        //{
        //    choppingCount++;
        //    circleBarImage.fillAmount = (float)choppingCount / (float)foodChoppingCount;
        //    circleBarRectTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, ((float)choppingCount / (float)foodChoppingCount) * -360));
        //}
        //else if (!gameObject && isFull && choppingCount == foodChoppingCount)
        //{
        //    DespawnFood(item);
        //    //SpawnSliceFood(GameService.Instance.networkItems.GetNetworkItemSlice(itemType));
        //    isSlice = true;
        //}
        //GameService.Instance.playerAction.stateManager.isKitchenAction = true;
    }

    private void ResetChopping()
    {
        choppingCount = 0;
        foodChoppingCount = 0;
        circleBarBackground.gameObject.SetActive(false);
    }
    private void CheckUI()
    {
        circleBarBackground.gameObject.SetActive(true);
        circleBarImage.fillAmount = (float)choppingCount / (float)foodChoppingCount;
        circleBarRectTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, ((float)choppingCount / (float)foodChoppingCount) * -360));
    }

    //public void GrabAndDropChoppingBoard(GameObject gameObject)// objeyi doðradýktan sonra ele alýnca eski halindeki objeden baþka bir tane spawn oluyo sanýrým hata var
    //{
    //    if (!gameObject && isFull && itemType!=null)
    //    {        
    //        GrabFood();
    //        itemType =ItemType.Null;
    //        DespawnFood(item);
    //        isFull = false;
    //        circleBarBackground.gameObject.SetActive(false);
    //        circleBarImage.fillAmount = 0;
    //        choppingCount = 0;
    //    }     
    //    else
    //    {
    //        //itemType = GameService.Instance.playerAction.keepObject.GetComponent<FoodItem>().foodType;
    //        if (GameService.Instance.playerAction.keepObject.GetComponent<FoodItem>().isSliced)
    //        {
    //            //SpawnFood(GameService.Instance.networkItems.GetNetworkItemSlice(itemType));
    //            isSlice = true;
    //        }
    //        else
    //        {
    //            //SpawnFood(GameService.Instance.networkItems.GetNetworkFoodItem(itemType));
    //        }
    //        DespawnFood(gameObject.GetComponent<NetworkObject>());
    //        isFull = true;
    //    }
    //}
    //[Button]
    //public void ChoppingFood(GameObject gameObject)
    //{
    //    circleBarBackground.gameObject.SetActive(true);
    //    if (!gameObject && isFull && choppingCount < itemChoppingCount)
    //    {
    //        choppingCount++;
    //        circleBarImage.fillAmount = (float)choppingCount / (float)itemChoppingCount;
    //        circleBarRectTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, ((float)choppingCount / (float)itemChoppingCount) * -360));
    //    }
    //    else if(!gameObject && isFull && choppingCount == itemChoppingCount)
    //    {
    //        DespawnFood(item);
    //        //SpawnSliceFood(GameService.Instance.networkItems.GetNetworkItemSlice(itemType));
    //        isSlice = true;
    //    }
    //}
    //public void SpawnFood(NetworkObject networkObject)
    //{
    //    item = NetworkManager.Instance.SessionRunner.Spawn(networkObject, itemAnchorPoint.position, this.transform.rotation, Object.InputAuthority);
    //    item.transform.SetParent(transform);
    //    item.gameObject.GetComponent<Rigidbody>().useGravity = false;
    //    item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    //    item.name = networkObject.name;
    //    itemChoppingCount = item.GetComponent<FoodItem>().choppingCount;
    //}
    //public void SpawnSliceFood(NetworkObject networkObject)
    //{
    //    itemSlice = NetworkManager.Instance.SessionRunner.Spawn(networkObject, itemAnchorPoint.position, this.transform.rotation, Object.InputAuthority);
    //    itemSlice.transform.SetParent(transform);
    //    itemSlice.gameObject.GetComponent<Rigidbody>().useGravity = false;
    //    itemSlice.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    //    itemSlice.name = networkObject.name;
    //    item = itemSlice;
    //}
    //public void DespawnFood(NetworkObject networkObject)
    //{
    //    GameService.Instance.playerAction.RPC_Despawn(networkObject);
    //}
    //public void GrabFood()
    //{
    //    if(isSlice)
    //    {
    //        //GameService.Instance.playerAction.playerInteraction.PlayerChoppingGrab(GameService.Instance.networkItems.GetNetworkItemSlice(itemType));
    //    }
    //    else
    //    {
    //        //GameService.Instance.playerAction.playerInteraction.PlayerChoppingGrab(GameService.Instance.networkItems.GetNetworkFoodItem(itemType));
    //    }
    //    isSlice = false;
    //}
    //public void InteractChoppingBoard()
    //{

    //    //GameService.Instance.playerAction.playerInteractionKitchenObject = this.gameObject;
    //}

    //public async void StartActivationDelay()
    //{
    //    _isInActivationDelay = true;
    //    await UniTask.WaitForSeconds(1f, cancellationToken: destroyCancellationToken);
    //    _isInActivationDelay = false;
    //}
}
