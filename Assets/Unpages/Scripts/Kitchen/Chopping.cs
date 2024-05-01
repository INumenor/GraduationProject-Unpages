using Cysharp.Threading.Tasks;
using Fusion;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unpages.Network;

public class Chopping :NetworkBehaviour
{
    public ItemType itemType;
    public int choppingCount=0;
    private int itemChoppingCount;
    public bool isFull =false;
    public bool isSlice = false;
    public NetworkObject item;
    public NetworkObject itemSlice;

    public Image circleBarImage;
    public Image circleBarBackground;
    public RectTransform circleBarRectTransform;

    public Transform itemAnchorPoint;

    private bool _isInActivationDelay;
    public void GrabAndDropChoppingBoard(GameObject gameObject)// objeyi doðradýktan sonra ele alýnca eski halindeki objeden baþka bir tane spawn oluyo sanýrým hata var
    {
        if (!gameObject && isFull && itemType!=null)
        {        
            GrabFood();
            itemType =ItemType.Null;
            DespawnFood(item);
            isFull = false;
            circleBarBackground.gameObject.SetActive(false);
            circleBarImage.fillAmount = 0;
            choppingCount = 0;
        }     
        else
        {
            itemType = GameService.Instance.playerAction.keepObject.GetComponent<FoodItem>().foodType;
            if (GameService.Instance.playerAction.keepObject.GetComponent<FoodItem>().isSliced)
            {
                SpawnFood(GameService.Instance.networkItems.GetNetworkItemSlice(itemType));
                isSlice = true;
            }
            else
            {
                SpawnFood(GameService.Instance.networkItems.GetNetworkFoodItem(itemType));
            }
            DespawnFood(gameObject.GetComponent<NetworkObject>());
            isFull = true;
        }
    }
    [Button]
    public void ChoppingFood(GameObject gameObject)
    {
        circleBarBackground.gameObject.SetActive(true);
        if (!gameObject && isFull && choppingCount < itemChoppingCount)
        {
            choppingCount++;
            circleBarImage.fillAmount = (float)choppingCount / (float)itemChoppingCount;
            circleBarRectTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, ((float)choppingCount / (float)itemChoppingCount) * -360));
        }
        else if(!gameObject && isFull && choppingCount == itemChoppingCount)
        {
            DespawnFood(item);
            SpawnSliceFood(GameService.Instance.networkItems.GetNetworkItemSlice(itemType));
            isSlice = true;
        }
    }
    public void SpawnFood(NetworkObject networkObject)
    {
        item = NetworkManager.Instance.SessionRunner.Spawn(networkObject, itemAnchorPoint.position, this.transform.rotation, Object.InputAuthority);
        item.transform.SetParent(transform);
        item.gameObject.GetComponent<Rigidbody>().useGravity = false;
        item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        item.name = networkObject.name;
        itemChoppingCount = item.GetComponent<FoodItem>().choppingCount;
    }
    public void SpawnSliceFood(NetworkObject networkObject)
    {
        itemSlice = NetworkManager.Instance.SessionRunner.Spawn(networkObject, itemAnchorPoint.position, this.transform.rotation, Object.InputAuthority);
        itemSlice.transform.SetParent(transform);
        itemSlice.gameObject.GetComponent<Rigidbody>().useGravity = false;
        itemSlice.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        itemSlice.name = networkObject.name;
        item = itemSlice;
    }
    public void DespawnFood(NetworkObject networkObject)
    {
        GameService.Instance.playerAction.RPC_Trigger(networkObject);
    }
    public void GrabFood()
    {
        if(isSlice)
        {
            GameService.Instance.playerAction.playerInteraction.PlayerChoppingGrab(GameService.Instance.networkItems.GetNetworkItemSlice(itemType));
        }
        else
        {
            GameService.Instance.playerAction.playerInteraction.PlayerChoppingGrab(GameService.Instance.networkItems.GetNetworkFoodItem(itemType));
        }
        isSlice = false;
    }
    public void InteractChoppingBoard()
    {

        GameService.Instance.playerAction.playerInteractionKitchenObject = this.gameObject;
    }

    public async void StartActivationDelay()
    {
        _isInActivationDelay = true;
        await UniTask.WaitForSeconds(1f, cancellationToken: destroyCancellationToken);
        _isInActivationDelay = false;
    }
}
